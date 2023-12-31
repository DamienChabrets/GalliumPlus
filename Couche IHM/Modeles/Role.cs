
namespace Modeles
{
    public class Role
    {

        private int id;
        private string name;

        /// <summary>
        /// Constructeur du modèle role
        /// </summary>
        /// <param name="id">id du role</param>
        /// <param name="name">nom du role</param>
        public Role(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        /// <summary>
        /// Id du role
        /// </summary>
        public int Id { get => id; set => id = value; }
        /// <summary>
        /// Nom du role
        /// </summary>
        public string Name { get => name; set => name = value; }

        public override bool Equals(object? obj)
        {
            return obj is Role role &&
                   name == role.name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string? ToString()
        {
            return Name;
        }
    }
}
