using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace teethLab.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /user/
        //Home Page
        public ActionResult Index()
        {
            string day = getDay();
            ViewData["day"] = day;
            return View();
        }
        public ActionResult storenewdate(){
            string date = Request.Form["date"];
            storeDate(date);
            return Redirect(Url.Action("Index", "Home"));
        }


        public void dateChanged(string change)
        {
            FileStream fs = new FileStream(Server.MapPath("~") + "/Content/files/newmonth.txt", FileMode.Open);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(change);
            sw.Close();
            fs.Close();
        }
        // TODO check month changed and write to file done
        public void storeDate(string dayinput)
        {
            string _date = this.getDay();
            DateTime lastDate = this.dateFromString(_date);

            DateTime currentDate = this.dateFromString(dayinput);

            if (currentDate.Month != lastDate.Month)
            {
                this.dateChanged("true");
            }

            FileStream fs = new FileStream(Server.MapPath("~") + "/Content/files/startdate.txt", FileMode.Open);
            StreamWriter sw = new StreamWriter(fs);
            //sw.Write(day.ToString("yyyy-MM-dd"));
            sw.Write(dayinput);

            sw.Close();
            fs.Close();
        }

        public string getDay()
        {
            FileStream fs = new FileStream(Server.MapPath("~") + "/Content/files/startdate.txt", FileMode.Open);

            StreamReader sr = new StreamReader(fs);

            string date = sr.ReadToEnd();
            sr.Close();
            fs.Close();
            return date;
        }

        public DateTime dateFromString(string date)
        {
            DateTime day;
            System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
            DateTime.TryParseExact(date, "yyyy-MM-dd", enUS,
                                      System.Globalization.DateTimeStyles.None, out day);
            return day;
        }



    }
}
