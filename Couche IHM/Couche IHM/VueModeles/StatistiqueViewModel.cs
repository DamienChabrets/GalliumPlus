
using Couche_Métier.Manager;
using MaterialDesignThemes.Wpf;
using Modeles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace Couche_IHM.VueModeles
{
    public class StatistiqueViewModel : INotifyPropertyChanged
    {

        #region attributes
        private StatProduitManager statProduitManager;
        private AccountManager accountManager;
        private List<StatProduitViewModel> statsProduit = new List<StatProduitViewModel>();

        private StatAccountManager statAccountManager;
        private ProductManager productManager;
        private List<StatAccountViewModel> statsAccount = new List<StatAccountViewModel>();
        #endregion

        #region inotify
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region properties
        public int FirstPurchaseCount
        {
            get
            {
                return PodiumProduits[0].PurchaseCount;
            }
        }
        public float FirstArgent
        {
            get
            {
                return PodiumAccount[0].Argent;
            }
        }

        public List<StatAccountViewModel> BestAcomptes
        {
            get
            {
                return this.statsAccount.OrderByDescending(x => x.Argent).Reverse().ToList();
            }
        }

        public List<StatProduitViewModel> BestProducts
        {
            get
            {
                return this.statsProduit.OrderByDescending(x => x.PurchaseCount).Reverse().ToList();
            }
        }
        /// <summary>
        /// Podium des trois meilleurs produits
        /// </summary>
        public List<StatProduitViewModel> PodiumProduits
        {
            get
            {
                return statsProduit.OrderByDescending(x => x.PurchaseCount).Take(4).ToList();
            }
        }

        /// <summary>
        /// Podium des trois meilleurs acomptes
        /// </summary>
        public List<StatAccountViewModel> PodiumAccount
        {
            get
            {
                return statsAccount.OrderByDescending(x => x.Argent).Take(4).ToList();
            }
        }

        #endregion

        #region events
        public RelayCommand FindAccount { get; set;}
        public RelayCommand FindProduct { get; set;}
        #endregion
        #region constructor
        /// <summary>
        /// Constructeur du statistique vue modele
        /// </summary>
        public StatistiqueViewModel(ProductManager produtManager,AccountManager accountManager,StatAccountManager statAccount,StatProduitManager statProduit)
        {
            // Initialisation des objets métiers
            this.statProduitManager = statProduit;
            this.statAccountManager = statAccount;
            this.productManager = produtManager;
            this.accountManager = accountManager;

            // Initialisation des events
            this.FindProduct = new RelayCommand(product => GoToProductDetails((ProductViewModel)product));
            this.FindAccount = new RelayCommand(acompte => GoToAccountDetails((AccountViewModel)acompte));

            // Initialisation des datas
            InitStatsProduit();
            InitStatsAccount();
            
        }
        #endregion

        #region methods

        /// <summary>
        /// Permet de retrouver l'acompte en liant avec la stat et de redirigier sur ses détails
        /// </summary>
        private void GoToAccountDetails(AccountViewModel acompte)
        {
            MainWindowViewModel.Instance.Frame = Frame.FRAMEACCOUNT;
            MainWindowViewModel.Instance.AccountsViewModel.CurrentAccount = MainWindowViewModel.Instance.AccountsViewModel.Accounts.ToList().Find(x => x.Id == acompte.Id);
        }
        /// <summary>
        /// Permet de retrouver le produit en liant avec la stat et de redirigier sur ses détails
        /// </summary>
        private void GoToProductDetails(ProductViewModel product)
        {
            MainWindowViewModel.Instance.Frame = Frame.FRAMESTOCK;
            MainWindowViewModel.Instance.ProductViewModel.CurrentProduct = MainWindowViewModel.Instance.ProductViewModel.GetProducts().Find(x => x.Id == product.Id);
        }

        /// <summary>
        /// Permet d'ajouter une stat de produit
        /// </summary>
        public void AddStatProduit(StatProduit stat)
        {
            StatProduitViewModel? produitStat = this.statsProduit.Find(x => x.ProductViewModel.Id == stat.Product_id);
            if( produitStat != null)
            {
                produitStat.PurchaseCount += stat.Number_sales;
            }
            else
            {
                this.statsProduit.Add(new StatProduitViewModel(stat, new ProductViewModel(productManager.GetProducts().Find(x => x.ID == stat.Product_id),null,null,null)));
            }
            NotifyPropertyChanged(nameof(this.PodiumProduits));
        }

        /// <summary>
        /// Permet d'ajouter une stat d'acompte
        /// </summary>
        public void AddStatAccount(StatAccount stat)
        {
            StatAccountViewModel? acompteStat = this.statsAccount.Find(x => x.AccountsViewModel.Id == stat.Account_Id);
            if (acompteStat != null)
            {
                acompteStat.Argent += stat.Money;
            }
            else
            {
                this.statsAccount.Add(new StatAccountViewModel(stat, new AccountViewModel(accountManager.GetAdhérents().Find(x => x.Id == stat.Account_Id),this.accountManager)));
            }
            NotifyPropertyChanged(nameof(this.PodiumAccount));
        }

        /// <summary>
        /// Permet d'inistaliser les stats des produits
        /// </summary>
        public void InitStatsProduit()
        {
            this.statsProduit.Clear();
            List<StatProduit> statProduit = this.statProduitManager.GetStats();
            foreach (StatProduit stat in statProduit)
            {
                if (productManager.GetProducts().Find(x => x.ID == stat.Product_id) is Product productLogic)
                {
                    this.statsProduit.Add(new StatProduitViewModel(stat, new ProductViewModel(productLogic, null, null, null)));
                }
            }
            NotifyPropertyChanged(nameof(this.PodiumProduits));
        }


        /// <summary>
        /// Permet d'initialiser les stats des acomptes
        /// </summary>
        public void InitStatsAccount()
        {
            this.statsAccount.Clear();
            List<StatAccount> statAccount = this.statAccountManager.GetStats();
            foreach (StatAccount stat in statAccount)
            {
                if (accountManager.GetAdhérents().Find(x => x.Id == stat.Account_Id) is Account acompte)
                {
                    this.statsAccount.Add(new StatAccountViewModel(stat, new AccountViewModel(acompte, this.accountManager)));
                }
            }
            NotifyPropertyChanged(nameof(this.PodiumAccount));
        }
        #endregion
    }
}
