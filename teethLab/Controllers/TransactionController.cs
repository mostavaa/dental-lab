using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace teethLab.Controllers
{
    public class TransactionController : Controller
    {
        #region views
        /// <summary>
        /// active , view all cases in the day in home page
        /// </summary>
        /// <returns></returns>
        public ActionResult followUpCases()
        {
            string day = Request.QueryString["day"];
            if (day != null)
            {
                DateTime date;
                System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                DateTime.TryParseExact(day, "yyyy-MM-dd", enUS,
                                          System.Globalization.DateTimeStyles.None, out date);
                ViewData["day"] = date;
            }
            teethLabEntities db = new teethLabEntities();
            List<doctor> doctors = db.doctors.ToList();
            List<teethLab.CurrentCas> cases = db.CurrentCases.ToList();
            ViewData["doctors"] = doctors;
            ViewData["cases"] = cases;
            return View();
        }
        /// <summary>
        /// active , view all transactions at specific day
        /// </summary>
        /// <returns></returns>
        public ActionResult recieveTransaction()
        {
            if (Request.Form["day"] != "" && Request.Form["day"] != null)
            {
                DateTime day;
                CultureInfo enUS = new CultureInfo("en-US");
                DateTime.TryParseExact(Request.Form["day"], "yyyy-MM-dd", enUS,
                            DateTimeStyles.None, out day);
                ViewData["day"] = day;
            }
            else
            {
                ViewData["day"] = DateTime.Now;
            }
            return View();
        }
        /// <summary>
        /// active , add , edit , delete current cases
        /// </summary>
        /// <returns></returns>
        public ActionResult viewCurrentCases()
        {
            teethLabEntities db = new teethLabEntities();
            List<CurrentCas> cases = db.CurrentCases.ToList();
            ViewData["cases"] = cases;
            return View();
        }
        public ActionResult Print()
        {
            ViewData["transId"] = int.Parse(Request.Form["transId"]);
            return View();
        }
        public ActionResult Index()
        {
            return null;

        }
        #endregion views
        public void getCaseByNumber()
        {
            try
            {

                int caseNumber = int.Parse(Request.Form["number"]);
                int caseDay = int.Parse(Request.Form["day"]);
                int caseMonth = int.Parse(Request.Form["month"]);
                teethLabEntities db = new teethLabEntities();
                db.Configuration.ProxyCreationEnabled = false;
                CaseTransaction trans = db.CaseTransactions.Where(c => c.caseNumber == caseNumber && c.recieveDate.Day == caseDay
                    && c.recieveDate.Month == caseMonth && c.recieveDate.Year == DateTime.Now.Year).First();
                Response.Write("success^" + JsonConvert.SerializeObject(trans));

                Json(trans, JsonRequestBehavior.AllowGet);
                //Json.Write(trans, Response.Output);
            }
            catch (Exception e)
            {

            }
        }
        public void getCaseById()
        {
            try
            {
                int caseId = int.Parse(Request.Form["caseId"]);
                teethLabEntities db = new teethLabEntities();
                if (db.CaseTransactions.Where(c => c.caseNumber == caseId).Count() > 0)
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    CaseTransaction trans = db.CaseTransactions.Where(c => c.caseNumber == caseId).First();
                    Response.Write("success^" + JsonConvert.SerializeObject(trans));
                }
            }
            catch (Exception e)
            {

            }
        }


        //reference from #inputBtn , #outPrint in follow up cases, 
        public void addCase()
        {
            try
            {


                int doctorId = int.Parse(Request.Form["doctorId"]);
                int caseId = int.Parse(Request.Form["caseId"]);
                string caseColor = Request.Form["caseColor"];
                string patientName = Request.Form["patientName"];
                string notes = Request.Form["notes"];
                DateTime recieveDate;
                CultureInfo enUS = new CultureInfo("en-US");
                DateTime.TryParseExact(Request.Form["recieveDate"], "yyyy-MM-dd", enUS,
                            DateTimeStyles.None, out recieveDate);
                string caseTeeth = Request.Form["caseTeeth"];
                DateTime enterdate;
                enUS = new CultureInfo("en-US");
                DateTime.TryParseExact(Request.Form["day"], "yyyy-MM-dd", enUS,
                            DateTimeStyles.None, out enterdate);


                teethLabEntities db = new teethLabEntities();

                //get price 
                int price = db.caseDoctorPrices.Where(o => o.doctorId == doctorId && o.caseId == caseId).First().price;

                //get last case number

                string newMonth = Request.Form["newMonth"];


                int caseNumber = this.NewCaseNumber();


                int numCases = int.Parse(Request.Form["numCases"]);



                string year = DateTime.Now.Year.ToString();
                string month = DateTime.Now.Month.ToString();
                if (int.Parse(month) < 10)
                {
                    month = "0" + month;
                }
                string day = DateTime.Now.Day.ToString();
                if (int.Parse(day) < 10)
                {
                    day = "0" + day;
                }
                price *= numCases;
                string action = Request.Form["action"];
                CaseTransaction tran = new CaseTransaction();
                if (action == "inputProva")
                {
                    int id = int.Parse(Request.Form["transId"]);
                    tran = db.CaseTransactions.Find(id);
                    tran.notes = notes + "*Case Input Prova at " + DateTime.Now.ToString("yyyy-MM-dd") + "*";
                    tran.isOut = false;
                    tran.prova = true;


                }
                else if (action == "inputCase")
                {
                    tran = new CaseTransaction();
                    tran.caseNumber = caseNumber;
                    DateTime currentDate = myDate.dateFromString(this.getDay());
                    tran.caseNumber = int.Parse(currentDate.Month + "" + currentDate.Day + "" + caseNumber);
                    tran.notes = notes;
                    tran.isOut = false;
                    tran.prova = false;
                    tran.recieveDate = enterdate;
                }
                else if (action == "outCase")
                {
                    int id = int.Parse(Request.Form["transId"]);
                    tran = db.CaseTransactions.Find(id);
                    tran.notes = notes + "*Case Out at " + DateTime.Now.ToString("yyyy-MM-dd") + "*";
                    tran.isOut = true;
                    tran.prova = false;

                    Session["transId"] = id;
                }
                else if (action == "outProva")
                {
                    int id = int.Parse(Request.Form["transId"]);
                    tran = db.CaseTransactions.Find(id);
                    tran.prova = true;
                    tran.notes = notes + "*Case Out Prova at " + DateTime.Now.ToString("yyyy-MM-dd") + "*";
                    tran.isOut = true;

                }
                else
                {
                    return;
                }
                //tran.caseTeeth = caseTeeth;
                tran.caseColor = caseColor;
                tran.price = price;
                tran.patientName = patientName;
                tran.deliverDate = recieveDate;

                tran.caseId = caseId;
                tran.doctorId = doctorId;
                db.CaseTransactions.Add(tran);
                if (action == "inputProva" || action == "outCase" || action == "outProva")
                {
                    db.Entry(tran).State = System.Data.EntityState.Modified;
                }
                else if (action == "inputCase")
                {
                    db.Entry(tran).State = System.Data.EntityState.Added;
                }
                db.SaveChanges();
                Response.Write("success," + price + "," + recieveDate.Month + "/" + recieveDate.Day + "/" + caseNumber);
            }
            catch (Exception e)
            {
                Response.Write("error");
            }
        }



        private bool monthChanged()
        {
            FileStream fs = new FileStream(Server.MapPath("~") + "/Content/files/newmonth.txt", FileMode.Open);

            StreamReader sr = new StreamReader(fs);

            string __monthChanged = sr.ReadToEnd();

            sr.Close();
            fs.Close();

            bool _monthChanged = bool.Parse(__monthChanged);
            if (_monthChanged)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void dateChanged(string change)
        {
            FileStream fs = new FileStream(Server.MapPath("~") + "/Content/files/newmonth.txt", FileMode.Open);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(change);
            sw.Close();
            fs.Close();
        }

        public int NewCaseNumber()
        {
            teethLabEntities db = new teethLabEntities();
            int lastCaseNumber;

            if (this.monthChanged())
            {
                lastCaseNumber = 0;
            }
            else
            {
                int maxId = db.CaseTransactions.Max(o => o.id);

                lastCaseNumber = db.CaseTransactions.Find(maxId).caseNumber;
            }
            lastCaseNumber++;
            return lastCaseNumber;
        }
        public void getNewCaseNumber()
        {
            int lastCaseNumber = this.NewCaseNumber();
            DateTime currentDate = myDate.dateFromString(this.getDay());
            Response.Write(currentDate.Month + "" + currentDate.Day + "" + lastCaseNumber);
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

        public void DeleteCaseById()
        {
            int id = int.Parse(Request.Form["id"]);
            transactionModel tm = new transactionModel();
            tm.DeleteCaseById(id);
            Response.Write("success");
        }

        public void EditCase()
        {
            int id = int.Parse(Request.Form["caseid"]);
            string casename = Request.Form["casename"];
            int defaultPrice = int.Parse(Request.Form["casedefaultprice"]);

            if (casename != "" && casename != null)
            {
                transactionModel tm = new transactionModel();
                tm.editCase(id, casename, defaultPrice);
            }

            Response.Write("success");
        }
        public void AddNewCase()
        {
            string casename = Request.Form["casename"];
            int defaultPrice = int.Parse(Request.Form["casedefaultprice"]);
            if (casename != "" && casename != null)
            {
                transactionModel tm = new transactionModel();
                tm.addNewCase(casename, defaultPrice);
            }
            Response.Write("success");
        }



    }

    public class transactionModel
    {

        public void DeleteCaseById(int caseid)
        {
            teethLabEntities db = new teethLabEntities();
            CurrentCas cas = db.CurrentCases.Find(caseid);
            if (cas != null)
            {
                db.Entry(cas).State = System.Data.EntityState.Deleted;
                db.SaveChanges();
            }
            this.deleteCasePrices(caseid);
        }
        public void deleteCasePrices(int caseId)
        {
            teethLabEntities db = new teethLabEntities();
            foreach (caseDoctorPrice cp in db.caseDoctorPrices.Where(c => c.caseId == caseId))
            {
                db.Entry(cp).State = System.Data.EntityState.Deleted;
            }
            db.SaveChanges();
        }
        public void editCase(int id, string casename, int defaultPrice)
        {
            teethLabEntities db = new teethLabEntities();
            CurrentCas cas = db.CurrentCases.Find(id);
            cas.name = casename;
            cas.defaultPrice = defaultPrice;
            db.Entry(cas).State = System.Data.EntityState.Modified;
            db.SaveChanges();
        }
        public void addNewCase(string casename, int caseDefaultPrice)
        {
            teethLabEntities db = new teethLabEntities();
            CurrentCas cas = new CurrentCas();
            cas.name = casename;
            cas.defaultPrice = caseDefaultPrice;
            db.Entry(cas).State = System.Data.EntityState.Added;
            db.CurrentCases.Add(cas);
            db.SaveChanges();

            this.addDefaultCasePrices(cas);
        }
        public void addDefaultCasePrices(CurrentCas cas)
        {
            teethLabEntities db = new teethLabEntities();
            List<doctor> docs = db.doctors.ToList();
            foreach (doctor doc in docs)
            {
                this.addNewCasePrice(doc.id, cas.id, cas.defaultPrice);
            }

        }

        public void addNewCasePrice(int docId, int caseId, int price)
        {
            caseDoctorPrice cp = new caseDoctorPrice();
            cp.caseId = caseId;
            cp.doctorId = docId;
            cp.price = price;

            teethLabEntities db = new teethLabEntities();
            db.caseDoctorPrices.Add(cp);
            db.SaveChanges();
        }
    }
}
