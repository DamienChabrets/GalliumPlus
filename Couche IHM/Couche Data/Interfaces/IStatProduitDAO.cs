using Modeles;

namespace Couche_Data.Interfaces
{
    public interface IStatProduitDAO
    {
        void CreateStat(StatProduit stat);
        List<StatProduit> GetStat();
    }
}