using GalliumPlusApi.Dao;
using Couche_Data.Interfaces;
using Modeles;
using Couche_Data.Dao;

namespace Couche_Métier.Manager
{
    public class StatProduitManager
    {
        #region attributes
        /// <summary>
        /// Dao permettant de gérer les données des stats produits
        /// </summary>
        private IStatProduitDAO dao;

        /// <summary>
        /// Liste des stats des produits
        /// </summary>
        private List<StatProduit> statProduitList;
        #endregion

        #region constructor
        /// <summary>
        /// Constructeur du statProduit manager
        /// </summary>
        public StatProduitManager()
        {
            if (DevelopmentInfo.isDevelopment)
            {
                dao = new StatProduitDAO();
            }
            else
            {
                dao = new StatProduitDao();
            }
           

        }
        #endregion

        #region methods
        public void CreateStat(StatProduit stat)
        {
            dao.CreateStat(stat);
        }

        public List<StatProduit> GetStats()
        {
            statProduitList = dao.GetStat();
            return this.statProduitList;
        }
        #endregion
    }
}
