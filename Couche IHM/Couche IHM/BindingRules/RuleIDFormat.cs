using Couche_IHM.VueModeles;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Couche_IHM.BindingRules
{

    public class RuleIDFormat : ValidationRule
    {

        /// <summary>
        /// Permet de tester la validité du binding
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = ValidationResult.ValidResult;

            AccountViewModel adh = MainWindowViewModel.Instance.AccountsViewModel.CurrentAccount;
            string identifiant = (string)value;
            if (identifiant.Length > 2)
            {
                string nom = adh.NomIHM;
                string prenom = adh.PrenomIHM;
                if (identifiant[0] != prenom.ToLower()[0] || identifiant[1] != nom.ToLower()[0])
                {
                    result = new ValidationResult(false, "Format invalide");
                }
            }
            else
            {
                result = new ValidationResult(false, "Format invalide");
            }
           
            

            return result;
        }
    }
}
