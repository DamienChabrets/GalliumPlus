using Modeles;

namespace Couche_Data.Interfaces
{
    public interface ILogDAO
    {
        void CreateLog(Log log);
        List<Log> GetLogs(int mois, int annee);
        IPaginatedLogReader GetLogsReader(int mois, int annee);
    }
}