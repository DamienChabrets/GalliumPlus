﻿using Modeles;
using MySql.Data.MySqlClient;

namespace Couche_Data.Dao
{
    public class LogDAO
    {

        public void CreateLog(Log log)
        {
            lock (dbsDAO.Instance.DatabaseLock)
            {
                //Connection
                dbsDAO.Instance.OpenDataBase();

                //Requette SQL
                string formattedDate = log.Date.ToString("yyyy-MM-dd HH:mm:ss");
                string stm = $"INSERT INTO logs VALUES(0,'{log.Message}','{formattedDate}',{log.Theme},'{log.Auteur}')";
                MySqlCommand cmd = new MySqlCommand(stm, dbsDAO.Instance.Sql);
                cmd.Prepare();

                //lecture de la requette
                cmd.ExecuteNonQuery();

                dbsDAO.Instance.CloseDatabase();
            }
        }


        public List<Log> GetLogs(int mois,int annee)
        {
            //Connection
            string connString = String.Format("server={0};port={1};user id={2};password={3};database={4};SslMode={5}", "51.178.36.43", "3306", "c2_gallium", "DfD2no5UJc_nB", "c2_gallium", "none");
            MySqlConnection sql = new MySqlConnection(connString);
            sql.Open();
            //Requette SQL 
            string stm = $"SELECT * FROM Logs WHERE YEAR(date_at) =  {annee} AND MONTH(date_at) = {mois} ORDER BY date_at DESC";
            MySqlCommand cmd = new MySqlCommand(stm, sql);
            cmd.Prepare();

            //lecture de la requette
            MySqlDataReader rdr = cmd.ExecuteReader();

            List<Log> logs = new List<Log>();
            while (rdr.Read())
            {
                logs.Add(new Log(DateTime.Parse(rdr.GetString("date_at")), rdr.GetInt16("log_category_id"), rdr.GetString("text"), rdr.GetString("user")));
            }
            rdr.Close();
            sql.Close();
            return logs;
        }

       



    }
}
