using Couche_Métier;
using Couche_Métier.Manager;
using Modeles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Couche_IHM.VueModeles
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region singleton
        private static MainWindowViewModel instance = null;

        public static MainWindowViewModel Instance => instance ?? throw new NullReferenceException("Le MainWindowViewModel doit d'abord être instancié par la fenêtre.");

        public static MainWindowViewModel GetInstanceFor(MainWindow mainWindow)
        {
            if (instance == null)
            {
                instance = new MainWindowViewModel(mainWindow);
            }
            return instance;
        }
        #endregion

        #region viewmodels
        public AccountsViewModel AccountsViewModel { get => accountsViewModel; set => accountsViewModel = value; }
        public ProductsViewModel ProductViewModel { get => productViewModel; set => productViewModel = value; }
        public PartenariatViewModel PartenariatViewModel { get => partenariatViewModel; set => partenariatViewModel = value; }
        public CaisseViewModel CaisseViewModel { get => caisseViewModel; set => caisseViewModel = value; }
        public LogsViewModel LogsViewModel { get => logsViewModel; set => logsViewModel = value; }
        public StatistiqueViewModel StatViewModel { get => statViewModel; set => statViewModel = value; }

        private AccountsViewModel accountsViewModel;
        private ProductsViewModel productViewModel;
        private PartenariatViewModel partenariatViewModel;
        private CaisseViewModel caisseViewModel;
        private LogsViewModel logsViewModel;
        private StatistiqueViewModel statViewModel;
        private MainWindow mainWindow;
        #endregion

        #region notify

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region attributes
        private AccountViewModel compteConnected;
        private Frame frame = Frame.FRAMEACCUEIL;
        private LogManager logManager;
        private AccountManager accountManager;
        private ProductManager productManager;
        private StatAccountManager statAccountManager;
        private StatProduitManager statProduitManager;
        private OrderManager orderManager;
        #endregion

        #region events
        public RelayCommand GoTwitter { get; set; }
        public RelayCommand GoTwitch { get; set; }
        public RelayCommand GoInsta { get; set; }
        public RelayCommand GoDrive { get; set; }
        public RelayCommand ChangeFrame { set; get; }
        #endregion

        #region properties
        /// <summary>
        /// Compte connecté à gallium
        /// </summary>
        public AccountViewModel CompteConnected
        {
            get => compteConnected;
            set => compteConnected = value;
        }

        /// <summary>
        /// Permet de créer des logs
        /// </summary>
        public LogManager LogManager
        {
            get => logManager;
            set => logManager = value;
        }

        public AccountManager AccountManager { get => accountManager; set => accountManager = value; }


        /// <summary>
        /// Représente la frame actuellement affichée
        /// </summary>
        public Frame Frame
        {
            get => frame;
            set
            {
                frame = value;

                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(Uri));
            }
        }

        /// <summary>
        /// L'uri de la frame
        /// </summary>
        public Uri Uri
        {
            get
            {
                return new Uri($"Frames/{frame}.xaml", UriKind.Relative);
            }
        }


        public MainWindow MainWindow => mainWindow;



        #endregion

        /// <summary>
        /// Constructeur du mainwindow vue modele
        /// </summary>
        private MainWindowViewModel(MainWindow mainWindow)
        {
            this.productManager = new ProductManager();
            this.accountManager = new AccountManager();
            this.logManager = new LogManager(this.accountManager);
            this.statAccountManager = new StatAccountManager();
            this.statProduitManager = new StatProduitManager();
            this.orderManager = new OrderManager();
            this.accountsViewModel = new AccountsViewModel(accountManager);
            this.productViewModel = new ProductsViewModel(productManager);
            this.caisseViewModel = new CaisseViewModel(this.statAccountManager, this.statProduitManager, this.orderManager);
            this.statViewModel = new StatistiqueViewModel(productManager, accountManager, statAccountManager, statProduitManager);
            this.logsViewModel = new LogsViewModel(accountManager, logManager);
            this.partenariatViewModel = new PartenariatViewModel();
            this.mainWindow = mainWindow;

            // Initialisation des events
            this.ChangeFrame = new RelayCommand(fram => ChangeFrameInit((Frame)fram));
        }

        /// <summary>
        /// Permet de changer de frame et d'initialiser des données voulues
        /// </summary>
        /// <param name="frame">Nouvelle frame à afficher</param>
        private void ChangeFrameInit(Frame frame)
        {
            // gestion de la sortie de l'ancienne frame
            this.LeaveFrame(this.Frame);
            // gestion de l'entrée sur une nouvelle frame
            this.EnterFrame(frame);
            // changement
            this.Frame = frame;
        }

        /// <summary>
        /// Appelée quand la frame change pour gérer la sortie de la frame.
        /// </summary>
        /// <param name="frame">La frame actuelle qui va être remplacée.</param>
        private void LeaveFrame(Frame frame)
        {
            switch (frame)
            {
                case Frame.FRAMELOG:
                    this.logsViewModel.StopLoading();
                    break;
            }
        }

        /// <summary>
        /// Appelée quand la frame change pour gérer l'entrée de la nouvelle frame.
        /// </summary>
        /// <param name="frame">La nouvelle frame qui va remplacer la frame actuelle.</param>
        private void EnterFrame(Frame frame)
        {
            switch (frame)
            {
                case Frame.FRAMECAISSE:
                    this.productViewModel.SearchFilter = "";
                    this.accountsViewModel.SearchFilter = "";
                    break;

                case Frame.FRAMELOG:
                    this.logsViewModel.ReloadInBackground();
                    break;
            }
        }
    }
}
