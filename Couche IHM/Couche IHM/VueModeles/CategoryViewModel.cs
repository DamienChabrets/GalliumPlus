
using Couche_Métier.Manager;
using Modeles;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Couche_IHM.VueModeles
{
    public class CategoryViewModel : INotifyPropertyChanged
    {

        #region inotify
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region attributes
        private CategoryManager categoryManager;
        private Category category;
        private string nomCat;
        private bool invisible;
        #endregion

        #region properties

        /// <summary>
        /// Id de la catégorie
        /// </summary>
        public int Id { get => category.IdCat; }
        /// <summary>
        /// Représente le modèle du nom de la catégorie
        /// </summary>
        public string NomCat 
        {
            get => nomCat;
            set 
            { 
                nomCat = value; 
            }
        }

        /// <summary>
        /// Est ce que la catégory est visible sur la caisse
        /// </summary>
        public bool Invisible 
        { 
            get => invisible;
            set 
            { 
                invisible = value;
                NotifyPropertyChanged();
            }

        }

        #endregion

        #region events
        public RelayCommand UpdateCat { get; set; }
        public RelayCommand DeleteCat { get; set; }


        public RelayCommand ActivateCat { get; set; }
        public Category Category { get => category; set => category = value; }
        #endregion

        #region constructor
        /// <summary>
        /// Constructeur du category vue modele
        /// </summary>
        public CategoryViewModel(CategoryManager categoryManager,Category category)
        {
            // Initialisation des datas
            this.categoryManager = categoryManager;
            this.nomCat = category.NomCategory;
            this.category = category;
            this.invisible = !category.Visible;

            // Initialisation des events
            this.UpdateCat = new RelayCommand(x => this.UpdateCategory());
            this.DeleteCat = new RelayCommand(x => this.DeleteCategory());
            this.ActivateCat = new RelayCommand(x => this.ActivateCategory());

        }
        #endregion

        #region methods

        /// <summary>
        /// Permet de rendre visible ou invisible une catégory
        /// </summary>
        public void ActivateCategory()
        {
            // Mise à jour data
            category.Visible = true;
            // TOUJOURS VISIBLE AHAHAHHAHA this.categoryManager.UpdateCategory(category);
        }

        /// <summary>
        /// Permet de mettre à jour une category
        /// </summary>
        public void UpdateCategory()
        {
            // Mise à jour data
            category.NomCategory = this.nomCat;
            MessageBoxErrorHandler.DoesntThrow(()=> this.categoryManager.UpdateCategory(category));
        }



        /// <summary>
        /// Permet de reset les propriétés de la catégorie
        /// </summary>
        public void ResetCategory()
        {
            // Mise à jour data
            this.nomCat = this.category.NomCategory;
            NotifyPropertyChanged(nameof(this.NomCat));
            MainWindowViewModel.Instance.ProductViewModel.ShowCategories = false;
        }

        /// <summary>
        /// Permet de supprimer une catégorie
        /// </summary>
        public void DeleteCategory()
        {
            // Mise à jour data
            if (MessageBoxErrorHandler.DoesntThrow(() => this.categoryManager.DeleteCategory(category)))
            {
                foreach (ProductViewModel prod in MainWindowViewModel.Instance.ProductViewModel.Products.ToList().FindAll(x => x.CategoryIHM == this))
                {
                    prod.DeleteCatNotify();
                }

                // Notifier la vue
                MainWindowViewModel.Instance.ProductViewModel.Categories.Remove(this);
            }
        }



        public override bool Equals(object? obj)
        {
            return obj is CategoryViewModel model &&
                   category == model.category;
        }
        #endregion


    }
}
