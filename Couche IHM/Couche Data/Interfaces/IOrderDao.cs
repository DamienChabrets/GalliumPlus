using Modeles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Couche_Data.Interfaces
{
    public interface IOrderDao
    {
        void ProcessOrder(Order order);
    }
}
