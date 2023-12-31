using Couche_Data.Dao;
using Couche_Data.Interfaces;
using DocumentFormat.OpenXml.Spreadsheet;
using GalliumPlusApi.Dao;
using Modeles;

namespace Couche_Métier.Manager
{
    public class AccountManager
    {
        #region attributes
        /// <summary>
        /// Dao permettant de gérer les données des acomptes
        /// </summary>
        private IAccountDao adhérentDao;

        /// <summary>
        /// Liste des acomptes
        /// </summary>
        private List<Account> adhérents = new List<Account>();

        /// <summary>
        /// Liste des rôles
        /// </summary>
        private List<Role> roles;
        #endregion

        #region constructor
        /// <summary>
        /// Constructeur de la classe adhérentManager
        /// </summary>
        public AccountManager()
        {
            if (DevelopmentInfo.isDevelopment)
            {
                this.adhérentDao = new AccountDAO();
            }
            else
            {
                this.adhérentDao = new AccountDao();
            }
            

            // Récupération des adhérents
            this.roles = adhérentDao.GetRoles();
            this.adhérents = adhérentDao.GetAdhérents();
        }
        #endregion

        #region methods
        /// <summary>
        /// Permet de récupérer la liste des roles
        /// </summary>
        /// <returns>les roles</returns>
        public List<Role> GetRoles()
        {
            return this.roles;
        }
        /// <summary>
        /// Permet de créer un nouvel adhérent
        /// </summary>
        /// <param name="adhérent">adhérent à créer</param>
        public void CreateAdhérent(Account adhérent)
        {
            adhérentDao.CreateAdhérent(adhérent);
            adhérents.Add(adhérent);
        }

        /// <summary>
        /// Permet de supprimer un adhérent
        /// </summary>
        /// <param name="adhérent">adhérent à supprimer</param>
        public void RemoveAdhérent(Account adhérent)
        {
            adhérentDao.RemoveAdhérent(adhérent);
            adhérents.Remove(adhérent);
        }


        /// <summary>
        /// Permet de mettre à jour les adhérents
        /// </summary>
        /// <param name="adhérent">adhérent à modifier</param>
        public void UpdateAdhérent(Account adhérent)
        {
            adhérentDao.UpdateAdhérent(adhérent);
            Account adhér = adhérents.Find(adh => adh.Id == adhérent.Id);
            adhér.Nom = adhérent.Nom;
            adhér.Prenom = adhérent.Prenom;
            adhér.Mail = adhérent.Mail;
            adhér.RoleId = adhérent.RoleId;
            adhér.Identifiant = adhérent.Identifiant;
            adhér.Argent = adhérent.Argent;
            adhér.IsMember = adhérent.IsMember;
            adhér.Formation = adhérent.Formation;
        }


       /// <summary>
       /// Permet de récupérer tous les adhérents
       /// </summary>
       /// <returns>tous les adhérents</returns>
        public List<Account> GetAdhérents()
        {
            return this.adhérents;
        }

        /// <summary>
        /// Permet de récupérer tous les membres du CA / Bureau
        /// </summary>
        /// <returns>tous les membres du bureau et du ca</returns>
        public List<Account> GetAdmins()
        {
            return this.adhérents.FindAll(x => x.RoleId != 2);
        }

        /// <summary>
        /// Permet de se connecter à un utilisatuer
        /// </summary>
        /// <param name="identifiant"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static Account ConnectCompte(string identifiant, string password)
        {
            SessionDao dao = new();

            var result = dao.LogIn(identifiant, password);
            Account? user = result?.Item1;

            if (user == null)
            {
                throw new Exception("Mauvaise combinaison identifiant/mot de passe");
            }

            Role? role = result?.Item2;
            user.RoleId = role.Id;
   
            if (role?.Name == "Adhérent")
            {
                throw new Exception("Le compte n'a pas les permissions pour se connecter");
            }

            DevelopmentInfo.isDevelopment = user.RoleId == 11 ? true : false;

            return user;
        }

        #endregion

    }
}
