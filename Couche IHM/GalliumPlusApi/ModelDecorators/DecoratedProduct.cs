using Modeles;

namespace GalliumPlusApi.ModelDecorators
{
    internal class DecoratedProduct : Product
    {
        private string availability;

        public string Availability => availability;

        public DecoratedProduct(
            int id,
            string nomProduit,
            int quantite,
            float prixAdherent,
            float prixNonAdherent,
            int categorie,
            string availability
        )
        : base (id, nomProduit, quantite, prixAdherent, prixNonAdherent, categorie)
        {
            this.availability = availability;
        }
    }
}
