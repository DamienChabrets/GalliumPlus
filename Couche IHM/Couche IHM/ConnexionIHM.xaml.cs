
using Couche_IHM.ImagesProduit;
using Couche_IHM.VueModeles;
using Couche_Métier;
using Couche_Métier.Manager;
using MaterialDesignThemes.Wpf;
using Modeles;
using System;
using System.Windows;
using System.Windows.Input;



namespace Couche_IHM
{
    /// <summary>
    /// Logique d'interaction pour ConnexionIHM.xaml
    /// </summary>
    public partial class ConnexionIHM : Window
    {
        #region attributes
        private LogManager logManager;
        private string identifiant = "";
        private string password = "";
        private SnackbarMessageQueue messageQueue;
        #endregion

        public ConnexionIHM()
        {
            InitializeComponent();
            DataContext = this;
            this.messageQueue = new SnackbarMessageQueue(new TimeSpan(0, 0, 2));
            ImageManager.VerifyFiles();
            this.IsVisibleChanged += ConnexionIHM_IsVisibleChanged; ;
        }

        private void ConnexionIHM_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible == true)
            {
                foreach (var window in Application.Current.Windows)
                {
                    if (window is MainWindow mainWindow)
                    {
                        mainWindow.Close();
                    }
                }
            }
        }

        #region properties
        /// <summary>
        /// Identifiant de connexion
        /// </summary>
        public string Identifiant { get => identifiant; set => identifiant = value; }
        /// <summary>
        /// Mot de passe de connexion
        /// </summary>
        public string Password { get => password; set => password = value; }
        /// <summary>
        /// Snackbar pour les informations
        /// </summary>
        public SnackbarMessageQueue MessageQueue { get => messageQueue; set => messageQueue = value; }
        #endregion


        /// <summary>
        /// Permet de se connecter à son compte et de créer la mainWindows
        /// </summary>
        private void ConnectToAccount(object sender, RoutedEventArgs e)
        {      
            try
            {
                if (password == "" || identifiant == "")
                {
                    throw new Exception("Vous devez remplir les deux champs !");
                }

                Account user = AccountManager.ConnectCompte(identifiant, password);

                MainWindow mainWindow = new MainWindow(user);
                mainWindow.Show();
                this.Close();

                // Log de la connexion
                logManager = MainWindowViewModel.Instance.LogManager;
                Log log = new Log(DateTime.Now, 1, $"Connexion de {user.Prenom} {user.Nom}", $"{user.Prenom} {user.Nom}");
                logManager.CreateLog(log);
                MainWindowViewModel.Instance.LogsViewModel.AddLog(new LogViewModel(log));
                    
            }
            catch(Exception ex)
            {
                messageQueue?.Enqueue(ex.Message);
            }                
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.ConnectToAccount(sender, e);
            }
        }
    }
}
