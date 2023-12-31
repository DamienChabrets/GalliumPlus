using Couche_Data.Interfaces;
using GalliumPlusApi.Dto;
using Modeles;
using System.Diagnostics;

namespace GalliumPlusApi.Dao
{
    public class CategoryDao : ICategoryDao
    {
        private CategoryDetails.Mapper mapper = new();

        public void CreateCategory(Category cat)
        {
            using var client = new GalliumPlusHttpClient();
            client.UseSessionToken(SessionStorage.Current.Get<string>("token"));

            client.Post($"v1/categories", mapper.FromModel(cat));
        }

        public void DeleteCategory(Category cat)
        {
            using var client = new GalliumPlusHttpClient();
            client.UseSessionToken(SessionStorage.Current.Get<string>("token"));

            client.Delete($"v1/categories/{cat.IdCat}");
        }

        public List<Category> ListALlCategory()
        {
            using var client = new GalliumPlusHttpClient();
            client.UseSessionToken(SessionStorage.Current.Get<string>("token"));

            try
            {
                var categories = client.Get<List<CategoryDetails>>("v1/categories");

                return mapper.ToModel(categories).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception non gérée lors de la récupération des catégories : {ex}");
                return new List<Category>();
            }
        }

        public void UpdateCategory(Category category)
        {
            using var client = new GalliumPlusHttpClient();
            client.UseSessionToken(SessionStorage.Current.Get<string>("token"));

            client.Put($"v1/categories/{category.IdCat}", mapper.FromModel(category));
        }
    }
}
