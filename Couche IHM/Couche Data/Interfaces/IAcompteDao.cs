
using Modeles;

namespace Couche_Data.Interfaces
{

    public interface IAccountDao
    {

        /// <summary>
        /// Permet de récupérer tous les adhérents
        /// </summary>
        public List<Account> GetAdhérents();

        /// <summary>
        /// Permet de récupérer tous les roles
        /// </summary>
        public List<Role> GetRoles();

        /// <summary>
        /// Permet de supprimer un adhérent
        /// </summary>
        public void RemoveAdhérent(Account adhérent);


        /// <summary>
        /// Permet de créer un nouvel adhérent
        /// </summary>
        public void CreateAdhérent(Account adhérent);

        /// <summary>
        /// Permet de modifier un adhérent
        /// </summary>
        public void UpdateAdhérent(Account adhérent);
    }
}
