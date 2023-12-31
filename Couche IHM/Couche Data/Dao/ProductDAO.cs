
using Couche_Data.Interfaces;
using Modeles;
using MySql.Data.MySqlClient;

namespace Couche_Data.Dao
{
    /// <summary>
    /// Interfaction avec les produits de la base de donnée
    /// </summary>
    public class ProductDAO : IProductDAO
    {

        public void CreateProduct(Product product)
        {
            //Connection
            dbsDAO.Instance.OpenDataBase();

            //Requette SQL
            string stm = $"INSERT INTO Product VALUES (0,'{product.NomProduit}',{product.Quantite},{product.PrixNonAdherent},{product.PrixAdherent},0,{product.Categorie})";
            MySqlCommand cmd = new MySqlCommand(stm, dbsDAO.Instance.Sql);
            cmd.Prepare();

            //lecture de la requette
            cmd.ExecuteNonQuery();
            

            using (MySqlCommand selectCommand = new MySqlCommand("SELECT LAST_INSERT_ID() AS nouvel_id", dbsDAO.Instance.Sql))
            {
                int nouvelId = Convert.ToInt32(selectCommand.ExecuteScalar());
                product.ID = nouvelId;
            }
            dbsDAO.Instance.CloseDatabase();

        }

        public List<Product> GetProducts()
        {
            //Connection
            dbsDAO.Instance.OpenDataBase();

            //Requette SQL
            string stm = "SELECT * FROM Product ORDER BY name";
            MySqlCommand cmd = new MySqlCommand(stm, dbsDAO.Instance.Sql);
            cmd.Prepare();

            //lecture de la requette
            MySqlDataReader rdr = cmd.ExecuteReader();
            List<Product> products = new List<Product>();

            while (rdr.Read())
            {
                int categoryId = rdr.IsDBNull(rdr.GetOrdinal("category")) ? -1 : rdr.GetInt32("category");
                products.Add(new Product(rdr.GetInt32("id"), rdr.GetString("name"), rdr.GetInt32("stock"), rdr.GetFloat("nonMemberPrice"), rdr.GetFloat("memberPrice"), categoryId));
            }
            rdr.Close();
            dbsDAO.Instance.CloseDatabase();
            return products;
        }

        public void RemoveProduct(Product product)
        {
            //Connection
            dbsDAO.Instance.OpenDataBase();

            //Requette SQL
            string stm = $"DELETE FROM Product WHERE id = {product.ID}";
            MySqlCommand cmd = new MySqlCommand(stm, dbsDAO.Instance.Sql);
            cmd.Prepare();

            //lecture de la requette
            cmd.ExecuteNonQuery();

            dbsDAO.Instance.CloseDatabase();
        }

        public void UpdateProduct(Product product)
        {

            //Connection
            dbsDAO.Instance.OpenDataBase();

            //Requette SQL
            string prixA = product.PrixAdherent.ToString(System.Globalization.CultureInfo.InvariantCulture);
            string prixNA = product.PrixNonAdherent.ToString(System.Globalization.CultureInfo.InvariantCulture);
            string stm = $"UPDATE Product SET name = '{product.NomProduit}', stock = {product.Quantite}, memberPrice = {prixA}, nonMemberPrice = {prixNA}, category = {product.Categorie} WHERE id = {product.ID}";
            MySqlCommand cmd = new MySqlCommand(stm, dbsDAO.Instance.Sql);
            cmd.Prepare();

            //lecture de la requette
            cmd.ExecuteNonQuery();

            dbsDAO.Instance.CloseDatabase();

        }

    }
}
