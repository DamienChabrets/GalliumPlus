using Couche_Data.Interfaces;
using GalliumPlusApi.Dao;
using Modeles;

namespace Couche_Métier.Manager
{
    /// <summary>
    /// Manager des catégories 
    /// </summary>
    public class CategoryManager
    {
        #region attributes
        /// <summary>
        /// Dao permettant de gérer les données des catégories
        /// </summary>
        private ICategoryDao iCategory;

        /// <summary>
        /// Liste des catégories
        /// </summary>
        private List<Category> categories;

        #endregion

        #region constructor
        /// <summary>
        /// Constructeur du manager des catégories
        /// </summary>
        public CategoryManager() 
        {
            this.iCategory = new CategoryDao();
            categories = this.iCategory.ListALlCategory();
        }
        #endregion

        #region methods
        /// <summary>
        /// Permet de créer une catégorie
        /// </summary>
        /// <param name="ca">catégorie à créer</param>
        public void CreateCategory(Category cat)
        {
            this.iCategory.CreateCategory(cat);
            this.categories.Add(cat);
        }

        /// <summary>
        /// Permet de supprimer une catégorie
        /// </summary>
        /// <param name="ca">catégorie à supprimer</param>
        public void DeleteCategory(Category cat)
        {
            this.iCategory.DeleteCategory(cat);
            this.categories.Remove(cat);
        }

        /// <summary>
        /// Permet de mettre à jour une catégorie
        /// </summary>
        /// <param name="bases"></param>
        /// <param name="news"></param>
        public void UpdateCategory(Category cat)
        {
            this.iCategory.UpdateCategory(cat);
            Category category = this.categories.Find(x => x.IdCat == cat.IdCat);
            category.NomCategory = cat.NomCategory;
            category.Visible = cat.Visible;
        }

        /// <summary>
        /// Liste les catégories
        /// </summary>
        public List<Category> ListAllCategory()
        {
            return categories;
        }

        #endregion
    }
}
