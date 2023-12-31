
using Couche_Data.Interfaces;
using Modeles;
using MySql.Data.MySqlClient;

namespace Couche_Data.Dao
{
    public class AccountDAO : IAccountDao
    {

        public void CreateAdhérent(Account adhérent)
        {
            //Connection
            dbsDAO.Instance.OpenDataBase();

            //Requette SQL
            string stm = $"INSERT INTO User VALUES (0,'{adhérent.Identifiant}','{adhérent.Prenom}','{adhérent.Nom}','{adhérent.Mail}',{adhérent.RoleId},'{adhérent.Formation}',{adhérent.Argent},{adhérent.IsMember},' ',' ','2030-11-25 00:00:00')";
            MySqlCommand cmd = new MySqlCommand(stm, dbsDAO.Instance.Sql);
            cmd.Prepare();

            //lecture de la requette
            cmd.ExecuteNonQuery();
            using (MySqlCommand selectCommand = new MySqlCommand("SELECT LAST_INSERT_ID() AS nouvel_id", dbsDAO.Instance.Sql))
            {
                int nouvelId = Convert.ToInt32(selectCommand.ExecuteScalar());
                adhérent.Id = nouvelId;
            }

            dbsDAO.Instance.CloseDatabase();
        }


        public List<Account> GetAdhérents()
        {
            //Connection
            dbsDAO.Instance.OpenDataBase();

            //Requette SQL
            string stm = "SELECT * FROM User ORDER BY userId";
            MySqlCommand cmd = new MySqlCommand(stm, dbsDAO.Instance.Sql);
            cmd.Prepare();

            //lecture de la requette
            MySqlDataReader rdr = cmd.ExecuteReader();

            List<Account> acomptes = new List<Account>();
            while (rdr.Read())
            {
               acomptes.Add(new Account(rdr.GetInt32("id"), rdr.GetString("userId"), rdr.GetString("lastName"), rdr.GetString("firstName"), rdr.GetString("email"), rdr.GetFloat("deposit"),rdr.GetString("year"),rdr.GetBoolean("isMember"), rdr.GetInt16("role")));
            }

            rdr.Close();
            dbsDAO.Instance.CloseDatabase();
            return acomptes;
        }

        public List<Role> GetRoles()
        {
            //Connection
            dbsDAO.Instance.OpenDataBase();

            //Requette SQL
            string stm = "SELECT * FROM Role";
            MySqlCommand cmd = new MySqlCommand(stm, dbsDAO.Instance.Sql);
            cmd.Prepare();

            //lecture de la requette
            MySqlDataReader rdr = cmd.ExecuteReader();

            List<Role> roles = new List<Role>();
            while (rdr.Read())
            {
                roles.Add(new Role(rdr.GetInt32("id"), rdr.GetString("name")));
            }

            rdr.Close();
            dbsDAO.Instance.CloseDatabase();
            return roles;
        }

        public void RemoveAdhérent(Account adhérent)
        {
            //Connection
            dbsDAO.Instance.OpenDataBase();

            //Requette SQL
            string stm = $"DELETE FROM  User WHERE id = {adhérent.Id}";
            MySqlCommand cmd = new MySqlCommand(stm, dbsDAO.Instance.Sql);
            cmd.Prepare();

            //lecture de la requette
            cmd.ExecuteNonQuery();

            dbsDAO.Instance.CloseDatabase();
        }

        public void UpdateAdhérent(Account adhérent)
        {
            //Connection
            dbsDAO.Instance.OpenDataBase();

            //Requette SQL
            
            string formattedAmountMoney = adhérent.Argent.ToString(System.Globalization.CultureInfo.InvariantCulture);
            string stm = $"UPDATE User set lastName = '{adhérent.Nom}', firstName = '{adhérent.Prenom}', deposit = {formattedAmountMoney}, isMember = {adhérent.IsMember}, role = {adhérent.RoleId} , email = '{adhérent.Mail}' ,year = '{adhérent.Formation}',password = '{adhérent.HashedPassword}', userId = '{adhérent.Identifiant}' WHERE id = {adhérent.Id}";
            MySqlCommand cmd = new MySqlCommand(stm, dbsDAO.Instance.Sql);
            cmd.Prepare();

            //lecture de la requette
            cmd.ExecuteNonQuery();

            dbsDAO.Instance.CloseDatabase();

        }
    }
}
