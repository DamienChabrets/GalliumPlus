using GalliumPlusApi.CompatibilityHelpers;
using GalliumPlusApi.ModelDecorators;
using Modeles;

namespace GalliumPlusApi.Dto
{
    public class ProductSummary
    {
        public int Id { get; set; } = -1;
        public string Name { get; set; } = "";
        public int Stock { get; set; } = 0;
        public decimal NonMemberPrice { get; set; } = -1;
        public decimal MemberPrice { get; set; } = -1;
        public string Availability { get; set; } = "";
        public int Category { get; set; } = -1;

        public class Mapper : Mapper<Product, ProductSummary>
        {
            public override ProductSummary FromModel(Product model)
            {
                string availability = "AUTO";
                if (model is DecoratedProduct deco)
                {
                    availability = deco.Availability;
                }

                return new ProductSummary
                {
                    Id = model.ID,
                    Name = model.NomProduit,
                    Stock = model.Quantite,
                    NonMemberPrice = Format.FloatToMonetary(model.PrixNonAdherent)!.Value,
                    MemberPrice = Format.FloatToMonetary(model.PrixAdherent)!.Value,
                    Availability = availability,
                    Category = model.Categorie
                };
            }

            public ProductSummary PatchWithModel(ProductDetails originalProduct, Product patch)
            {
                return new ProductSummary
                {
                    Id = patch.ID,
                    Name = patch.NomProduit,
                    Stock = patch.Quantite,
                    NonMemberPrice = Format.FloatToMonetary(patch.PrixNonAdherent)!.Value,
                    MemberPrice = Format.FloatToMonetary(patch.PrixAdherent)!.Value,
                    Availability = originalProduct.Availability,
                    Category = patch.Categorie
                };
            }

            public override Product ToModel(ProductSummary dto)
            {
                return new Product(
                    id: dto.Id,
                    nomProduit: dto.Name,
                    quantite: dto.Stock,
                    prixAdherent: (float)dto.MemberPrice,
                    prixNonAdherent: (float)dto.NonMemberPrice,
                    categorie: dto.Category
                );
            }
        }
    }
}