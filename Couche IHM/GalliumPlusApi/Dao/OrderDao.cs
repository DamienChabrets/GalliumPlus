using Couche_Data.Interfaces;
using GalliumPlusApi.Dto;
using Modeles;

namespace GalliumPlusApi.Dao
{
    public class OrderDao : IOrderDao
    {
        private OrderSummary.Mapper mapper = new();

        public void ProcessOrder(Order order)
        {
            using var client = new GalliumPlusHttpClient();
            client.UseSessionToken(SessionStorage.Current.Get<string>("token"));

            client.Post($"v1/orders", mapper.FromModel(order));
        }
    }
}
