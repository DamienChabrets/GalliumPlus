﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Gallium_v1.Data
{
    /// <summary>
    /// Classe qui permet de créer une connexion avec la database
    /// </summary>
    public class dbsDAO
    {
        #region attribut
        private MySqlConnection sql;
        private static dbsDAO instance = null;
        private MySqlCommand cmd;
        private static MySqlDataReader reader;
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
        }

        /// <summary>
        /// permet de lire les données
        /// </summary>
        public static MySqlDataReader Reader
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

        /// <summary>
        /// Permet la connexion
        /// </summary>
        private dbsDAO()
        {
            try
            {
                sql = new MySqlConnection($"SERVER={InformationConnexion.Server};PORT={InformationConnexion.Port};UID={InformationConnexion.Uid};PWD={InformationConnexion.Pwd};DATABASE={InformationConnexion.Databases};SSLMODE=NONE");
                this.sql.Open();
                isConnected = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Problème connexion à database");
            }

        }

        /// <summary>
        /// Modifie la base de donnée
        /// </summary>
        /// <param name="requete"> requete sql </param>
        public void RequeteSQL(string requete)
        {
            cmd = new MySqlCommand(requete, sql);
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear(); 
        }

        /// <summary>
        /// Interroge la base de donnée
        /// </summary>
        /// <param name="requete"> reuqete sql </param>
       public string FetchSQL(string requete)
       {
            cmd = new MySqlCommand(requete, sql);
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();

            return cmd.ToString();
        }
    }
}
