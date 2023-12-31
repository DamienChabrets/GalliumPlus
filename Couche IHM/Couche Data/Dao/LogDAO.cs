using Couche_Data.Interfaces;
using Modeles;
using MySql.Data.MySqlClient;

namespace Couche_Data.Dao
{
    public class LogDAO : ILogDAO
    {

        public void CreateLog(Log log)
        {
            string connString = dbsDAO.ConnectionString;
            MySqlConnection sql = new MySqlConnection(connString);

            //Connection
            sql.Open();

            //Requette SQL
            string formattedDate = log.Date.ToString("yyyy-MM-dd HH:mm:ss");
            string stm = $"INSERT INTO logs VALUES(0,@message,'{formattedDate}',{log.Theme},'{log.Auteur}')";
            MySqlCommand cmd = new MySqlCommand(stm, sql);
            cmd.Parameters.AddWithValue("@message", log.Message);
            cmd.Prepare();

            //lecture de la requette
            cmd.ExecuteNonQuery();

            sql.Close();
            
        }


        public List<Log> GetLogs(int mois, int annee)
        {
            //Connection
            string connString = dbsDAO.ConnectionString;
            MySqlConnection sql = new MySqlConnection(connString);
            sql.Open();
            //Requette SQL 
            string stm = $"SELECT * FROM logs WHERE YEAR(date_at) =  {annee} AND MONTH(date_at) = {mois} ORDER BY date_at DESC";
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

        public IPaginatedLogReader GetLogsReader(int mois, int annee)
        {
            return new CachedLogReader(20, this.GetLogs(mois, annee));
        }
    }
}
