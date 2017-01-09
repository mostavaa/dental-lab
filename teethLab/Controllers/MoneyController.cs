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
        public ActionResult Export()
        {
            string _companyCheckBox = Request.Form["companyCheckBox"];
            string _companySelect = Request.Form["companySelect"];
            string _employeeSelect = Request.Form["employeeSelect"];
            string _employeeCheckBox = Request.Form["employeeCheckBox"];
            string _otherTo = Request.Form["otherTo"];
            string _otherCheckBox = Request.Form["otherCheckBox"];

            string _valueTo = Request.Form["valueTo"];
            string _toNotes = Request.Form["toNotes"];
            string _day = Request.Form["day"];

            teethLabEntities db = new teethLabEntities();
            if (_companyCheckBox == "on")
            {
                if (_companySelect != null && _companySelect != string.Empty)
                {
                    int companyId = int.Parse(_companySelect);
                    company comp = db.companies.FirstOrDefault(o => o.id == companyId);
                    if (comp != null)
                    {
                        int valueTo = int.Parse(_valueTo);
                        string toNotes = _toNotes;
                        if (_day != null)
                        {
                            DateTime date;
                            System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                            DateTime.TryParseExact(_day, "yyyy-MM-dd", enUS,
                                                      System.Globalization.DateTimeStyles.None, out date);
                            if (date != null)
                            {

                                comp.credit--;
                                db.Entry(comp).State = System.Data.EntityState.Modified;
                                money money = new money { company = comp, currentCredit = comp.credit, import = false, notes = toNotes, value = valueTo, recieveDate = date };
                                db.moneys.Add(money);
                                db.SaveChanges();
                            }

                        }

                    }
                }
            }
            else if (_employeeCheckBox == "on")
            {
                int empId = int.Parse(_employeeSelect);
                employee emp = db.employees.FirstOrDefault(o => o.id == empId);
                if (emp != null)
                {
                    int valueTo = int.Parse(_valueTo);
                    string toNotes = _toNotes;
                    if (_day != null)
                    {
                        DateTime date;
                        System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                        DateTime.TryParseExact(_day, "yyyy-MM-dd", enUS,
                                                  System.Globalization.DateTimeStyles.None, out date);
                        if (date != null)
                        {
                            emp.credit--;
                            db.Entry(emp).State = System.Data.EntityState.Modified;
                            money money = new money { employee = emp, currentCredit = emp.credit, import = false, notes = toNotes, value = valueTo, recieveDate = date };
                            db.moneys.Add(money);
                            db.SaveChanges();
                        }
                    }
                        
                }
            }
            else if (_otherCheckBox == "on")
            {
                string other = _otherTo;
             
                    int valueTo = int.Parse(_valueTo);
                    string toNotes = _toNotes;
                    if (_day != null)
                    {
                        DateTime date;
                        System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                        DateTime.TryParseExact(_day, "yyyy-MM-dd", enUS,
                                                  System.Globalization.DateTimeStyles.None, out date);
                        if (date != null)
                        {

                            money money = new money { other = other, currentCredit = 0, import = false, notes = toNotes, value = valueTo, recieveDate = date };
                            db.moneys.Add(money);
                            db.SaveChanges();
                        }

                    }

            }

            return RedirectToAction("viewMoneyDays", new { day = _day });

        }
       
        public ActionResult Import()
        {
            string _doctorSelect = Request.Form["doctorSelect"];
            string _valueFrom = Request.Form["valueFrom"];
            string _fromNotes = Request.Form["fromNotes"];
  
            string _day = Request.Form["day"];

            teethLabEntities db = new teethLabEntities();

            if (_doctorSelect != null && _doctorSelect != string.Empty)
                {
                    int doctorId = int.Parse(_doctorSelect);
                    doctor doc = db.doctors.FirstOrDefault(o => o.id == doctorId);
                    if (doc != null)
                    {
                        int valueFrom = int.Parse(_valueFrom);
                        string fromNotes = _fromNotes;
                        if (_day != null)
                        {
                            DateTime date;
                            System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                            DateTime.TryParseExact(_day, "yyyy-MM-dd", enUS,
                                                      System.Globalization.DateTimeStyles.None, out date);
                            if (date != null)
                            {
                                doc.Credit--;
                                db.Entry(doc).State = System.Data.EntityState.Modified;
                                money money = new money { doctor = doc, currentCredit = doc.Credit, import = true, notes = fromNotes, value = valueFrom, recieveDate = date };
                                db.moneys.Add(money);
                                db.SaveChanges();
                            }
                        }
                    }
                }
            
            return RedirectToAction("viewMoneyDays", new { day = _day });

        }
        public ActionResult delete() { 
        string _moneyId = Request.Form["moneyId"];
            if(_moneyId!=null&& _moneyId!=string.Empty ){
                int moneyId= int.Parse(_moneyId);
                teethLabEntities db = new teethLabEntities();
                money mon = db.moneys.FirstOrDefault(m=>m.id==moneyId);
                if(mon !=null){
                    if (mon.employee != null)
                    {
                        if (mon.import)
                        {
                            mon.employee.credit += mon.value;
                        }
                        else
                        {
                            mon.employee.credit -= mon.value;
                        }
                    }
                    else if (mon.doctor != null)
                    {
                        if (mon.import)
                        {
                            mon.doctor.Credit += mon.value;

                        }
                        else
                        {
                            mon.doctor.Credit -= mon.value;
                        }
                    }
                    else if (mon.company != null)
                    {
                        if (mon.import)
                        {
                            mon.company.credit += mon.value;
                        }
                        else
                        {
                            mon.company.credit -= mon.value;
                        }
                    }
                    db.Entry(mon).State = System.Data.EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("viewMoneyDays", new { day = Request.Form["day"] });
        }
        public ActionResult viewMoneyDays()
        {
            teethLabEntities db = new teethLabEntities();
            //List<doctor> doctors = db.doctors.Where(d => d.isActive == true).ToList();
            //ViewData["doctors"] = doctors;
            //ViewData["emps"] = db.employees.ToList();
            //ViewData["companies"] = db.companies.ToList();

            //this.newDay();

            string day = Request.QueryString["day"];
            if (day != null)
            {
                DateTime date;
                System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                DateTime.TryParseExact(day, "yyyy-MM-dd", enUS,
                                          System.Globalization.DateTimeStyles.None, out date);
                ViewData["day"] = date;
            }
            return View();
        }



    }
}
