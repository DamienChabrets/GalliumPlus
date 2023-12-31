
using Couche_Métier.Manager;
using DocumentFormat.OpenXml.Office2016.Presentation.Command;
using Modeles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Couche_IHM.VueModeles
{
    public class AccountsViewModel : INotifyPropertyChanged
    {
        #region attributes
        private ObservableCollection<AccountViewModel> accounts;
        private ObservableCollection<AccountViewModel> accountsAdmins;
        private List<Role> roles;
        private AccountViewModel currentAccount;
        private AccountManager accountManager;
        private string searchFilter = "";
        private bool showAccount = false;
        private bool showModifButtons = false;
        private bool showDeleteAccount = false;
        private bool dialogModifAccount = false;

        private bool dialogModifAdmin = false;
        #endregion

        #region notify
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region events
        public RelayCommand OpenModifAdh { get; set; }
        public RelayCommand OpenModifAdmin { get; set; }

        #endregion

        #region properties
        /// <summary>
        /// Liste des roles disponibles
        /// </summary>
        public List<Role> Roles { get => roles; }

        /// <summary>
        /// Liste des accounts
        /// </summary>
        public ObservableCollection<AccountViewModel> Accounts 
        {
            get 
            {
                ObservableCollection<AccountViewModel> adhs;
                if (searchFilter == "")
                {
                    adhs = this.accounts;
                }
                else
                {
                    adhs = new ObservableCollection<AccountViewModel>(accounts.ToList().FindAll(adh =>
                    adh.NomCompletIHM.ToUpper().Contains(searchFilter.ToUpper()) ||
                    adh.IdentifiantIHM.ToUpper().Contains(searchFilter.ToUpper()))) ;
                }
                
                return adhs;
            }
        }

        /// <summary>
        /// Liste des accounts
        /// </summary>
        public ObservableCollection<AccountViewModel> AccountsAdmins
        {
            get
            {
                return accountsAdmins;
            }
        }

        /// <summary>
        /// Filtre de barre de recherche
        /// </summary>
        public string SearchFilter 
        { 
            get => searchFilter;
            set 
            { 
                searchFilter = value; 
                NotifyPropertyChanged(nameof(Accounts)); 
            } 
        }

        /// <summary>
        /// Est ce qu'on affiche la fenetre de l account
        /// </summary>
        public bool ShowAccount
        { 
            get => showAccount;
            set 
            {
                showAccount = value; 
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Permet d'afficher les boutons de modification de l'account
        /// </summary>
        public bool ShowModifButtons
        { 
            get => showModifButtons;
            set 
            {
                showModifButtons = value; 
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Représente l'account selectionné
        /// </summary>
        public AccountViewModel CurrentAccount 
        { 
            get => currentAccount;
            set 
            {
                if (currentAccount != null)
                {
                    this.currentAccount.ResetAccount();
                }

                currentAccount = value;
                if (value != null)
                {
                    ShowAccount = true;

                }
                else
                {
                    this.ShowAccount = false;
                    
                }

                NotifyPropertyChanged();

            }
        }
        /// <summary>
        /// Ouvrir la fenetre pour modifier l'account
        /// </summary>
        public bool DialogModifAccount
        {
            get => dialogModifAccount;
            set
            {
                dialogModifAccount = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Permet d'afficher le bouton de suppresion
        /// </summary>
        public bool ShowDeleteAccount 
        { 
            get => showDeleteAccount;
            set 
            {
                if (MainWindowViewModel.Instance.CompteConnected.Role.Name != "Conseil d'administration")
                {
                    showDeleteAccount = value;
                    NotifyPropertyChanged();
                }
                    
            }

        }

        /// <summary>
        /// Permet d'afficher la popup de modification de compte admin
        /// </summary>
        public bool DialogModifAdmin 
        { 
            get => dialogModifAdmin; 
            set 
            { 
                dialogModifAdmin = value; 
                NotifyPropertyChanged(); 
            } 
        }


        #endregion

        #region constructor
        /// <summary>
        /// Constructeur de la classe account view model
        /// </summary>
        public AccountsViewModel(AccountManager accountManager)
        {
            // Initialisation des datas
            this.accountManager = accountManager;
            this.accounts = new ObservableCollection<AccountViewModel>();
            this.accountsAdmins = new ObservableCollection<AccountViewModel>();
            this.roles = accountManager.GetRoles();
            InitAccountsAcomptes();
            InitAccountsAdmins();

            // Initialisation des events
            this.OpenModifAdh = new RelayCommand(x => this.OpenAccountDetails((string)x));
            this.OpenModifAdmin = new RelayCommand(x => this.OpenUserDetails());


        }
        #endregion

        #region methods
        private class AccountComparer : IComparer<Account>
        {
            public int Compare(Account? x, Account? y)
            {
                return StringComparer.Ordinal.Compare(x?.Identifiant, y?.Identifiant);
            }
        }

        /// <summary>
        /// Permet de récupérer la liste des accounts
        /// </summary>
        private void InitAccountsAcomptes()
        {
            List<Account> adherents = this.accountManager.GetAdhérents();
            adherents.Sort(new AccountComparer());
            foreach (Account adh in adherents)
            {
                this.accounts.Add(new AccountViewModel(adh,this.accountManager));
            }
        }

        /// <summary>
        /// Permet de récupérer la liste des accounts
        /// </summary>
        public void InitAccountsAdmins()
        {
            this.accountsAdmins.Clear();
            List<Account> admins = this.accountManager.GetAdmins();
            admins.Sort(new AccountComparer());
            foreach (Account adh in admins)
            {
                this.accountsAdmins.Add(new AccountViewModel(adh, this.accountManager));
            }
            NotifyPropertyChanged(nameof(this.accountsAdmins));
        }

        /// <summary>
        /// Permet d'ouvrir les détails du compte
        /// </summary>
        public void OpenUserDetails()
        {
            if (currentAccount == null)
            {
                CurrentAccount = new AccountViewModel(new Account(), this.accountManager,"NEW");
            }

            DialogModifAdmin = true;
        }

        /// <summary>
        /// Permet d'ouvrir le détail de l'account
        /// </summary>
        private void OpenAccountDetails(string action)
        {

            if (action == "NEW" || currentAccount == null || currentAccount.Action == "NEW")
            {
                ShowDeleteAccount = false;
                CurrentAccount = new AccountViewModel(new Account(),this.accountManager,"NEW");
            }
            else
            {
                ShowDeleteAccount = true;
            }

            DialogModifAccount = true;
        }

        /// <summary>
        /// Permet de rajouter un account  dans la liste
        /// </summary>
        public void AddAccount(AccountViewModel account)
        {
            this.accounts.Add(account);
            NotifyPropertyChanged(nameof(Accounts));

        }

        /// <summary>
        /// Permet de supprimer un account  dans la liste
        /// </summary>
        public void RemoveAccount(AccountViewModel account)
        {
            this.accounts.Remove(account);
            NotifyPropertyChanged(nameof(Accounts));
        }

        #endregion


    }
}
