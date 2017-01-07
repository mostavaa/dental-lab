
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Web;
using System;
using System.IO;

namespace teethLab
{
    public class tools
    {
        /*
        private HttpServerUtilityBase Server;

        public tools(HttpServerUtilityBase Server)
        {
            // TODO: Complete member initialization
            this.Server = Server;
        }
*/

        public static bool isUserLogged()
        {
            if (HttpContext.Current.Session["userId"] == null)
            {
                return false;
            }
            int userId = (int)HttpContext.Current.Session["userId"];
            if (userId != 0) {
                return true;    
            }
            return false;
        }

        public static bool isAdminLogged()
        {
            if (tools.isUserLogged())
            {
                try
                {
                    int adminId = (int)HttpContext.Current.Session["userId"];
                    if (adminId == 1)
                        return true;
                
                }catch(Exception e){

                }
                
            }
            return false;
        }
        public static string removeSpaces(string input)
        {
            input = trimMoreThanOneSpace(input);
            return input.Replace(" ", "");

        }
        public static string trimMoreThanOneSpace(string input)
        {
            input = input.Trim();
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(@"[ ]{2,}", options);
            return regex.Replace(input, @" ");
        }


    }
}