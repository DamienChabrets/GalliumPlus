using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalliumPlusApi.Exceptions
{
    internal class CantSellException : GalliumPlusHttpException
    {
        public CantSellException() : base("Impossible de passer la commande") { }

        public CantSellException(string message) : base(message) { }
    }
}
