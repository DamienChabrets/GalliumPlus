using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modeles
{
    public class Order
    {
        private Dictionary<int, int> orderedProducts;
        private string? customer;
        private string paymentMethod;

        public static readonly string ANONYMOUS_MEMBER = "@anonymousmember";

        public IDictionary<int, int> OrderedProducts => orderedProducts;

        public string? Customer => customer;

        public string PaymentMethod => paymentMethod;

        public Order(string paymentMethod, string? customer = null)
        {
            this.paymentMethod = paymentMethod;
            this.customer = customer;
            orderedProducts = new Dictionary<int, int>();
        }

        public void AddProduct(int productId, int quantity)
        {
            orderedProducts[productId] = quantity;
        }
    }
}
