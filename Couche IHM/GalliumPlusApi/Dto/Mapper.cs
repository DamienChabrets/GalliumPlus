namespace GalliumPlusApi.Dto
{
    public abstract class Mapper<TModel, TDto>
    {
        public virtual TModel ToModel(TDto dto)
        {
            throw new InvalidOperationException("Ce DTO ne doit pas entrer !! >:(");
        }

        public IEnumerable<TModel> ToModel(IEnumerable<TDto> dtos)
        {
            foreach (TDto dto in dtos) yield return ToModel(dto);
        }

        public virtual TDto FromModel(TModel model)
        {
            throw new InvalidOperationException("Ce DTO ne doit pas sortir !! >:(");
        }

        public IEnumerable<TDto> FromModel(IEnumerable<TModel> models)
        {
            foreach (TModel model in models) yield return FromModel(model);
        }
    }
}
