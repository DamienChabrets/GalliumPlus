
namespace Modeles
{
    public class StatAccount
    {
        #region attributes
        private int id;
        private int acompte_id;
        private float money;
        private DateTime date;
        #endregion

        #region constructor
        /// <summary>
        /// Constructeur du modèle statacompte
        /// </summary>
        /// <param name="id">id de la stat</param>
        /// <param name="date">date de la stat</param>
        /// <param name="money">argent de la stat</param>
        /// <param name="acompte_id">numéro de l'acompte</param>
        public StatAccount(int id, DateTime date, float money, int acompte_id)
        {
            this.acompte_id = acompte_id;
            this.money = money;
            this.id = id;
            this.date = date;
        }
        #endregion

        #region properties
        public int Account_Id { get => acompte_id; set => acompte_id = value; }
        public float Money { get => money; set => money = value; }
        public int Id { get => id; set => id = value; }
        public DateTime Date { get => date; set => date = value; }
        #endregion
    }
}
