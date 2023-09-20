using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BD_EF_Core
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Account> Accounts { get; set; }

        public bool IsStrongPassword(string password)
        {
            // Define regular expressions for each criterion
            Regex minLengthRegex = new Regex(@".{8,}");
            Regex uppercaseRegex = new Regex(@"[A-Z]");
            Regex lowercaseRegex = new Regex(@"[a-z]");
            Regex digitRegex = new Regex(@"\d");
            Regex symbolRegex = new Regex(@"[!@#$%^&*()_+{}\[\]:;<>,.?~\\-]");

            // Check each criterion
            bool hasMinLength = minLengthRegex.IsMatch(password);
            bool hasUppercase = uppercaseRegex.IsMatch(password);
            bool hasLowercase = lowercaseRegex.IsMatch(password);
            bool hasDigit = digitRegex.IsMatch(password);
            bool hasSymbol = symbolRegex.IsMatch(password);

            // Check if all criteria are met
            return hasMinLength && hasUppercase && hasLowercase && hasDigit && hasSymbol;
        }

        


    }
}
