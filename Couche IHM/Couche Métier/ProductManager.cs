﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Couche_Métier
{
    public class ProductManager
    {
        private List<Product> products;

        public ProductManager()
        {
            this.products = new List<Product>();
        }

        /// <summary>
        /// Ajoute un produit
        /// </summary>
        public void CreateProduct(Product p)
        {
            products.Add(p);
        }

        /// <summary>
        /// Retire un produit
        /// </summary>
        public void RemoveProduct(Product p)
        {
            products.Remove(p); 
        }

        /// <summary>
        /// Update un produit
        /// </summary>
        public void UpdateProduct()
        {

        }

        /// <summary>
        /// Cherche un produit
        /// </summary>
        /// <param name="productName"> nom du produit </param>
        /// <returns> produit </returns>
        public Product GetProduct(string productName)
        {
            Product produit = null;
            foreach(Product product in products)
            {
                if(product.NomProduit == productName)
                    produit = product;
            }
            return produit;
        }
        
        /// <summary>
        /// Retourne la liste des produits
        /// </summary>
        /// <returns> Liste de produit </returns>
        public List<Product> GetProducts()
        {
            return this.products;
        }
    }
}
