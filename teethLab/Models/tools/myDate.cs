
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Web;
using System;
using System.IO;

namespace teethLab
{
    public class myDate
    {

        public static DateTime dateFromString(string date)
        {
            DateTime day;
            System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
            DateTime.TryParseExact(date, "yyyy-MM-dd", enUS,
                                      System.Globalization.DateTimeStyles.None, out day);
            return day;
        }
    }
}