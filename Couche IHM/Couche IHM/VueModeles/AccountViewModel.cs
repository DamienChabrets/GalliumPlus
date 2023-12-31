
using Couche_Métier.Manager;
using Couche_Métier.Utilitaire;
using Modeles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Couche_IHM.VueModeles
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        #region attributes

        private Account acompte;
        private AccountManager accountManager;
        private string argentIHM;
        private string identifiantIHM;
        private bool isAdherentIHM;
        private string nomIHM;
        private string prenomIHM;
        private string formationIHM;
        private string action;
        private string email;
        private Role role;
        private string mdpIHM1;
        private string mdpIHM2;
        private bool showConfirmationDelete;


        #endregion

        #region events
        public RelayCommand ShowUpdate { get; set; }
        public RelayCommand ModifyAdherent { get; set; }
        public RelayCommand ResetAdh { get; set; }
        public RelayCommand CreateAdh { get; set; }
        public RelayCommand PreviewAdh { get; set; }
        public RelayCommand DeleteAdh { get; set; }
        public RelayCommand CancelDeleteAdh { get; set; }
        #endregion

        #region notify
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region properties

        /// <summary>
        /// Nom complet de l'acompte
        /// </summary>
        public string NomCompletIHM
        {
            get 
            { 
                string result = $"{acompte.Nom.ToUpper()} {acompte.Prenom}";
                if (result.Trim() == "")
                {
                    result = "----------------------------------";
                }
                return result;
            }
            

        }
        /// <summary>
        /// Id de l'acompte
        /// </summary>
        public int Id
        {
            get => acompte.Id;
        }

        /// <summary>
        /// Renvoie l'argent de l'adhérent sous un string formatté
        /// </summary>
        public string ArgentIHM
        {
            get => argentIHM;
            set
            {
                argentIHM = value;
                MainWindowViewModel.Instance.AccountsViewModel.ShowModifButtons = true;
            }
        }

        /// <summary>
        /// Id de l'adhérent
        /// </summary>
        public string IdentifiantIHM
        {
            get => identifiantIHM;
            set
            {
                identifiantIHM = value;
                MainWindowViewModel.Instance.AccountsViewModel.ShowModifButtons = true;
            }
        }
        /// <summary>
        /// Nom de l'acompte
        /// </summary>
        public string NomIHM { get => nomIHM; set => nomIHM = value; }

        /// <summary>
        /// Prenom de l'acompte
        /// </summary>
        public string PrenomIHM { get => prenomIHM; set => prenomIHM = value; }
        
        /// <summary>
        /// Formation de l'acompte
        /// </summary>
        public string FormationIHM { get => formationIHM; set => formationIHM = value; }

        /// <summary>
        /// Est ce que l'acompte est adhérent
        /// </summary>
        public bool IsAdherentIHM { get => isAdherentIHM; set => isAdherentIHM = value; }

        /// <summary>
        /// Action à réaliser sur l'acompte
        /// </summary>
        public string Action { get => this.action; }

        /// <summary>
        /// Afficher la popup pour supprimer un compte ?
        /// </summary>
        public bool ShowConfirmationDelete 
        { 
            get => showConfirmationDelete;
            set 
            { 
                showConfirmationDelete = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Mail du compte
        /// </summary>
        public string Email { get => email; set => email = value; }

        /// <summary>
        /// Mdp1 du compte
        /// </summary>
        public string MdpIHM1 { get => mdpIHM1; set => mdpIHM1 = value; }
        /// <summary>
        /// Mdp2 du compte
        /// </summary>
        public string MdpIHM2 { get => mdpIHM2; set => mdpIHM2 = value; }
        /// <summary>
        /// Role du compte
        /// </summary>
        public Role Role { get => role; set => role = value; }
        #endregion

        #region constructor
        /// <summary>
        /// Constructeur du acompteViewModel
        /// </summary>
        public AccountViewModel(Account acompte, AccountManager accountManager, string action = "UPDATE")
        {
            this.acompte = acompte;
            this.accountManager = accountManager;
            this.action = action;

            // Initialisation propriétés
            this.argentIHM = ConverterFormatArgent.ConvertToString(acompte.Argent);
            this.identifiantIHM = acompte.Identifiant;
            this.formationIHM = acompte.Formation;
            this.isAdherentIHM = acompte.IsMember;
            this.nomIHM = acompte.Nom;
            this.prenomIHM = acompte.Prenom;
            this.role = accountManager.GetRoles().Find(x => x.Id == acompte.RoleId);
            this.email = acompte.Mail;
            this.mdpIHM1 = acompte.HashedPassword;
            this.mdpIHM2 = acompte.HashedPassword;

            // Initialisation des events
            this.ShowUpdate = new RelayCommand(x =>
            {
                MainWindowViewModel.Instance.AccountsViewModel.CurrentAccount = this;
                MainWindowViewModel.Instance.AccountsViewModel.OpenUserDetails();
            });
            this.ModifyAdherent = new RelayCommand(x => this.UpdateAccount());


            this.ResetAdh = new RelayCommand(x =>
            {
            this.ResetAccount(); 
            this.HideAccountDetails();
            });

            this.CreateAdh = new RelayCommand(x => this.CreateAccount());
            this.PreviewAdh = new RelayCommand(x => ShowConfirmationDelete = true);
            this.CancelDeleteAdh = new RelayCommand(x => ShowConfirmationDelete = false);
            this.DeleteAdh = new RelayCommand(x => this.DeleteAccount());

        }
        #endregion

        #region methods
        /// <summary>
        /// Permet de supprimer un acompte
        /// </summary>
        private void DeleteAccount()
        {
            // Modifier la data
            if (MessageBoxErrorHandler.DoesntThrow(() => this.accountManager.RemoveAdhérent(this.acompte)))
            {
                // Log l'action
                Log log = new Log(DateTime.Now, 2, $"Suppresion du compte : {this.NomCompletIHM}", MainWindowViewModel.Instance.CompteConnected.NomCompletIHM);
                MainWindowViewModel.Instance.LogManager.CreateLog(log);

                // Notifier la vue
                MainWindowViewModel.Instance.AccountsViewModel.RemoveAccount(this);
                MainWindowViewModel.Instance.LogsViewModel.AddLog(new LogViewModel(log));
                MainWindowViewModel.Instance.AccountsViewModel.DialogModifAccount = false;
                MainWindowViewModel.Instance.AccountsViewModel.ShowModifButtons = false;
            }
            ShowConfirmationDelete = false;
        }

        /// <summary>
        /// Permet de créer un acompte
        /// </summary>
        private void CreateAccount()
        {
            // Changer la data
            this.acompte.Nom = this.nomIHM;
            this.acompte.Prenom = this.prenomIHM;
            this.acompte.Argent = ConverterFormatArgent.ConvertToDouble(this.ArgentIHM);
            this.acompte.Formation = this.formationIHM;
            this.acompte.Identifiant = this.identifiantIHM;
            this.acompte.IsMember = this.isAdherentIHM;
            this.acompte.Mail = "UKN";
            this.action = "UPDATE";
            
            if (MessageBoxErrorHandler.DoesntThrow(() => accountManager.CreateAdhérent(this.acompte)))
            {
                // Log l'action
                Log log = new Log(DateTime.Now, 2, $"Création de l acompte : {this.NomCompletIHM}", MainWindowViewModel.Instance.CompteConnected.NomCompletIHM);
                MainWindowViewModel.Instance.LogManager.CreateLog(log);

                // Notifier la vue
                MainWindowViewModel.Instance.AccountsViewModel.AddAccount(this);

                NotifyPropertyChanged(nameof(this.Action));
                NotifyPropertyChanged(nameof(IdentifiantIHM));
                NotifyPropertyChanged(nameof(ArgentIHM));
                NotifyPropertyChanged(nameof(NomCompletIHM));
                NotifyPropertyChanged(nameof(Email));
                NotifyPropertyChanged(nameof(Role));
                NotifyPropertyChanged(nameof(IsAdherentIHM));
                MainWindowViewModel.Instance.LogsViewModel.AddLog(new LogViewModel(log));
            }
            MainWindowViewModel.Instance.AccountsViewModel.DialogModifAccount = false;
            MainWindowViewModel.Instance.AccountsViewModel.ShowModifButtons = false;
        }

        /// <summary>
        /// Permet de mettre à jour visuellement les modifications de l'adhérent
        /// </summary>
        public void UpdateAccount(bool doLog = true, bool persistChanges = true)
        {
            // Log l'action
            float argent = ConverterFormatArgent.ConvertToDouble(this.ArgentIHM);
            if (doLog && acompte.Argent != argent)
            {
                // Si suppresion argent
                string logMessage;
                if (acompte.Argent > argent)
                {
                    logMessage = $"Prélèvement de {ConverterFormatArgent.ConvertToString(acompte.Argent - argent)} sur {acompte.Identifiant}";
                }
                else
                {
                    logMessage = $"Ajout de {ConverterFormatArgent.ConvertToString(argent - acompte.Argent)} sur {acompte.Identifiant}";
                }

                Log log = new Log(DateTime.Now, 2, logMessage, MainWindowViewModel.Instance.CompteConnected.NomCompletIHM);
                MainWindowViewModel.Instance.LogManager.CreateLog(log);
                MainWindowViewModel.Instance.LogsViewModel.AddLog(new LogViewModel(log));
            }

            // Changer la data
            this.acompte.Nom = this.nomIHM;
            this.acompte.Prenom = this.prenomIHM;
            this.acompte.Argent = ConverterFormatArgent.ConvertToDouble(this.ArgentIHM);
            this.acompte.Formation = this.formationIHM;
            this.acompte.Identifiant = this.identifiantIHM;
            this.acompte.IsMember = this.isAdherentIHM;
            this.acompte.Mail = this.email;
            this.acompte.RoleId = this.role.Id;

            if (persistChanges && MessageBoxErrorHandler.DoesntThrow(() => accountManager.UpdateAdhérent(this.acompte)))
            {
                // Notifier la vue
                NotifyPropertyChanged(nameof(IdentifiantIHM));
                NotifyPropertyChanged(nameof(ArgentIHM));
                NotifyPropertyChanged(nameof(NomCompletIHM));
                NotifyPropertyChanged(nameof(Email));
                NotifyPropertyChanged(nameof(Role));
                NotifyPropertyChanged(nameof(IsAdherentIHM));
            }
            MainWindowViewModel.Instance.AccountsViewModel.InitAccountsAdmins();
            MainWindowViewModel.Instance.AccountsViewModel.DialogModifAccount = false;
            MainWindowViewModel.Instance.AccountsViewModel.DialogModifAdmin = false;
            MainWindowViewModel.Instance.AccountsViewModel.ShowModifButtons = false;
        }


        /// <summary>
        /// Permet de reset les propriétés de l'acompte
        /// </summary>
        public void ResetAccount()
        {
            // Initialisation propriétés
            this.argentIHM = ConverterFormatArgent.ConvertToString(acompte.Argent);
            this.identifiantIHM = acompte.Identifiant;
            this.formationIHM = acompte.Formation;
            this.nomIHM = acompte.Nom;
            this.prenomIHM = acompte.Prenom;
            this.isAdherentIHM = acompte.IsMember;
            this.email = acompte.Mail;
            this.role = accountManager.GetRoles().Find(x => x.Id == acompte.RoleId);

            // Notifier la vue
            NotifyPropertyChanged(nameof(IdentifiantIHM));
            NotifyPropertyChanged(nameof(ArgentIHM));
            NotifyPropertyChanged(nameof(NomCompletIHM));
            NotifyPropertyChanged(nameof(NomIHM));
            NotifyPropertyChanged(nameof(Email));
            NotifyPropertyChanged(nameof(Role));
            NotifyPropertyChanged(nameof(PrenomIHM));
            NotifyPropertyChanged(nameof(FormationIHM));
            NotifyPropertyChanged(nameof(IsAdherentIHM));
            MainWindowViewModel.Instance.AccountsViewModel.ShowModifButtons = false;
        }

        /// <summary>
        /// Permet de fermer les popup de modification de comptes
        /// </summary>
        public void HideAccountDetails()
        {
            MainWindowViewModel.Instance.AccountsViewModel.DialogModifAccount = false;
            MainWindowViewModel.Instance.AccountsViewModel.DialogModifAdmin = false;
        }

        public override string? ToString()
        {
            return $"{acompte.Identifiant} {acompte.Nom} {acompte.Prenom}";
        }

        public override bool Equals(object? obj)
        {
            return obj is AccountViewModel model &&
                   EqualityComparer<Account>.Default.Equals(acompte, model.acompte);
        }


        #endregion
    }
}
