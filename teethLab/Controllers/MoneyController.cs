using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace teethLab.Controllers
{
    public class MoneyController : Controller
    {
        //
        // GET: /Money/
        // sadrat w wardat 
        public ActionResult Index()
        {
            teethLabEntities db = new teethLabEntities();
            List<doctor> doctors = db.doctors.Where(d => d.isActive == true).ToList();
            ViewData["doctors"] = doctors;
            ViewData["emps"] = db.employees.ToList();
            ViewData["companies"] = db.companies.ToList();
            DateTime now = DateTime.Now;
            int credit = 0;

            if (db.moneyDays.Where(o => o.day.Year == now.Year
                && o.day.Month == now.Month
                && o.day.Day == now.Day
                ).Count() <= 0)
            {
                //get last day credit
                int lastId = db.moneyDays.Max(o => o.id);
                credit = 0;
                if (lastId != 0)
                {
                    credit = db.moneyDays.Find(lastId).credit;
                }
                moneyDay md = new moneyDay();
                md.credit = credit;
                md.day = now;
                db.moneyDays.Add(md);
                db.SaveChanges();
            }
            else
            {

                credit = db.moneyDays.Where(o => o.day.Year == now.Year
                && o.day.Month == now.Month
                && o.day.Day == now.Day
                ).First().credit;
            }
            ViewData["credit"] = credit;
            return View();
        }
        public void export()
        {
            string notes = Request.Form["notes"];
            int cost = 0;
            try
            {
                cost = int.Parse(Request.Form["cost"]);
            }
            catch (Exception e)
            {
                return;
            }
            int id = 0;
            try
            {
                id = int.Parse(Request.Form["id"]);
            }
            catch (Exception e)
            {

            }

            string name = Request.Form["name"];
            if (name == "" || name == null)
            {
                return;
            }
            string type = Request.Form["type"];


            teethLabEntities db = new teethLabEntities();



            int dayId = 0;
            DateTime day;
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime.TryParseExact(Request.Form["day"], "yyyy-MM-dd", enUS,
                        DateTimeStyles.None, out day);

            moneyDay md = new moneyDay();

            if (db.moneyDays.Where(o => o.day.Year == day.Year
            && o.day.Month == day.Month
                && o.day.Day == day.Day).Count() > 0)
            {
                md = db.moneyDays.Where(o => o.day.Year == day.Year
                && o.day.Month == day.Month
                && o.day.Day == day.Day
                ).First();
                md.credit -= cost;
                db.Entry(md).State = System.Data.EntityState.Modified;

            }
            else
            {
                return;
            }


            db.SaveChanges();
            dayId = md.id;


            db = new teethLabEntities();
            money m = new money();
            m.dayId = dayId;
            
            m.fromTo = name;

            m.type = "export";
            m.value = cost;
            m.recieveDate = DateTime.Now;


            m.notes = notes;

            if (type == "new")
            {

            }
            else if (type == "Doctor")
            {
                m.doctorId = id;
            }
            else if (type == "Company")
            {
                m.companyId = id;
            }
            else if (type == "Employee")
            {
                m.employeeId = id;
            }




            db.Entry(m).State = System.Data.EntityState.Added;
            db.moneys.Add(m);
            db.SaveChanges();

            db = new teethLabEntities();
            if (type == "Company")
            {

                product p = new product();
                p.companyId = id;
                p.enterDate = day;
                p.name = notes;
                p.isFinished = false;
                p.price = cost;
                db.Entry(p).State = System.Data.EntityState.Added;


            }
            else if (type == "Doctor")
            {
                doctor doc = db.doctors.Find(id);
                db.Entry(doc).State = System.Data.EntityState.Modified;
                doc.depit += cost;

            }
            db.SaveChanges();
            Response.Write("success");

        }
        public void import()
        {
            string notes = Request.Form["notes"];
            //test changes in source control
            int cost, id = 0, receiptno = 0;
            try
            {
                receiptno = int.Parse(Request.Form["receiptno"]);
                cost = int.Parse(Request.Form["cost"]);
            }
            catch (Exception e)
            {
                Response.Write("Please enter cost and reciept number");
                return;
            }
            if (Request.Form["id"] != "" && Request.Form["id"] != null)
            {
                id = int.Parse(Request.Form["id"]);
            }
            string name = Request.Form["name"];

            if (Request.Form["day"] == "" || Request.Form["day"] == null)
            {
                Response.Write("you must select day first");

                return;
            }
            DateTime day;
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime.TryParseExact(Request.Form["day"], "yyyy-MM-dd", enUS,
                        DateTimeStyles.None, out day);

            teethLabEntities db = new teethLabEntities();
            int dayId = 0;
            moneyDay md = new moneyDay();
            if (db.moneyDays.Where(o => o.day.Year == day.Year
                && o.day.Month == day.Month
                && o.day.Day == day.Day).Count() > 0)
            {
                md = db.moneyDays.Where(o => o.day.Year == day.Year
                && o.day.Month == day.Month
                && o.day.Day == day.Day
                ).First();
                md.credit += cost;
                db.Entry(md).State = System.Data.EntityState.Modified;

            }
            else
            {
                Response.Write("you must select day first");
                return;
            }

            db.SaveChanges();
            dayId = md.id;
            db = new teethLabEntities();

            money m = new money();
            m.dayId = dayId;
            if (id != 0)
            {
                m.doctorId = id;
                try
                {
                    m.fromTo = name;
                }
                catch (Exception e)
                {
                    m.fromTo = "طبيب";
                }
            }
            else if (name != "" || name != null)
            {
                m.fromTo = name;
            }
            if (db.moneys.Where(o => o.receiptno == receiptno).Count() > 0)
            {
                Response.Write("receipt number cannot duplicated");
                return;
            }
            m.receiptno = receiptno;
            m.type = "import";
            m.recieveDate = DateTime.Now;
            m.value = cost;
            m.notes = notes;
            db.Entry(m).State = System.Data.EntityState.Added;
            db.moneys.Add(m);
            db.SaveChanges();
            if (id != 0)
            {
                db = new teethLabEntities();
                doctor doc = db.doctors.Find(id);
                doc.depit -= cost;
                db.Entry(doc).State = System.Data.EntityState.Modified;
                db.SaveChanges();
            }
            Response.Write("success");

        }
        private moneyDay currentMoneyDay;
        private int currentCredit;

        private void newDay()
        {

            moneyDay md = null;


            DateTime day;
            FileStream fs = new FileStream(Server.MapPath("~") + "/Content/files/startdate.txt", FileMode.Open);

            StreamReader sr = new StreamReader(fs);

            string date = sr.ReadToEnd();
            sr.Close();
            fs.Close();
            System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
            DateTime.TryParseExact(date, "yyyy-MM-dd", enUS,
                                      System.Globalization.DateTimeStyles.None, out day);



            teethLabEntities db = new teethLabEntities();


            int credit = 0;

            if (db.moneyDays.Where(o => o.day.Year == day.Year &&
          o.day.Month == day.Month &&
           o.day.Day == day.Day
          ).Count() > 0)
            {

                md = db.moneyDays.Where(o => o.day.Year == day.Year &&
          o.day.Month == day.Month &&
           o.day.Day == day.Day).First();

                credit = md.credit;
            }
            else
            {

                //get last day credit
                int lastId = db.moneyDays.Max(o => o.id);
                credit = 0;
                if (lastId != 0)
                {
                    credit = db.moneyDays.Find(lastId).credit;
                }
                md = new moneyDay();
                md.credit = credit;
                md.day = day;
                db.moneyDays.Add(md);
                db.SaveChanges();

            }
            this.currentMoneyDay = md;
            this.currentCredit = credit;
        }
        //view sadrat and wardat in specific day
        public ActionResult viewMoneyDays()
        {
            teethLabEntities db = new teethLabEntities();
            List<doctor> doctors = db.doctors.Where(d => d.isActive == true).ToList();
            ViewData["doctors"] = doctors;
            ViewData["emps"] = db.employees.ToList();
            ViewData["companies"] = db.companies.ToList();

            this.newDay();

            ViewData["credit"] = this.currentCredit;


            ViewData["moneyDay"] = this.currentMoneyDay;
            return View();
        }



        public void getFrom()
        {
            string q = Request.Form["q"];
            List<doctor> docs = this.getDocsByName(q);
            if (docs != null)
            {
                foreach (doctor doc in docs)
                {
                    Response.Write(doc.name + "," + doc.id + ",Doctor&");
                }
            }
        }
        public void getTo()
        {
            string q = Request.Form["q"];
            List<employee> emps = this.getEmpsByName(q);
            if (emps != null)
            {
                foreach (employee emp in emps)
                {
                    Response.Write(emp.name + "," + emp.id + ",Employee&");
                }
            }

            List<doctor> docs = this.getDocsByName(q);
            if (docs != null)
            {
                foreach (doctor doc in docs)
                {
                    Response.Write(doc.name + "," + doc.id + ",Doctor&");
                }
            }

            List<company> comps = this.getCompaniesByName(q);
            if (comps != null)
            {
                foreach (company comp in comps)
                {
                    Response.Write(comp.name + "," + comp.id + ",Company&");
                }
            }

        }

        private List<employee> getEmpsByName(string name)
        {
            teethLabEntities db = new teethLabEntities();

            if (db.employees.Where(e => e.name.Contains(name)).Count() > 0)
            {
                return db.employees.Where(e => e.name.Contains(name)).ToList();
            }
            return null;
        }

        private List<company> getCompaniesByName(string name)
        {
            teethLabEntities db = new teethLabEntities();

            if (db.companies.Where(c => c.name.Contains(name)).Count() > 0)
            {
                return db.companies.Where(c => c.name.Contains(name)).ToList();
            }
            return null;
        }

        private List<doctor> getDocsByName(string name)
        {
            teethLabEntities db = new teethLabEntities();

            if (db.doctors.Where(d => d.name.Contains(name)).Count() > 0)
            {
                return db.doctors.Where(d => d.name.Contains(name)).ToList();
            }
            return null;
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
        public void startNextDay()
        {

            
            DateTime day;
            FileStream fs = new FileStream(Server.MapPath("~") + "/Content/files/startdate.txt", FileMode.Open);

            StreamReader sr = new StreamReader(fs);

            string date = sr.ReadToEnd();
            sr.Close();
            fs.Close();
            System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
            DateTime.TryParseExact(date, "yyyy-MM-dd", enUS,
                                      System.Globalization.DateTimeStyles.None, out day);

            int lastMonth = day.Month;

            
            day = day.AddDays(1);
            
            int currentMonth = day.Month;
            if (currentMonth != lastMonth)
            {
                this.dateChanged("true");
            }


            fs = new FileStream(Server.MapPath("~") + "/Content/files/startdate.txt", FileMode.Open);
            StreamWriter sw = new StreamWriter(fs);

            sw.Write(day.ToString("yyyy-MM-dd"));
            sw.Close();
            fs.Close();
            Response.Write("success");
        }












        /// <summary>
        /// 
        /// </summary>





        public void getDateCredit()
        {

            if (Request.Form["day"] == "" || Request.Form["day"] == null)
            {
                Response.Write("");
                return;
            }

            DateTime day;
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime.TryParseExact(Request.Form["day"], "yyyy-MM-dd", enUS,
                        DateTimeStyles.None, out day);

            teethLabEntities db = new teethLabEntities();
            if (db.moneyDays.Where(o => o.day.Year == day.Year &&
                o.day.Month == day.Month &&
                 o.day.Day == day.Day
                ).Count() > 0)
            {
                Response.Write(db.moneyDays.Where(o => o.day.Year == day.Year &&
                 o.day.Month == day.Month &&
                  o.day.Day == day.Day
                 ).First().credit);
            }
        }

        public ActionResult MoneyDays()
        {
            DateTime day;
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime.TryParseExact(Request.Form["day"], "yyyy-MM-dd", enUS,
                        DateTimeStyles.None, out day);
            teethLabEntities db = new teethLabEntities();
            moneyDay md = null;
            if (db.moneyDays.Where(o => o.day.Year == day.Year &&
          o.day.Month == day.Month &&
           o.day.Day == day.Day
          ).Count() > 0)
            {

                md = db.moneyDays.Where(o => o.day.Year == day.Year &&
          o.day.Month == day.Month &&
           o.day.Day == day.Day).First();
            }

            TempData["moneyDay"] = md;
            return Redirect(Url.Action("viewMoneyDays", "Money"));
        }


    }
}
