using Couche_Data.Interfaces;
using Modeles;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Couche_Data.Dao
{
    public class OrderDAO : IOrderDao
    {
        public void ProcessOrder(Order order)
        {
            //Connection
            dbsDAO.Instance.OpenDataBase();

            float prix = 0;
            //Requette SQL
            for (int i=0; i<order.OrderedProducts.Count; i++)
            {

                string stm1 = $"Select stock,memberPrice from Product WHERE id = {order.OrderedProducts.ElementAt(i).Key}";
                MySqlCommand cmd = new MySqlCommand(stm1, dbsDAO.Instance.Sql);
                cmd.Prepare();

                //lecture de la requette
                MySqlDataReader rdr = cmd.ExecuteReader();

                int newStock = 0;
                while (rdr.Read())
                {
                    newStock = rdr.GetInt16(0);
                    prix += rdr.GetFloat(1);
                }
                newStock -= order.OrderedProducts.ElementAt(i).Value;
                rdr.Close();
                string stm = $"UPDATE Product SET stock  = {newStock} WHERE id = {order.OrderedProducts.ElementAt(i).Key}";
                MySqlCommand cmd2 = new MySqlCommand(stm, dbsDAO.Instance.Sql);
                cmd2.Prepare();

                //lecture de la requette
                cmd2.ExecuteNonQuery();
            }
                
            
            if (order.Customer != null)
            {
                string stm1 = $"Select deposit from User WHERE userId = '{order.Customer}'";
                MySqlCommand cmd = new MySqlCommand(stm1, dbsDAO.Instance.Sql);
                cmd.Prepare();

                //lecture de la requette
                MySqlDataReader rdr = cmd.ExecuteReader();

                float newArgent = 0;
                while (rdr.Read())
                {
                    newArgent = rdr.GetFloat(0);
                }

                newArgent = newArgent - prix;
                string argent = newArgent.ToString(System.Globalization.CultureInfo.InvariantCulture);
                rdr.Close();
                string stm = $"UPDATE User SET deposit = {argent} WHERE userId = '{order.Customer}'";
                MySqlCommand cmd2 = new MySqlCommand(stm, dbsDAO.Instance.Sql);
                cmd2.Prepare();

                //lecture de la requette
                cmd2.ExecuteNonQuery();
            }
            

            dbsDAO.Instance.CloseDatabase();
        }
    }
}
