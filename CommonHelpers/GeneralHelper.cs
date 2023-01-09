using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace pukhoApp.Helpers
{
    internal static class GeneralHelper
    {
        public static bool EmailValidator(string email)
        {
            Regex rEmail = new Regex
            (@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");

            return rEmail.IsMatch(email) && email.Length >= 3;
        }
    }
}
