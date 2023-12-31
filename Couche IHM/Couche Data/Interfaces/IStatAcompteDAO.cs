using Modeles;

namespace Couche_Data.Interfaces
{
    public interface IStatAccountDAO
    {
        void CreateStat(StatAccount stat);
        List<StatAccount> GetStat();
    }
}