using Couche_Data.Dao;
using Couche_Data.Interfaces;
using DocumentFormat.OpenXml.Presentation;
using GalliumPlusApi.Dao;
using Modeles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Couche_Métier.Manager
{
    public class OrderManager
    {
        private IOrderDao dao;
        private Order? currentOrder;

        public Order CurrentOrder => currentOrder!;

        public OrderManager()
        {
            if (DevelopmentInfo.isDevelopment)
            {
                dao = new OrderDAO();
            }
            else
            {
                dao = new OrderDao();
            }

        }

        public void NewOrder(string paymentMethod, string customer = null)
        {
            currentOrder = new Order(paymentMethod, customer);
        }

        public void ClearOrder()
        {
            currentOrder = null;
        }

        public void ProcessOrder()
        {
            dao.ProcessOrder(currentOrder!);
            ClearOrder();
        }
    }
}
