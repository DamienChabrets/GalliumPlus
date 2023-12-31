

using Couche_IHM.ImagesProduit;
using Couche_Métier.Manager;
using Couche_Métier.Utilitaire;
using Microsoft.Win32;
using Modeles;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Couche_IHM.VueModeles
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        #region attributes
        private Product product;
        private BitmapImage image;
        private string image2;
        private ProductManager productManager;
        private CategoryManager categoryManager;
        private int quantiteIHM;
        private string nomProduitIHM;
        private CategoryViewModel categoryIHM;
        private string prixAdherentIHM;
        private string prixNonAdherentIHM;
        private string action;
        private bool showConfirmationDelete;
        #endregion

        #region notify
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region events
        public RelayCommand CreateProd { get; set; }
        public RelayCommand ResetProd { get; set; }
        public RelayCommand UpdateProd { get; set; }
        public RelayCommand DeleteProd { get; set; }
        public RelayCommand CancelDeleteProd { get; set; }
        public RelayCommand PreviewDeleteProd { get; set; }

        public RelayCommand ChangeImage { get;set; }
        #endregion

        #region properties

        /// <summary>
        /// Id du produit
        /// </summary>
        public int Id
        {
            get => product.ID;
        }

        public double Opacity
        {
            get
            {
                double d = 1;
                if (!isDisponible)
                {
                    d = 0.5;
                }
                return d;
            }
        }
        /// <summary>
        /// Permet de savoir si le produit est disponible
        /// </summary>
        public bool isDisponible
        {
            get
            {
                return (product.Quantite > 0);
            }
        }

        /// <summary>
        /// Renvoie la quantite du produit
        /// </summary>
        public int QuantiteIHM
        {
            get
            {
                return quantiteIHM;
            }
            set 
            { 
                quantiteIHM = value;
                MainWindowViewModel.Instance.ProductViewModel.ShowModifButtons = true;
            }
        }

        /// <summary>
        /// Nom du produit
        /// </summary>
        public string NomProduitIHM
        {
            get => nomProduitIHM;
            set
            {
                nomProduitIHM = value;
                MainWindowViewModel.Instance.ProductViewModel.ShowModifButtons = true;
            }
        }

        /// <summary>
        /// Prix Adherent formatté pour l'afficher
        /// </summary>
        public string PrixAdherentIHM
        {
            get => prixAdherentIHM;
            set 
            { 
                prixAdherentIHM = value;
            }
        }
        /// <summary>
        /// Prix non adhérent formatté pour l'afficher
        /// </summary>
        public string PrixNonAdherentIHM
        {
            get => prixNonAdherentIHM;
            set 
            { 
                prixNonAdherentIHM = value; 
            }
        }

        /// <summary>
        /// Categorie du produit
        /// </summary>
        public CategoryViewModel CategoryIHM
        {
            get => categoryIHM;
            set 
            { 
                categoryIHM = value;
                MainWindowViewModel.Instance.ProductViewModel.ShowModifButtons = true;
            }
        }
        /// <summary>
        /// Action à réaliser sur le produit
        /// </summary>
        public string Action
        {
            get
            {
                return this.action;
            }
        }

        /// <summary>
        /// Image du produit
        /// </summary>
        public BitmapImage ImageProduct 
        {
            get => image;
            set 
            { 
                image = value;
                NotifyPropertyChanged();
            }
            
        }

        public bool ShowConfirmationDelete
        {
            get => showConfirmationDelete;
            set
            {
                showConfirmationDelete = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region constructor
        /// <summary>
        /// Constructeur du produit vue modele
        /// </summary>
        public ProductViewModel(Product product, ProductManager productManager, CategoryManager categoryManager, CategoryViewModel categoryProduit,string action = "UPDATE")
        {
            // Initialisation du modele
            this.product = product;

            // Initialisation des objets metiers
            this.categoryManager = categoryManager;
            this.productManager = productManager;

            // Initialisation des attributsIHM
            this.action = action;
            this.categoryIHM = categoryProduit;
            this.quantiteIHM = product.Quantite;
            this.nomProduitIHM = product.NomProduit;
            this.image = new BitmapImage(new Uri(ImageManager.GetImageFromProduct(this.NomProduitIHM), UriKind.Absolute));
            this.image2 = image.UriSource.ToString();
            this.prixNonAdherentIHM = ConverterFormatArgent.ConvertToString(product.PrixNonAdherent);
            this.prixAdherentIHM = ConverterFormatArgent.ConvertToString(product.PrixAdherent);

            // Initialisation des events
            this.ResetProd = new RelayCommand(x => ResetProduct());
            this.UpdateProd = new RelayCommand(x => UpdateProduct());
            this.CreateProd = new RelayCommand(x => CreateProduct());
            this.DeleteProd = new RelayCommand(x => DeleteProduct());
            this.PreviewDeleteProd = new RelayCommand(x => ShowConfirmationDelete = true);
            this.CancelDeleteProd = new RelayCommand(x => ShowConfirmationDelete = false);
            this.ChangeImage = new RelayCommand(x => ChangeImageProduct()); 

        }
        #endregion

        #region methods
        /// <summary>
        /// Permet de changer l'image du produit
        /// </summary>
        public void ChangeImageProduct()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg; *.png)|*.jpg; *.png|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string nomImage =openFileDialog.FileName;
                this.ImageProduct = new BitmapImage(new Uri(nomImage, UriKind.Absolute));
                NotifyPropertyChanged(nameof(ImageProduct));
            }
        }
        /// <summary>
        /// Permet de supprimer la catégorie et de le notifier
        /// </summary>
        public void DeleteCatNotify()
        {
            this.categoryIHM = null;
            NotifyPropertyChanged(nameof(this.CategoryIHM));
        }

        /// <summary>
        /// Permet de mettre à jour l'image si elle a été changée
        /// </summary>
        private void ChangeImageIfNeeded()
        {
            // Changer l'image
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (image.UriSource.ToString() != image2)
                {
                    this.image2 = image.UriSource.ToString();
                    byte[] bitsImage = ImageManager.ConvertImageToBlob(image.UriSource.ToString());
                    ImageManager.CreateImageFromBlob(this.nomProduitIHM, bitsImage);
                }
            });

        }

        /// <summary>
        /// Permet de mettre à jour le produit en local
        /// </summary>
        public void UpdateLocalProduct()
        {
            // Changer la data
            this.product.Quantite = this.quantiteIHM;
            this.product.NomProduit = this.nomProduitIHM;
            this.product.Categorie = this.categoryManager.ListAllCategory().Find(x => x.NomCategory == categoryIHM.NomCat).IdCat;
            this.product.PrixAdherent = ConverterFormatArgent.ConvertToDouble(this.prixAdherentIHM);
            this.product.PrixNonAdherent = ConverterFormatArgent.ConvertToDouble(this.prixNonAdherentIHM);
            ChangeImageIfNeeded();

            // Notifier la vue
            NotifyPropertyChanged(nameof(NomProduitIHM));
            NotifyPropertyChanged(nameof(PrixAdherentIHM));
            NotifyPropertyChanged(nameof(QuantiteIHM));
            NotifyPropertyChanged(nameof(CategoryIHM));
            NotifyPropertyChanged(nameof(isDisponible));

        }
        /// <summary>
        /// Permet de mettre à jour visuellement les modifications de l'adhérent
        /// </summary>
        public void UpdateProduct(bool doLog = true)
        {
            if (this.categoryIHM != null)
            {
                UpdateLocalProduct();
                MessageBoxErrorHandler.DoesntThrow(() => this.productManager.UpdateProduct(this.product));

                // Log l'opération
                if (doLog && product.Quantite != this.quantiteIHM)
                {
                    string log;
                    if (product.Quantite > this.quantiteIHM)
                    {
                        log = $"Prélèvement du stock  ( -{product.Quantite - this.quantiteIHM} {product.NomProduit} )";
                    }
                    else
                    {
                        log = $"Ajout du stock ( +{quantiteIHM - product.Quantite} {product.NomProduit} )";
                    }
                    Log log2 = new Log(DateTime.Now, 3, log, MainWindowViewModel.Instance.CompteConnected.NomCompletIHM);
                    MainWindowViewModel.Instance.LogManager.CreateLog(log2);
                    MainWindowViewModel.Instance.LogsViewModel.AddLog(new LogViewModel(log2));

                }
   
                MainWindowViewModel.Instance.ProductViewModel.ShowProductDetail = false;
                MainWindowViewModel.Instance.ProductViewModel.ShowModifButtons = false;
            }
            else
            {
                MessageBox.Show("Vous n'avez pas sélectionné de catégorie");
            }

        }

        /// <summary>
        /// Permet de mettre à jour visuellement les modifications de l'adhérent
        /// </summary>
        public void DeleteProduct()
        {
            // Changer la data
            if (MessageBoxErrorHandler.DoesntThrow(() => this.productManager.RemoveProduct(this.product)))
            {

                // Log l'action
                Log log = new Log(DateTime.Now, 3, $"Suppression du produit : {this.NomProduitIHM}", MainWindowViewModel.Instance.CompteConnected.NomCompletIHM);
                MainWindowViewModel.Instance.LogManager.CreateLog(log);

                // Notifier la vue
                MainWindowViewModel.Instance.ProductViewModel.RemoveProduct(this);
                MainWindowViewModel.Instance.LogsViewModel.AddLog(new LogViewModel(log));
            }
            MainWindowViewModel.Instance.ProductViewModel.ShowProductDetail = false;
            MainWindowViewModel.Instance.ProductViewModel.ShowModifButtons = false;
            this.ShowConfirmationDelete = false;

        }

        /// <summary>
        /// Permet de mettre à jour visuellement les modifications de l'adhérent
        /// </summary>
        public void CreateProduct()
        {
            if (this.categoryIHM != null)
            {

                // Changer la data
                this.product.Quantite = this.quantiteIHM;
                this.product.NomProduit = this.nomProduitIHM;
                byte[] bitsImage = ImageManager .ConvertImageToBlob(image.UriSource.ToString());
                ImageManager.CreateImageFromBlob(this.nomProduitIHM, bitsImage);
                this.product.Categorie = this.categoryManager.ListAllCategory().Find(x => x.NomCategory == categoryIHM.NomCat).IdCat;
                this.product.PrixAdherent = ConverterFormatArgent.ConvertToDouble(this.prixAdherentIHM);
                this.product.PrixNonAdherent = ConverterFormatArgent.ConvertToDouble(this.prixNonAdherentIHM);

                if (MessageBoxErrorHandler.DoesntThrow(() => this.productManager.CreateProduct(this.product)))
                {
                    this.action = "UPDATE";

                    // Log l'action
                    Log log = new Log(DateTime.Now, 3, $"Ajout du produit : {product.NomProduit}", MainWindowViewModel.Instance.CompteConnected.NomCompletIHM);
                    MainWindowViewModel.Instance.LogManager.CreateLog(log);


                    // Notifier la vue
                    MainWindowViewModel.Instance.ProductViewModel.AddProduct(this);
                    NotifyPropertyChanged(nameof(NomProduitIHM));
                    NotifyPropertyChanged(nameof(QuantiteIHM));
                    NotifyPropertyChanged(nameof(CategoryIHM));
                    NotifyPropertyChanged(nameof(isDisponible));
                    MainWindowViewModel.Instance.LogsViewModel.AddLog(new LogViewModel(log));
                }
                MainWindowViewModel.Instance.ProductViewModel.ShowProductDetail = false;
                MainWindowViewModel.Instance.ProductViewModel.ShowModifButtons = false;
            }
            else
            {
                MessageBox.Show("Vous n'avez pas sélectionné de catégorie");
            }
            
        }

        /// <summary>
        /// Permet de reset les propriétés de l'adhérent
        /// </summary>
        public void ResetProduct()
        {
            // Initialisation propriétés
            if(this.categoryIHM != null)
            {
                this.categoryIHM = MainWindowViewModel.Instance.ProductViewModel.Categories.ToList().Find(x => x.Id == product.Categorie);
            }

           
            this.quantiteIHM = product.Quantite;
            this.nomProduitIHM = product.NomProduit;
            this.image = new BitmapImage(new Uri(ImageManager.GetImageFromProduct(this.NomProduitIHM), UriKind.Absolute));
            this.prixNonAdherentIHM = ConverterFormatArgent.ConvertToString(product.PrixNonAdherent);
            this.prixAdherentIHM = ConverterFormatArgent.ConvertToString(product.PrixAdherent);

            // Notifier la vue
            NotifyPropertyChanged(nameof(ImageProduct));
            NotifyPropertyChanged(nameof(CategoryIHM));
            NotifyPropertyChanged(nameof(QuantiteIHM));
            NotifyPropertyChanged(nameof(NomProduitIHM));
            NotifyPropertyChanged(nameof(PrixNonAdherentIHM));
            NotifyPropertyChanged(nameof(PrixAdherentIHM));
            MainWindowViewModel.Instance.ProductViewModel.ShowProductDetail = false;
            MainWindowViewModel.Instance.ProductViewModel.ShowModifButtons = false;
        }

        public override bool Equals(object? obj)
        {
            return obj is ProductViewModel model &&
                   nomProduitIHM == model.nomProduitIHM;
        }

   

        #endregion
    }
}
