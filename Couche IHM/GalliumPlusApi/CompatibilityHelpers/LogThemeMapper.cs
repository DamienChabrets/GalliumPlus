using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalliumPlusApi.CompatibilityHelpers
{
    public static class LogThemeMapper
    {
        public static int? ActionKindToLogTheme(string actionKind)
        {
            return actionKind switch
            {
                "LogIn" => 1,
                "EditProductsOrCategories" => 3,
                "EditUsersOrRoles" => 6,
                "Purchase" => 5,
                "Deposit" => 2,
                _ => null
            };
        }
    }
}
