using Couche_Data.Interfaces;
using Couche_Métier;
using Couche_Métier.Manager;
using Modeles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Couche_IHM.VueModeles
{
    public class LogsViewModel : INotifyPropertyChanged
    {

        #region attributes 
        private ObservableCollection<LogViewModel> logs = new ObservableCollection<LogViewModel>();
        private AccountManager userManager;
        private LogManager logManager;
        private string currentAuteur;
        private int currentAnnee;
        private string currentMois;
        private List<string> mois = new List<string>()
        {
            "janvier",
            "février",
            "mars",
            "avril",
            "mai",
            "juin",
            "juillet",
            "août",
            "septembre",
            "octobre",
            "novembre",
            "décembre"
        };
        private List<int> annees;
        private List<int> themeLog;
        private bool selectVente;
        private bool selectConnexion;
        private bool selectProduct;
        private bool selectAccount;
        private bool selectCompte;
        private CancellationTokenSource reloadCTS;
        private DateTime nextPageLoadTime;
        private TimeSpan pageLoadCooldown;
        private IPaginatedLogReader? logsReader;
        #endregion

        #region constructor

        /// <summary>
        /// Constructeur du logs vue modele
        /// </summary>
        public LogsViewModel(AccountManager userManager,LogManager logManager)
        {
            // Initialisation des datas
            this.themeLog = new List<int>();
            this.userManager = userManager;
            this.logManager = logManager;
            DateTime currentDate = DateTime.Now;
            currentAuteur = Auteurs[0];
            int année = Convert.ToInt16(currentDate.ToString("yyyy"));
            this.annees = new List<int>() { année, année - 1, année - 2 };
            currentMois = currentDate.ToString("MMMM");
            currentAnnee = année;
            this.SelectVente = true;
            this.SelectAccount = false;
            this.SelectConnexion = false;
            this.SelectProduct = false;
            this.SelectCompte = false;
            this.reloadCTS = new CancellationTokenSource();
            this.pageLoadCooldown = TimeSpan.FromMilliseconds(100);
        }
        #endregion

        #region properties
        /// <summary>
        /// Auteur sélectionné
        /// </summary>
        public string CurrentAuteur
        {
            get => currentAuteur;
            set
            {
                currentAuteur = value;
                this.ReloadFiltersInBackground();
            }
        }
        /// <summary>
        /// Liste des auteurs potentiels
        /// </summary>
        public List<string> Auteurs
        {
            get
            {
                List<string> auteurs = new List<string>() { "Tout le monde" };
                List<Account> users = this.userManager.GetAdmins();
                foreach (Account u in users)
                {
                    auteurs.Add($"{u.Prenom} {u.Nom}");
                }
                return auteurs;
            }
        }

        /// <summary>
        /// Liste des logs
        /// </summary>
        public ObservableCollection<LogViewModel> Logs
        {
            get => this.logs;
            set => logs = value;
        }

        /// <summary>
        /// Filtre afficher les ventes
        /// </summary>
        public bool SelectVente
        {
            get => selectVente;
            set
            {
                if (value)
                {
                    this.themeLog.Add(5);
                }
                else
                {
                    this.themeLog.Remove(5);
                }
                selectVente = value;
                this.ReloadFiltersInBackground();
            }
        }

        /// <summary>
        /// Filtre afficher les connexions
        /// </summary>
        public bool SelectConnexion
        {
            get => selectConnexion;
            set
            {
                if (value)
                {
                    this.themeLog.Add(1);
                }
                else
                {
                    this.themeLog.Remove(1);
                }
                selectConnexion = value;
                this.ReloadFiltersInBackground();
            }
        }
        /// <summary>
        /// Filtre afficher les produits
        /// </summary>
        public bool SelectProduct
        {
            get => selectProduct;
            set
            {
                if (value)
                {
                    this.themeLog.Add(3);
                }
                else
                {
                    this.themeLog.Remove(3);
                }
                selectProduct = value;
                this.ReloadFiltersInBackground();
            }
        }

        /// <summary>
        /// Filtre afficher les acomptes
        /// </summary>
        public bool SelectAccount 
        { 
            get => selectAccount;
            set
            {
                if (value)
                {
                    this.themeLog.Add(2);
                }
                else
                {
                    this.themeLog.Remove(2);
                }
                selectAccount = value;
                this.ReloadFiltersInBackground();
            }
        }

        /// <summary>
        /// Filtre afficher les comptes
        /// </summary>
        public bool SelectCompte
        {
            get => selectCompte;
            set
            {
                if (value)
                {
                    this.themeLog.Add(6);
                }
                else
                {
                    this.themeLog.Remove(6);
                }
                selectCompte = value;
                this.ReloadFiltersInBackground();
            }
        }
        /// <summary>
        /// Liste des mois disponibles
        /// </summary>
        public List<string> Mois { get => mois; set => mois = value; }

        /// <summary>
        /// Liste des années disponibles
        /// </summary>
        public List<int> Annees { get => annees; set => annees = value; }

        /// <summary>
        /// Mois sélectionné
        /// </summary>
        public string CurrentMois
        {
            get => currentMois;
            set
            {
                currentMois = value;
                //InitLogs(this.mois.IndexOf(currentMois) + 1, this.currentAnnee);
                this.ReloadInBackground();
            }
        }

        /// <summary>
        /// Annee selectionné
        /// </summary>
        public int CurrentAnnee
        {
            get => currentAnnee;
            set
            {
                currentAnnee = value;
                //InitLogs(this.mois.IndexOf(currentMois) + 1, this.currentAnnee);
                this.ReloadInBackground();
            }
        }
        #endregion

        #region notify
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region methods
        /// <summary>
        /// Permet d'initialiser la liste des logs
        /// </summary>
        public void InitLogs(int mois = 0, int annee = 0)
        {
            List<Log> logs = this.logManager.GetLogs(mois, annee);
            foreach (Log log in logs)
            {
                this.logs.Add(new LogViewModel(log));
            }

        }

        /// <summary>
        /// Permet d'ajouter un log
        /// </summary>
        /// <param name="log">og à ajouter</param>
        public void AddLog(LogViewModel log)
        {

            if (mois.IndexOf(currentMois) + 1 == Convert.ToInt16(log.DateTime.ToString("MM")) && currentAnnee == Convert.ToInt16(log.DateTime.ToString("yyyy")))
            {
                this.logs.Insert(0, log);
            }

        }

        /// <summary>
        /// Indique si une entrée doit être affiché ou non.
        /// </summary>
        /// <param name="log">L'entrée à tester.</param>
        /// <returns><see langword="true"/> si l'entrée peut être affichée.</returns>
        private bool Filtrer(Log log)
        {
            if (!themeLog.Contains(log.Theme))
            {
                return false;
            }
            if (currentAuteur != "Tout le monde" && log.Auteur != currentAuteur)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Lance le chargement des logs en arrière-plan.
        /// </summary>
        public void ReloadInBackground()
        {
            this.StopLoading();
            this.reloadCTS = new();
            this.logsReader = this.logManager.GetLogsReader(this.mois.IndexOf(currentMois) + 1, this.currentAnnee);
            Task.Run(() => this.ReloadAsync(this.reloadCTS.Token));
        }

        /// <summary>
        /// Recharge les logs en changeant uniquement les filtres.
        /// </summary>
        public void ReloadFiltersInBackground()
        {
            // rien gardé en mémoire, on recharge tout
            if (this.logsReader == null)
            {
                this.ReloadInBackground();
                return;
            }

            this.StopLoading();
            this.reloadCTS = new();
            this.logsReader.Reset();
            Task.Run(() => this.ReloadAsync(this.reloadCTS.Token));

        }

        /// <summary>
        /// Arrête le chargement des logs.
        /// </summary>
        public void StopLoading()
        {
            this.reloadCTS?.Cancel();
        }

        /// <summary>
        /// Charge les logs suivants.
        /// </summary>
        public void LoadNextPage()
        {
            if (DateTime.Now > this.nextPageLoadTime)
            {
                this.logsReader?.LoadNextPage();
                this.nextPageLoadTime = DateTime.Now + this.pageLoadCooldown;
            }
        }

        /// <summary>
        /// Charge les logs.
        /// </summary>
        /// <param name="ct">Un jeton d'annulation pour arrêter le chargement en cours.</param>
        private async Task ReloadAsync(CancellationToken ct = default)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.logs.Clear();
            });

            if (this.logsReader == null) return;

            int read = 0;
            int added = 0;
            await foreach (Log log in this.logsReader!.GetAsyncStream(ct))
            {
                read++;
                if (this.Filtrer(log))
                {
                    added++;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        this.logs.Add(new LogViewModel(log));
                    });
                }

                // on a traité une page entière
                if (read == this.logsReader.PageSize)
                {
                    // si des éléments se sont fait filtrer, on charge la suivante
                    if (added < this.logsReader.PageSize)
                    {
                        read = 0;
                        this.logsReader!.LoadNextPage();
                    }
                    else
                    {
                        added = 0;
                    }
                }
            }
        }

        #endregion
    }
}
