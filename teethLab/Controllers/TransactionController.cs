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
                CaseTransaction trans = db.CaseTransactions.Where(c => c.caseNumber == caseNumber && c.recieveDate.Value.Day == caseDay
                   && c.recieveDate != null && c.recieveDate.Value.Month == caseMonth && c.recieveDate.Value.Year == DateTime.Now.Year).First();
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

        public ActionResult inputCase()
        {
            teethLabEntities db = new teethLabEntities();

            int doctorId = int.Parse(Request.Form["doctor"]);
            doctor doc = db.doctors.FirstOrDefault(d => d.id == doctorId);
            if (doc != null)
            {

                int selectedCaseId = int.Parse(Request.Form["selectedCase"]);
                CurrentCas myCase = db.CurrentCases.FirstOrDefault(c => c.id == selectedCaseId);
                if (myCase != null)
                {

                    string caseColor1 = Request.Form["shadeColor1"];
                    string caseColor2 = Request.Form["shadeColor2"];
                    string caseColor3 = Request.Form["shadeColor3"];
                    string patientName = Request.Form["patientName"];
                    string notes = Request.Form["notes"];
                    DateTime recieveDate;
                    CultureInfo enUS = new CultureInfo("en-US");
                    DateTime.TryParseExact(Request.Form["recieveDate"], "yyyy-MM-dd", enUS,
                                DateTimeStyles.None, out recieveDate);

                    DateTime enterdate;
                    enUS = new CultureInfo("en-US");
                    DateTime.TryParseExact(Request.Form["day"], "yyyy-MM-dd", enUS,
                                DateTimeStyles.None, out enterdate);
                    if (enterdate != null)
                    {
                        string _UL8 = Request.Form["UL8"];
                        string _UL7 = Request.Form["UL7"];
                        string _UL6 = Request.Form["UL6"];
                        string _UL5 = Request.Form["UL5"];
                        string _UL4 = Request.Form["UL4"];
                        string _UL3 = Request.Form["UL3"];
                        string _UL2 = Request.Form["UL2"];
                        string _UL1 = Request.Form["UL1"];

                        string _UR1 = Request.Form["UR1"];
                        string _UR2 = Request.Form["UR2"];
                        string _UR3 = Request.Form["UR3"];
                        string _UR4 = Request.Form["UR4"];
                        string _UR5 = Request.Form["UR5"];
                        string _UR6 = Request.Form["UR6"];
                        string _UR7 = Request.Form["UR7"];
                        string _UR8 = Request.Form["UR8"];

                        string _LL8 = Request.Form["LL8"];
                        string _LL7 = Request.Form["LL7"];
                        string _LL6 = Request.Form["LL6"];
                        string _LL5 = Request.Form["LL5"];
                        string _LL4 = Request.Form["LL4"];
                        string _LL3 = Request.Form["LL3"];
                        string _LL2 = Request.Form["LL2"];
                        string _LL1 = Request.Form["LL1"];

                        string _LR1 = Request.Form["LR1"];
                        string _LR2 = Request.Form["LR2"];
                        string _LR3 = Request.Form["LR3"];
                        string _LR4 = Request.Form["LR4"];
                        string _LR5 = Request.Form["LR5"];
                        string _LR6 = Request.Form["LR6"];
                        string _LR7 = Request.Form["LR7"];
                        string _LR8 = Request.Form["LR8"];

                        bool UL8 = _UL8 == "0" ? false : true;
                        bool UL7 = _UL7 == "0" ? false : true;
                        bool UL6 = _UL6 == "0" ? false : true;
                        bool UL5 = _UL5 == "0" ? false : true;
                        bool UL4 = _UL4 == "0" ? false : true;
                        bool UL3 = _UL3 == "0" ? false : true;
                        bool UL2 = _UL2 == "0" ? false : true;
                        bool UL1 = _UL1 == "0" ? false : true;
                        bool UR1 = _UR1 == "0" ? false : true;
                        bool UR2 = _UR2 == "0" ? false : true;
                        bool UR3 = _UR3 == "0" ? false : true;
                        bool UR4 = _UR4 == "0" ? false : true;
                        bool UR5 = _UR5 == "0" ? false : true;
                        bool UR6 = _UR6 == "0" ? false : true;
                        bool UR7 = _UR7 == "0" ? false : true;
                        bool UR8 = _UR8 == "0" ? false : true;
                        bool LL8 = _LL8 == "0" ? false : true;
                        bool LL7 = _LL7 == "0" ? false : true;
                        bool LL6 = _LL6 == "0" ? false : true;
                        bool LL5 = _LL5 == "0" ? false : true;
                        bool LL4 = _LL4 == "0" ? false : true;
                        bool LL3 = _LL3 == "0" ? false : true;
                        bool LL2 = _LL2 == "0" ? false : true;
                        bool LL1 = _LL1 == "0" ? false : true;
                        bool LR1 = _LR1 == "0" ? false : true;
                        bool LR2 = _LR2 == "0" ? false : true;
                        bool LR3 = _LR3 == "0" ? false : true;
                        bool LR4 = _LR4 == "0" ? false : true;
                        bool LR5 = _LR5 == "0" ? false : true;
                        bool LR6 = _LR6 == "0" ? false : true;
                        bool LR7 = _LR7 == "0" ? false : true;
                        bool LR8 = _LR8 == "0" ? false : true;

                        int numCases = 0;
                        if (UL8) { numCases++; }
                        if (UL7) { numCases++; }
                        if (UL6) { numCases++; }
                        if (UL5) { numCases++; }
                        if (UL4) { numCases++; }
                        if (UL3) { numCases++; }
                        if (UL2) { numCases++; }
                        if (UL1) { numCases++; }
                        if (UR1) { numCases++; }
                        if (UR2) { numCases++; }
                        if (UR3) { numCases++; }
                        if (UR4) { numCases++; }
                        if (UR5) { numCases++; }
                        if (UR6) { numCases++; }
                        if (UR7) { numCases++; }
                        if (UR8) { numCases++; }
                        if (LL8) { numCases++; }
                        if (LL7) { numCases++; }
                        if (LL6) { numCases++; }
                        if (LL5) { numCases++; }
                        if (LL4) { numCases++; }
                        if (LL3) { numCases++; }
                        if (LL2) { numCases++; }
                        if (LL1) { numCases++; }
                        if (LR1) { numCases++; }
                        if (LR2) { numCases++; }
                        if (LR3) { numCases++; }
                        if (LR4) { numCases++; }
                        if (LR5) { numCases++; }
                        if (LR6) { numCases++; }
                        if (LR7) { numCases++; }
                        if (LR8) { numCases++; }

                        int caseNumber = int.Parse(Request.Form["caseNumber"]);
                        string action = Request.Form["status"];
                        caseDoctorPrice casePrice = db.caseDoctorPrices.FirstOrDefault(o => o.doctorId == doctorId && o.caseId == selectedCaseId);
                        int price = 0;
                        if (casePrice != null)
                        {
                            price = casePrice.price;
                        }
                        else
                        {
                            price = myCase.defaultPrice;
                        }
                        price *= numCases;
                        if (action == "inputCase")
                        {
                            CaseTransaction tran = new CaseTransaction()
                            {
                                caseColor1 = caseColor1,
                                caseColor2 = caseColor2,
                                caseColor3 = caseColor3,
                                caseId = selectedCaseId
                                ,
                                caseNumber = caseNumber,
                                deliverDate = recieveDate,
                                doctorId = doctorId,
                                isOut = false,
                                notes = notes,
                                patientName = patientName,
                                prova = false,
                                price = price,
                                LL1 = LL1,
                                LL2 = LL2,
                                LL3 = LL3,
                                LL4 = LL4,
                                LL5 = LL5,
                                LL6 = LL6,
                                LL7 = LL7,
                                LL8 = LL8,
                                LR1 = LR1,
                                LR2 = LR2,
                                LR3 = LR3,
                                LR4 = LR4,
                                LR5 = LR5,
                                LR6 = LR6,
                                LR7 = LR7,
                                LR8 = LR8,
                                UR1 = UR1,
                                UR2 = UR2,
                                UR3 = UR3,
                                UR4 = UR4,
                                UR5 = UR5,
                                UR6 = UR6,
                                UR7 = UR7,
                                UR8 = UR8,
                                UL1 = UL1,
                                UL2 = UL2,
                                UL3 = UL3,
                                UL4 = UL4,
                                UL5 = UL5,
                                UL6 = UL6,
                                UL7 = UL7,
                                UL8 = UL8,
                                recieveDate = enterdate
                            };
                            db.CaseTransactions.Add(tran);
                            db.SaveChanges();
                        }
                        else if (action == "inputProva")
                        {
                            int id = int.Parse(Request.Form["caseId"]);
                            CaseTransaction tran = db.CaseTransactions.FirstOrDefault(o => o.id == id);
                            if (tran!=null) {
                                tran.notes = notes + "*Case Input Prova at " + enterdate.ToString("yyyy-MM-dd") + "*";
                                tran.isOut = false;
                                tran.prova = true;

                                tran.caseColor1 = caseColor1;
                                tran.caseColor2 = caseColor2;
                                tran.caseColor3 = caseColor3;
                                tran.caseId = selectedCaseId;
                                tran.caseNumber = caseNumber;
                                tran.deliverDate = recieveDate;
                                tran.doctorId = doctorId;
                                tran.patientName = patientName;
                                tran.price = price;
                                tran.LL1 = LL1;
                                tran.LL2 = LL2;
                                tran.LL3 = LL3;
                                tran.LL4 = LL4;
                                tran.LL5 = LL5;
                                tran.LL6 = LL6;
                                tran.LL7 = LL7;
                                tran.LL8 = LL8;
                                tran.LR1 = LR1;
                                tran.LR2 = LR2;
                                tran.LR3 = LR3;
                                tran.LR4 = LR4;
                                tran.LR5 = LR5;
                                tran.LR6 = LR6;
                                tran.LR7 = LR7;
                                tran.LR8 = LR8;
                                tran.UR1 = UR1;
                                tran.UR2 = UR2;
                                tran.UR3 = UR3;
                                tran.UR4 = UR4;
                                tran.UR5 = UR5;
                                tran.UR6 = UR6;
                                tran.UR7 = UR7;
                                tran.UR8 = UR8;
                                tran.UL1 = UL1;
                                tran.UL2 = UL2;
                                tran.UL3 = UL3;
                                tran.UL4 = UL4;
                                tran.UL5 = UL5;
                                tran.UL6 = UL6;
                                tran.UL7 = UL7;
                                tran.UL8 = UL8;

                                db.Entry(tran).State = System.Data.EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        else if (action == "outCase")
                        {
                            int id = int.Parse(Request.Form["caseId"]);
                            CaseTransaction tran = db.CaseTransactions.FirstOrDefault(o => o.id == id);
                            if (tran != null)
                            {
                                tran.notes = notes + "*Case Out at " + DateTime.Now.ToString("yyyy-MM-dd") + "*";
                                tran.isOut = true;
                                tran.prova = false;

                                tran.caseColor1 = caseColor1;
                                tran.caseColor2 = caseColor2;
                                tran.caseColor3 = caseColor3;
                                tran.caseId = selectedCaseId;
                                tran.caseNumber = caseNumber;
                                tran.deliverDate = recieveDate;
                                tran.doctorId = doctorId;
                                tran.patientName = patientName;
                                tran.price = price;
                                tran.LL1 = LL1;
                                tran.LL2 = LL2;
                                tran.LL3 = LL3;
                                tran.LL4 = LL4;
                                tran.LL5 = LL5;
                                tran.LL6 = LL6;
                                tran.LL7 = LL7;
                                tran.LL8 = LL8;
                                tran.LR1 = LR1;
                                tran.LR2 = LR2;
                                tran.LR3 = LR3;
                                tran.LR4 = LR4;
                                tran.LR5 = LR5;
                                tran.LR6 = LR6;
                                tran.LR7 = LR7;
                                tran.LR8 = LR8;
                                tran.UR1 = UR1;
                                tran.UR2 = UR2;
                                tran.UR3 = UR3;
                                tran.UR4 = UR4;
                                tran.UR5 = UR5;
                                tran.UR6 = UR6;
                                tran.UR7 = UR7;
                                tran.UR8 = UR8;
                                tran.UL1 = UL1;
                                tran.UL2 = UL2;
                                tran.UL3 = UL3;
                                tran.UL4 = UL4;
                                tran.UL5 = UL5;
                                tran.UL6 = UL6;
                                tran.UL7 = UL7;
                                tran.UL8 = UL8;
                                db.Entry(tran).State = System.Data.EntityState.Modified;
                                db.SaveChanges();
                            } 
                        }
                        else if (action == "outProva")
                        {
                            int id = int.Parse(Request.Form["caseId"]);
                            CaseTransaction tran = db.CaseTransactions.FirstOrDefault(o => o.id == id);
                            if (tran != null)
                            {
                                tran.notes = notes + "*Case Out Prova at " + DateTime.Now.ToString("yyyy-MM-dd") + "*";
                                tran.isOut = true;
                                tran.prova = true;

                                tran.caseColor1 = caseColor1;
                                tran.caseColor2 = caseColor2;
                                tran.caseColor3 = caseColor3;
                                tran.caseId = selectedCaseId;
                                tran.caseNumber = caseNumber;
                                tran.deliverDate = recieveDate;
                                tran.doctorId = doctorId;
                                tran.patientName = patientName;
                                tran.price = price;
                                tran.LL1 = LL1;
                                tran.LL2 = LL2;
                                tran.LL3 = LL3;
                                tran.LL4 = LL4;
                                tran.LL5 = LL5;
                                tran.LL6 = LL6;
                                tran.LL7 = LL7;
                                tran.LL8 = LL8;
                                tran.LR1 = LR1;
                                tran.LR2 = LR2;
                                tran.LR3 = LR3;
                                tran.LR4 = LR4;
                                tran.LR5 = LR5;
                                tran.LR6 = LR6;
                                tran.LR7 = LR7;
                                tran.LR8 = LR8;
                                tran.UR1 = UR1;
                                tran.UR2 = UR2;
                                tran.UR3 = UR3;
                                tran.UR4 = UR4;
                                tran.UR5 = UR5;
                                tran.UR6 = UR6;
                                tran.UR7 = UR7;
                                tran.UR8 = UR8;
                                tran.UL1 = UL1;
                                tran.UL2 = UL2;
                                tran.UL3 = UL3;
                                tran.UL4 = UL4;
                                tran.UL5 = UL5;
                                tran.UL6 = UL6;
                                tran.UL7 = UL7;
                                tran.UL8 = UL8;

                                db.Entry(tran).State = System.Data.EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            return RedirectToAction("followUpCases", new { day = enterdate.ToString("yyyy-MM-dd") });
                        }

                    }
                    return RedirectToAction("followUpCases", new { day = enterdate.ToString("yyyy-MM-dd") });
                }

            }

            return RedirectToAction("followUpCases");
        }
        //reference from #inputBtn , #outPrint in follow up cases, 
        public void addCase()
        {
            /*
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
                tran.caseColor1 = caseColor;
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
            */
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
            int maxId = db.CaseTransactions.Max(o => o.id);
            return maxId;
        }
        public void getNewCaseNumber()
        {
            DateTime Date;
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime.TryParseExact(Request.Form["day"], "yyyy-MM-dd", enUS,
                        DateTimeStyles.None, out Date);
            int lastCaseNumber = this.NewCaseNumber();
            DateTime currentDate = DateTime.Now;
            Response.Write(Date.Month + "" + Date.Day + "" + lastCaseNumber);
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
