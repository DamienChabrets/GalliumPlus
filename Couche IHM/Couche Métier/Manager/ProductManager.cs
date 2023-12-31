
using Couche_Data.Dao;
using Couche_Data.Interfaces;
using GalliumPlusApi.Dao;
using Modeles;


namespace Couche_Métier.Manager
{
    /// <summary>
    /// Manager de produit qui gère la DATA et le METIER
    /// </summary>
    public class ProductManager
    {
        #region attributes
        /// <summary>
        /// Liste des produits
        /// </summary>
        private List<Product> products;

        /// <summary>
        /// Dao permettant de gérer les données des produits
        /// </summary>
        private IProductDAO productDAO;
        #endregion

        #region constructor
        /// <summary>
        /// Constructeur de Product Manager
        /// </summary>
        public ProductManager()
        {
            if (DevelopmentInfo.isDevelopment)
            {
                this.productDAO = new ProductDAO();
            }
            else
            {
                this.productDAO = new ProductDao();
            }
            
            this.products = new List<Product>(this.productDAO.GetProducts());
        }
        #endregion

        #region methods
        /// <summary>
        /// Ajoute un produit
        /// </summary>
        public void CreateProduct(Product p)
        {
            products.Add(p);
            productDAO.CreateProduct(p);
        }

        /// <summary>
        /// Retire un produit
        /// </summary>
        public void RemoveProduct(Product p)
        {
            products.Remove(p); 
            productDAO.RemoveProduct(p);
        }

        /// <summary>
        /// Update un produit
        /// </summary>
        public void UpdateProduct(Product p)
        {
            Product actalProduit = this.products.Find(x => p.ID == x.ID);
            actalProduit.NomProduit = p.NomProduit;
            actalProduit.PrixNonAdherent = p.PrixNonAdherent;
            actalProduit.PrixAdherent = p.PrixAdherent;
            actalProduit.Quantite = p.Quantite;
            actalProduit.Categorie = p.Categorie;
            productDAO.UpdateProduct(p);

        }

        /// <summary>
        /// Retourne la liste des produits
        /// </summary>
        /// <returns> Liste de produit </returns>
        public List<Product> GetProducts()
        {
            return this.products;
        }

        #endregion
    }
}
