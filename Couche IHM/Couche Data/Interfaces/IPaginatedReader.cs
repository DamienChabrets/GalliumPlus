using Modeles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Couche_Data.Interfaces
{
    /// <summary>
    /// Permet de récupérer les logs page par page.
    /// </summary>
    public interface IPaginatedLogReader
    {
        /// <summary>
        /// Le nombre d'éléments par page.
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// Charge la page suivante s'il reste des logs à charger
        /// </summary>
        void LoadNextPage();

        /// <summary>
        /// Récupère les logs de manière asynchrone au fur et à mesure qu'ils sont chargés.
        /// </summary>
        /// <returns>Un flux de logs.</returns>
        IAsyncEnumerable<Log> GetAsyncStream(CancellationToken ct = default);

        /// <summary>
        /// Réinitialise le lecteur au début.
        /// </summary>
        void Reset();
    }
}
