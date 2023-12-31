using MySql.Data.MySqlClient;
using System.Reflection;

namespace Couche_Data.Dao
{

    /// <summary>
    /// Classe qui permet de créer une connexion avec la database
    /// </summary>
    /// <Author> Damien C.</Author>
    public class dbsDAO
    {
        #region attribut
        private MySqlConnection sql;
        private static dbsDAO instance = null;
        private MySqlCommand cmd;
        private MySqlDataReader reader;
        private static bool isConnected;
        #endregion

        /// <summary>
        /// Singleton qui permet d'avoir qu'une connexion
        /// </summary>
        public static dbsDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new dbsDAO();

                }
                return instance;
            }
        }

        /// <summary>
        /// Permet de faire des requêtes
        /// </summary>
        public MySqlCommand CMD
        {
            get => cmd;
            set => cmd = value;
        }

        /// <summary>
        /// permet de lire les données
        /// </summary>
        public MySqlDataReader Reader
        {
            get => reader;
            set => reader = value;
        }

        /// <summary>
        /// Vérifie si la connexion à la bdd existe 
        /// </summary>
        public static bool IsConnected
        {
            get => isConnected;
        }
        public MySqlConnection Sql { get => sql; set => sql = value; }

        /// <summary>
        /// Permet la connexion au serveur
        /// </summary>
        private dbsDAO()
        {
            this.ConnexionToBdd();

        }
        private object databaseLock = new object();

        public object DatabaseLock
        {
            get { return databaseLock; }
        }

        private static string? connectionString;

        public static string ConnectionString
        {
            get
            {
                if (connectionString == null)
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("ConnectionString.txt"));
                    using Stream stream = assembly.GetManifestResourceStream(resourceName)!;
                    using StreamReader reader = new StreamReader(stream);
                    connectionString = reader.ReadToEnd();
                }
                return connectionString;
            }
        }
        public static string ConnectionStringV
        {
            get
            {
                if (connectionString == null)
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("ConnectionString.txt"));
                    using Stream stream = assembly.GetManifestResourceStream(resourceName)!;
                    using StreamReader reader = new StreamReader(stream);
                    connectionString = reader.ReadToEnd();
                    connectionString = connectionString.Replace("database=c2_gallium", "database=c2_etismash"); // TODO enlever cette ligne quand stat avec api
                }
                return connectionString;
            }
        }




        /// <summary>
        /// Se connecte à la base de donnée
        /// </summary>
        private void ConnexionToBdd()
        {
            sql = new MySqlConnection(ConnectionString);
        }

        /// <summary>
        /// Ouvre la connexion
        /// </summary>
        public void OpenDataBase()
        {
            if (!isConnected)
            {
                sql.Open();
                isConnected = true;
            }

        }

        /// <summary>
        /// Ferme la connexion 
        /// </summary>
        public void CloseDatabase()
        {
            if (isConnected)
            {
                sql.Close();
                isConnected = false;
            }
        }
    }

}
