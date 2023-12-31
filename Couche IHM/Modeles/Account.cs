
namespace Modeles
{
    public class Account
    {
        #region attributes
        private int id;
        private string identifiant;
        private string nom = "";
        private string prenom = "";
        private string mail = "";
        private float argent;
        private bool isMember;
        private string year;
        private int roleId;
        private string hashedPassword;
        #endregion

        #region constructeurs
        /// <summary>
        /// Constructeur de la classe adhérent
        /// </summary>
        public Account(int id, string identifiant, string nom, string prenom,string mail, float argent,string year,bool isMember = true,int role = 2)
        {
            this.id = id;
            this.identifiant = identifiant;
            this.nom = nom;
            this.prenom = prenom;
            this.argent = argent;
            this.year = year;
            this.mail = mail;
            this.roleId = role;
            this.isMember = isMember;
        }

 
        /// <summary>
        /// Constructeur vide pour créer des adhérents
        /// </summary>
        public Account()
        {
            this.isMember = true;
            this.roleId = 2;
            this.argent = 0;
        }

        #endregion

        #region properties
        /// <summary>
        /// Id de l'identifiant dans la bdd
        /// </summary>
        public int Id { get => id; set => id = value;}

        /// <summary>
        /// Id de l'adhérent
        /// </summary>
        public string Identifiant { get => identifiant; set => identifiant = value; }

        /// <summary>
        /// Nom de l'adhérent
        /// </summary>
        public string Nom { get => nom; set => nom = value; }

        /// <summary>
        /// Prénom de l'adhérent
        /// </summary>
        public string Prenom { get => prenom; set => prenom = value; }


        /// <summary>
        /// Argent de l'adhérent
        /// </summary>
        public float Argent { get => argent; set => argent = value; }

        /// <summary>
        /// Est ce que le compte est toujours adhérent
        /// </summary>
        public bool IsMember { get => isMember; set => isMember = value; }

        /// <summary>
        /// Formation de l'adhérent
        /// </summary>
        public string Formation { get => year; set => year = value; }
        public int RoleId { get => roleId; set => roleId = value; }
        public string Mail { get => mail; set => mail = value; }
        public string HashedPassword { get => hashedPassword; set => hashedPassword = value; }

        public override bool Equals(object? obj)
        {
            return obj is Account account &&
                   identifiant == account.identifiant;
        }

        #endregion

    }
}
