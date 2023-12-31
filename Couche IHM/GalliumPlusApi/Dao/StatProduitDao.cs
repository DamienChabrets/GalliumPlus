using Couche_Data.Dao;
using Couche_Data.Interfaces;
using Modeles;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace GalliumPlusApi.Dao
{
    public class StatProduitDao : IStatProduitDAO
    {

        public List<StatProduit> GetStat()
        {
            MySqlConnection sql = new MySqlConnection(dbsDAO.ConnectionStringV);
            try
            {
                sql.Open();

                //Requette SQL
                string stm = "SELECT product_id,sum(number_sales) as nb FROM best_sales WHERE YEARWEEK(date_sale) = YEARWEEK(CURRENT_DATE) group by product_id order by nb desc";
                MySqlCommand cmd = new MySqlCommand(stm, sql);
                cmd.Prepare();

                //lecture de la requette
                MySqlDataReader rdr = cmd.ExecuteReader();

                List<StatProduit> statProduitList = new List<StatProduit>();
                while (rdr.Read())
                {
                    statProduitList.Add(new StatProduit(0, DateTime.Now, rdr.GetInt16("nb"), rdr.GetInt16("product_id")));
                }

                rdr.Close();
                sql.Close();
                return statProduitList;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur pendant le chargement des stats produit : {ex.Message}");
                return new();
            }
        }

        public void CreateStat(StatProduit stat)
        {
            //Connection
            MySqlConnection sql = new MySqlConnection(dbsDAO.ConnectionStringV);
            try
            {              
                sql.Open();

                //Requette SQL
                string formattedDate = stat.Date.ToString("yyyy-MM-dd");
                string stm = $"INSERT INTO best_sales VALUES(0,'{formattedDate}',{stat.Number_sales},{stat.Product_id})";
                MySqlCommand cmd = new MySqlCommand(stm, sql);
                cmd.Prepare();

                //lecture de la requette
                cmd.ExecuteNonQuery();

                sql.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur pendant l'ajout d'une stat produit : {ex.Message}");
            }
        }
    }
}
