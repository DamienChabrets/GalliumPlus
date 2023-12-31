using Couche_IHM.VueModeles;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Couche_IHM.BindingRules
{
    public class FrameEnumConverterName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum enumValue)
            {
                string enumName = enumValue.ToString();
                string retour;
                switch (enumValue)
                {
                    case Frame.FRAMECAISSE:
                        retour = "Caisse";
                        break;
                    case Frame.FRAMEACCOUNT:
                        retour = "Acomptes";
                        break;
                    case Frame.FRAMESTOCK:
                        retour = "Produits";
                        break;
                    case Frame.FRAMECOMPTES:
                        retour = "Comptes";
                        break;
                    case Frame.FRAMEACCUEIL:
                        retour = "Accueil";
                        break;
                    case Frame.FRAMELOG:
                        retour = "Logs";
                        break;
                    case Frame.FRAMESTATISTIQUE:
                        retour = "Statistiques";
                        break;
                    default:
                        retour = enumName;
                        break;
                }
                return retour;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
