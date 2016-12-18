
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System;
using System.Globalization;

namespace teethLab
{
    class Validation
    {
        
        public static bool isAlpha(string input)
        {
            //(\w)\1\1
            Regex regex = new Regex(@"^[a-zA-Z]+$");
            Match match = regex.Match(input);
            if (match.Success)
            {
                return true;
            }
            return false;
        }
        public static bool isAlphaNumeric(string input)
        {
            Regex regex = new Regex(@"^[a-zA-Z0-9]+$");
            Match match = regex.Match(input);
            if (match.Success)
            {
                return true;
            }
            return false;
        }
        public static bool isAlphaNumericSomeSpecial(string input)
        {
            Regex regex = new Regex(@"^[a-zA-Z0-9_]+$");
            Match match = regex.Match(input);
            if (match.Success)
            {
                return true;
            }
            return false;
        }
        public static bool isHtmlDate(string input)
        {
            DateTime res;
            CultureInfo enUS = new CultureInfo("en-US");
            if (DateTime.TryParseExact(input, "yyyy-MM-dd", enUS,
                        DateTimeStyles.None, out res))
            {
                return true;
            }
            return false;
        }
        public static bool isNumeric(string input)
        {

            Regex regex = new Regex(@"^[0-9]+$");
            Match match = regex.Match(input);
            if (match.Success)
            {
                return true;
            }
            return false;
        }
    }
}