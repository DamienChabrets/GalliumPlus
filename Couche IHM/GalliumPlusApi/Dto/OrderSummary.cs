using Modeles;

namespace GalliumPlusApi.Dto
{
    public class OrderSummary
    {
        public string PaymentMethod { get; set; } = String.Empty;
        public string? Customer { get; set; }
        public List<OrderItemSummary> Items { get; set; } = new();

        public class Mapper : Mapper<Order, OrderSummary>
        {
            private OrderItemSummary.Mapper orderItemMapper = new();

            public override OrderSummary FromModel(Order model)
            {
                return new OrderSummary
                {
                    Customer = model.Customer,
                    PaymentMethod = model.PaymentMethod,
                    Items = orderItemMapper.FromModel(model.OrderedProducts).ToList(),
                };
            }
        }
    }
}