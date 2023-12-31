namespace Modeles
{
    public class Category
    { 
        #region attributes
        private int idCat;
        private string nomCategory;
        private bool visible;
        #endregion

        #region constructors
        /// <summary>
        /// Constructeur du modèle category
        /// </summary>
        /// <param name="idCat">id de la categorie</param>
        /// <param name="nomCategory">nom de la categorie</param>
        /// <param name="visible">visibilite de la categorie</param>
        public Category(int idCat, string nomCategory,bool visible)
        {
            this.idCat = idCat;
            this.nomCategory = nomCategory;
            this.visible=visible;
        }
        #endregion

        #region properties
        /// <summary>
        /// Id de la catégorie
        /// </summary>
        public int IdCat { get => idCat; set => idCat = value; }
        /// <summary>
        /// Nom de la catégorie
        /// </summary>
        public string NomCategory { get => nomCategory; set => nomCategory = value; }
        /// <summary>
        /// Visibilité de la catégorie
        /// </summary>
        public bool Visible { get => visible; set => visible = value; }
        #endregion
    }
}
