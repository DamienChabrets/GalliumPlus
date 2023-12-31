namespace GalliumPlusApi.Dto
{
    public class OrderItemSummary
    {
        public int Product { get; set; }
        public int Quantity { get; set; }

        public class Mapper : Mapper<KeyValuePair<int, int>, OrderItemSummary>
        {
            public override OrderItemSummary FromModel(KeyValuePair<int, int> model)
            {
                return new OrderItemSummary { Product = model.Key, Quantity = model.Value };
            }
        }
    }
}