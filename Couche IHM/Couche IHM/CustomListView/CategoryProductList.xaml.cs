
using Couche_IHM.VueModeles;
using Modeles;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Couche_IHM.CustomListView
{
    /// <summary>
    /// Logique d'interaction pour CategoryProductList.xaml
    /// </summary>
    public partial class CategoryProductList : UserControl
    {
        #region attributes
        private List<DetailedProduct> listProductView = new List<DetailedProduct>();
        private string category;
        #endregion

        #region properties
        public string Category { get { return category; } set => category = value; }
        public List<DetailedProduct> ListProductView { get => listProductView; }

        #endregion

        /// <summary>
        /// Constructeur de la classe CategoryProductList
        /// </summary>
        /// <param name="category"></param>
        /// <param name="produits"></param>
        public CategoryProductList(string category)
        {
            InitializeComponent();
            DataContext = this;
            this.Category = category;
            List<ProductViewModel> produits = MainWindowViewModel.Instance.ProductViewModel.Products.ToList().FindAll(prod => prod.CategoryIHM != null && prod.isDisponible == true && prod.CategoryIHM.NomCat == category);
            foreach (ProductViewModel p in produits)
            {
                DetailedProduct dp = new DetailedProduct(p);
                listProductView.Add(dp);
            }
        }

        private void FilterDisponibility(object sender, System.Windows.RoutedEventArgs e)
        {
            bool activate = ((ToggleButton)sender).IsChecked.Value;
            this.listProductView.Clear();
            if (activate)
            {
                List<ProductViewModel> produits = MainWindowViewModel.Instance.ProductViewModel.Products.ToList().FindAll(prod => prod.CategoryIHM != null && prod.isDisponible== true && prod.CategoryIHM.NomCat == category);
                foreach (ProductViewModel p in produits)
                {
                    DetailedProduct dp = new DetailedProduct(p);
                    listProductView.Add(dp);
                }
            }
            else
            {
                List<ProductViewModel> produits = MainWindowViewModel.Instance.ProductViewModel.Products.ToList().FindAll(prod => prod.CategoryIHM != null && prod.CategoryIHM.NomCat == category);
                foreach (ProductViewModel p in produits)
                {
                    DetailedProduct dp = new DetailedProduct(p);
                    listProductView.Add(dp);
                }
            }
            produits.ItemsSource = null;
            produits.ItemsSource = this.listProductView;
        }
    }
}
