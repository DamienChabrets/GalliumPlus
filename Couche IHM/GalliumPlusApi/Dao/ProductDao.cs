using Couche_Data.Interfaces;
using GalliumPlusApi.Dto;
using GalliumPlusApi.ModelDecorators;
using Modeles;
using System.Diagnostics;

namespace GalliumPlusApi.Dao
{
    public class ProductDao : IProductDAO
    {
        private ProductSummary.Mapper mapper = new();

        public void CreateProduct(Product product)
        {
            using var client = new GalliumPlusHttpClient();
            client.UseSessionToken(SessionStorage.Current.Get<string>("token"));
            
            client.Post("v1/products", mapper.FromModel(product));
        }

        public List<Product> GetProducts()
        {
            using var client = new GalliumPlusHttpClient();
            client.UseSessionToken(SessionStorage.Current.Get<string>("token"));

            try
            {
                var products = client.Get<List<ProductSummary>>("v1/products");
                
                return mapper.ToModel(products).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception non gérée lors de la récupération des produits : {ex}");
                return new List<Product>();
            }
        }

        public void RemoveProduct(Product product)
        {
            using var client = new GalliumPlusHttpClient();
            client.UseSessionToken(SessionStorage.Current.Get<string>("token"));

            client.Delete($"v1/products/{product.ID}");
        }

        public void UpdateProduct(Product product)
        {
            using var client = new GalliumPlusHttpClient();
            client.UseSessionToken(SessionStorage.Current.Get<string>("token"));

            if (product is DecoratedProduct deco)
            {
                client.Put($"v1/products/{product.ID}", mapper.FromModel(deco));
            }
            else
            {
                var original = client.Get<ProductDetails>($"v1/products/{product.ID}");
                client.Put($"v1/products/{product.ID}", mapper.PatchWithModel(original, product));
            }
        }
    }
}
