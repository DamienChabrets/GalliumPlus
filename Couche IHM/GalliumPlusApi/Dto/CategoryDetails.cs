using Modeles;

namespace GalliumPlusApi.Dto
{
    public class CategoryDetails
    {
        public int Id { get; set; } = -1;
        public string Name { get; set; } = String.Empty;

        public class Mapper : Mapper<Category, CategoryDetails>
        {
            public override CategoryDetails FromModel(Category model)
            {
                return new CategoryDetails { Id = model.IdCat, Name = model.NomCategory };
            }

            public override Category ToModel(CategoryDetails dto)
            {
                return new Category(dto.Id, dto.Name, dto.Name != "Hidden");
            }
        }
    }
}