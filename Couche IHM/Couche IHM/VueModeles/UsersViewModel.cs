﻿
using Couche_Métier.Manager;
using Modeles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Couche_IHM.VueModeles
{
    public class UsersViewModel : INotifyPropertyChanged
    {

        #region notify
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region attributes
        private List<Role> roles;
        private ObservableCollection<UserViewModel> users = new ObservableCollection<UserViewModel>();
        private UserManager userManager;

        private UserViewModel currentUser;

        private bool showDeleteUser;
        private bool showModifCreateUser = false;

       
        #endregion

        #region properties
        /// <summary>
        /// Liste des utilisateurs
        /// </summary>
        public ObservableCollection<UserViewModel> Users
        {
            get => users;
            set => users = value;
        }

        /// <summary>
        /// Permet de montrer la fenetre pour modifier et créer un user
        /// </summary>
        public bool ShowModifCreateUser
        {
            get => showModifCreateUser;
            set 
            {
                showModifCreateUser = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Liste des roles disponibles aux users
        /// </summary>
        public List<Role> Roles { get => roles; set => roles = value; }

        /// <summary>
        /// Utilisateur sélectionné
        /// </summary>
        public UserViewModel CurrentUser 
        { 
            get => currentUser;
            set 
            { 
                currentUser = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Permet d'afficher l'option pour supprimer un utilisateur
        /// </summary>
        public bool ShowDeleteUser
        {
            get => showDeleteUser;
            set 
            {
                showDeleteUser = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region events
        public RelayCommand OpenUser { get; set; }
        #endregion

        #region constructor
        /// <summary>
        /// Constructeur de l'utilisateur vue modele
        /// </summary>
        public UsersViewModel(UserManager userManager)
        {
            // Initialisation des datas
            this.userManager = userManager;
            this.roles = userManager.GetRoles();
            InitUsers();

            // Initialisation des events
            this.OpenUser = new RelayCommand(x => this.OpenUserDetails((string)x));
        }
        #endregion

        #region methods

        /// <summary>
        /// Permet de récupérer la liste des users
        /// </summary>
        private void InitUsers()
        {
            List<User> users = this.userManager.GetComptes();

            foreach (User user in users)
            {
                this.users.Add(new UserViewModel(user, this.userManager));
            }
        }

        /// <summary>
        /// Permet d'ouvrir les détails du compte
        /// </summary>
        public void OpenUserDetails(string action)
        {
            if (action == "NEW")
            {
                ShowDeleteUser = false;
                CurrentUser = new UserViewModel(new User(), this.userManager);
                CurrentUser.Action = "NEW";
            }
            else
            {
                ShowDeleteUser = true;
            }

            ShowModifCreateUser = true;
        }


        /// <summary>
        /// Permet d'ajouter un compte
        /// </summary>
        public void AddUser(UserViewModel user)
        {
            this.users.Add(user);
            NotifyPropertyChanged(nameof(Users));

        }

        /// <summary>
        /// Permet de supprimer un compte
        /// </summary>
        /// <param name="user"></param>
        public void RemoveUser(UserViewModel user)
        {
            this.users.Remove(user);
            NotifyPropertyChanged(nameof(Users));
        }

        #endregion
    }
}