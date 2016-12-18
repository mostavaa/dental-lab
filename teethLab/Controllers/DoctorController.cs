using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace teethLab.Controllers
{
    public class DoctorController : Controller
    {
        //
        // GET: /Doctor/
        /// <summary>
        /// add , edit , delete Doctor
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            /*
            if (!tools.isUserLogged())
            {
                return Redirect(Url.Action("viewLogin", "user"));
            }
             */
            teethLabEntities db = new teethLabEntities();
            ViewData["doctors"] = db.doctors.ToList();
            return View();
        }
        /// <summary>
        /// add Dooctor
        /// </summary>
        /// <returns></returns>
        public ActionResult viewAddDoc()
        {
            teethLabEntities db = new teethLabEntities();
            ViewData["suppliers"] = db.suppliers.ToList();
            return View();
        }
        /// <summary>
        /// edit Dorctor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult viewEditDoc(int id)
        {
            teethLabEntities db = new teethLabEntities();

            ViewData["suppliers"] = db.suppliers.ToList();
            ViewData["doctor"] = db.doctors.Find(id);
            return View();
        }
        /// <summary>
        /// doctor cases
        /// </summary>
        /// <returns></returns>
        public ActionResult doctor()
        {
            ViewData["money"] = TempData["moneyDays"];
            ViewData["transactions"] = TempData["transactions"];
            ViewData["doctor"] = TempData["doctor"];
            return View();
        }
        public ActionResult viewDoctor()
        {
            int docId = int.Parse(Request.Form["doctorId"]);
            DateTime fromDate;
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime.TryParseExact(Request.Form["fromDate"], "yyyy-MM-dd", enUS,
                        DateTimeStyles.None, out fromDate);
            DateTime toDate;
            enUS = new CultureInfo("en-US");
            DateTime.TryParseExact(Request.Form["toDate"], "yyyy-MM-dd", enUS,
                        DateTimeStyles.None, out toDate);
            teethLabEntities db = new teethLabEntities();
            TempData["money"] = db.moneys.Where(md => md.recieveDate >= fromDate && md.recieveDate <= toDate && md.doctorId == docId).ToList();
            TempData["transactions"] = db.transactions.Where(t => t.doctorId == docId && t.recieveDate >= fromDate && t.recieveDate <= toDate).ToList();
            TempData["doctor"] = db.doctors.Find(docId);
            return Redirect(Url.Action("doctor", "Doctor"));

        }
        public void changeCasePrice()
        {
            teethLabEntities db = new teethLabEntities();
            int docId = int.Parse(Request.Form["docId"]);
            int caseId = int.Parse(Request.Form["caseId"]);
            int price = int.Parse(Request.Form["price"]);
            caseDoctorPrice cp = null;
            if (db.caseDoctorPrices.Where(o => o.caseId == caseId && o.doctorId == docId).Count() > 0)
            {
                cp = db.caseDoctorPrices.Where(o => o.caseId == caseId && o.doctorId == docId).First();
                db.Entry(cp).State = System.Data.EntityState.Modified;
                cp.price = price;
            }
            else
            {
                cp = new caseDoctorPrice();
                cp.doctorId = docId;
                cp.caseId = caseId;
                cp.price = price;
                db.Entry(cp).State = System.Data.EntityState.Added;
            }
            db.SaveChanges();
            Response.Write("success");
        }
        public void activateDoc()
        {
            teethLabEntities db = new teethLabEntities();
            int docId = int.Parse(Request.Form["id"]);

            doctor doc = db.doctors.Find(docId);
            db.Entry(doc).State = System.Data.EntityState.Modified;
            if (doc.isActive)
            {
                doc.isActive = false;
            }
            else
            {
                doc.isActive = true;
            }
            db.SaveChanges();
            Response.Write("success");
        }
        public void changeCase()
        {
            teethLabEntities db = new teethLabEntities();
            int transId = int.Parse(Request.Form["transId"]);
            transaction trans = db.transactions.Find(transId);
            db.Entry(trans).State = System.Data.EntityState.Modified;
            if (trans.isOut == true)
            {
                trans.isOut = false;
            }
            else
            {
                trans.isOut = true;
            }
            db.SaveChanges();
            Response.Write("success");
        }
        public ActionResult editDoc()
        {

            string name = Request.Form["name"];
            string address = Request.Form["address"];
            int supplierId = int.Parse(Request.Form["supplier"]);
            int docId = int.Parse(Request.Form["id"]);
            teethLabEntities db = new teethLabEntities();
            doctor doc = db.doctors.Find(docId);
            doc.name = name;
            doc.address = address;

            if (supplierId != 0)
                doc.supplierId = supplierId;
            db.Entry(db.doctors.Find(docId)).State = System.Data.EntityState.Modified;
            db.SaveChanges();

            return Redirect(Url.Action("Index", "Doctor"));
        }
        public ActionResult addDoc()
        {


            string name = Request.Form["name"];
            string address = Request.Form["address"];
            int supplierId = int.Parse(Request.Form["supplier"]);

            teethLabEntities db = new teethLabEntities();
            doctor doc = new doctor();
            doc.name = name;
            doc.address = address;
            doc.depit = 0;
            doc.isActive = false;
            if (supplierId != 0)
                doc.supplierId = supplierId;
            db.doctors.Add(doc);
            db.SaveChanges();

            return Redirect(Url.Action("Index", "Doctor"));

        }
        public void deleteDoc()
        {
            teethLabEntities db = new teethLabEntities();
            int docId = int.Parse(Request.Form["id"]);
            db.Entry(db.doctors.Find(docId)).State = System.Data.EntityState.Deleted;
            db.SaveChanges();
            Response.Write("success");
        }




        ///////////// new one 
        public ActionResult clientsdatabase()
        {
            teethLabEntities db = new teethLabEntities();
            ViewData["docs"] = db.doctors.ToList();
            return View();
        }

        public void getDoctorByName()
        {
            string val = Request.Form["val"];
            doctormodel doc = new doctormodel();
            List<doctor> docs = doc.getdoctorbyname(val);
            if (docs != null && docs.Count > 0)
            {
                foreach (doctor docc in docs)
                {
                    Response.Write(docc.id + "," + docc.name + "&");
                }
            }
        }

        public void getDoctorById()
        {
            string id = Request.Form["id"];
            doctormodel doc = new doctormodel();
            doctor doct = doc.getdoctorbyid(int.Parse(id));
            if (doct != null)
            {
                Response.Write(doct.name + "," + doct.address + ","+doct.isActive+ "&");
                List<doctorPhone> docphones = doct.doctorPhones.ToList();
                if (docphones != null && docphones.Count > 0)
                {
                    foreach (doctorPhone docphone in docphones)
                    {
                        Response.Write(docphone.phone + "^");
                    }

                }
                Response.Write("&");
                List<caseDoctorPrice> docprices = doc.getDoctorPrices(doct.id);
                if (docprices != null && docprices.Count > 0)
                {
                    foreach (caseDoctorPrice price in docprices)
                    {
                        Response.Write(price.@case.name + "," + price.price + "," + price.id + "^");
                    }
                }
            }


        }

        public void getallcases()
        {
            doctormodel dm = new doctormodel();
            List<@case> allcases = dm.getallcases();
            if (allcases != null && allcases.Count > 0)
            {
                foreach (@case cs in allcases)
                {
                    Response.Write(cs.id + "," + cs.name + "&");
                }
            }

        }
        public void getDoctorClinics()
        {
            int docId = int.Parse(Request.Form["id"]);
            teethLabEntities db = new teethLabEntities();
            
            foreach (clinic c in db.clinics.Where(c => c.doctorId == docId))
            {
                Response.Write("<hr>");
                Response.Write("<div class='row'>");
                            Response.Write("<div class='col-md-3'>");
                            Response.Write("<label class='alllabels'>Clinic Name : </label>");
                            Response.Write("</div>");
                            Response.Write("<div class='col-md-9'>");
                            Response.Write("<label class='alllabels'>" + c.name + "</label>");
                            Response.Write("</div>");
                            Response.Write("</div>");

                            Response.Write("<div class='row'>");
                            Response.Write("<div class='col-md-3'>");
                            Response.Write("<label class='alllabels'>Clinic Address : </label>");
                            Response.Write("</div>");
                            Response.Write("<div class='col-md-9'>");
                            Response.Write("<label class='alllabels'>" + c.address + "</label>");
                            Response.Write("</div>");
                            Response.Write("</div>");



                            Response.Write("<div class='row'>");
                            Response.Write("<div class='col-md-3'>");
                            Response.Write("<label class='alllabels'>Clinic Phones : </label>");
                            Response.Write("</div>");
                            Response.Write("<div class='col-md-9'>");
                    

                            foreach (clinicPhone cp in c.clinicPhones)
                            {
                                Response.Write("<label class='alllabels'>" + cp.phone + "</label>  ,  ");

                            }
                            Response.Write("<br><button class='btn btn-primary deleteClinic' data-id='"+c.id+"'>Delete</button>");
                            Response.Write("</div>");
                            Response.Write("</div>");
            }

        }
        public void DeleteClinic()
        {
            int cliId = int.Parse(Request.Form["id"]);
            teethLabEntities db = new teethLabEntities();
            clinic cli = db.clinics.Find(cliId);
            foreach (clinicPhone clp in cli.clinicPhones)
            {
                db.Entry(clp).State = System.Data.EntityState.Deleted;
            }
            db.Entry(cli).State = System.Data.EntityState.Deleted;
            db.SaveChanges();
            Response.Write("success");
        }

        public void addNewClinic()
        {
            int docId = int.Parse(Request.Form["doctorid"]);
            string name = Request.Form["name"];
            string address = Request.Form["address"];
            string phones = Request.Form["phones"];
            if (name == "" || name == null
    || address == "" || address == null
    )
            {
                return;
            }
            teethLabEntities db = new teethLabEntities();
            clinic cli = new clinic();
            cli.name = name;
            cli.address = address;
            cli.doctorId = docId;

            db.clinics.Add(cli);
            string[] allphones = phones.Split(',');
            for (int i = 0; i < allphones.Length - 1; i++)
            {
                clinicPhone cp = new clinicPhone();
                cp.phone = allphones[i];
                cp.clinicId = cli.id;
                db.clinicPhones.Add(cp);
                
            }
            db.SaveChanges();
            Response.Write("success");
        }
        /// <summary>
        /// add and edit
        /// </summary>
        public void addnewdoctor()
        {

            string name = Request.Form["name"];
            string address = Request.Form["address"];
            string phones = Request.Form["phones"];
            string products = Request.Form["products"];
          //  bool active = bool.Parse(Request.Form["activate"]);
            if (name == "" || name == null
                || address == "" || address == null
                || products == "" || products == null
                )
            {
                return;
            }

            string action = Request.Form["action"];

            teethLabEntities db = new teethLabEntities();

            doctor newdoc = null;
            if (action == "add")
            {
                newdoc = new teethLab.doctor();
            }
            else if (action == "edit")
            {
                int id = int.Parse(Request.Form["doctorid"]);
                newdoc = db.doctors.Find(id);
            }

            newdoc.name = name;
            newdoc.address = address;
//            newdoc.isActive = active;
            newdoc.isActive = false;
            doctormodel dm = new doctormodel();

            if (action == "add")
            {
                dm.addnewdoctor(newdoc);

            }

            if (action == "edit")
            {
                dm.deletealldoctorphones(newdoc.id);
                db.Entry(newdoc).State = System.Data.EntityState.Modified;
                db.SaveChanges();
            }

            string[] allphones = phones.Split(',');
            for (int i = 0; i < allphones.Length - 1; i++)
            {
                doctorPhone dp = new doctorPhone();
                dp.phone = allphones[i];
                dp.doctorId = newdoc.id;
                if (!dm.addnewdoctorphone(dp))
                {
                    return;
                }
            }


            if (action == "add")
            {
                string[] allproducts = products.Split('^');
                for (int i = 0; i < allproducts.Length - 1; i++)
                {
                    caseDoctorPrice cp = new caseDoctorPrice();
                    cp.caseId = int.Parse(allproducts[i].Split(',')[0]);
                    cp.doctorId = newdoc.id;
                    if (allproducts[i].Split(',')[1] == "") {
                        cp.price = db.cases.Find(cp.caseId).defaultPrice;

                    }
                    else
                    {
                        cp.price = int.Parse(allproducts[i].Split(',')[1]);
                    }
                    dm.addnewcaseprice(cp);
                }
            }
            else if (action == "edit")
            {
                string[] allproducts = products.Split('^');
                 db = new teethLabEntities();
                for (int i = 0; i < allproducts.Length - 1; i++)
                {
                    int id=int.Parse(allproducts[i].Split(',')[0]);
                    
                    caseDoctorPrice cp =db.caseDoctorPrices.Find(id);
                    cp.price = int.Parse(allproducts[i].Split(',')[1]);
                    db.Entry(cp).State = System.Data.EntityState.Modified;
                }
                db.SaveChanges();
            }

            Response.Write("success");
        }

        public ActionResult navigatetodoctor()
        {

            return Redirect(Url.Action("viewdoctorcases", "Doctor"));
        }
        public ActionResult viewdoctorcases()
        {
            int docid = int.Parse(Request.Form["docid"]);
            teethLabEntities db = new teethLabEntities();
            doctor doc = db.doctors.Find(docid);
            if (doc == null)
                return null;
            doc.isActive = true;
            db.Entry(doc).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            ViewData["docid"] = docid;

            return View();
        }

        public ActionResult Report()
        {
            DateTime fromDate = this.dateFromString(Request.Form["fromDate"]);
            DateTime toDate = this.dateFromString(Request.Form["toDate"]);
            if (fromDate == null || toDate == null)
            {
                return null;
            }
            int docid = int.Parse(Request.Form["docid"]);
            teethLabEntities db = new teethLabEntities();
            List<transaction> trans;
            if (db.transactions.Where(o => o.recieveDate >= fromDate && o.recieveDate <= toDate && o.doctorId==docid).Count() > 0)
            {
                trans = db.transactions.Where(o => o.recieveDate >= fromDate && o.recieveDate <= toDate && o.doctorId == docid).ToList();
            }
            else
            {
                trans = new List<transaction>();
            }

            List<money> m;
            if (db.moneys.Where(o => o.moneyDay.day >= fromDate && o.moneyDay.day<= toDate && o.doctorId==docid ).Count() > 0)
            {
                m = db.moneys.Where(o => o.moneyDay.day >= fromDate && o.moneyDay.day <= toDate && o.doctorId == docid).ToList();
            }
            else
            {
                m = new List<money>();
            }
            ViewData["m"] = m;
            ViewData["trans"] = trans;
            ViewData["docid"] = docid;
            return View();

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

    public class doctormodel : doctor
    {
        public int getCurrentDoctorCredit(int docId)
        {
            return this.toDocSum(docId) - this.FrmoDocSum(docId);
        }
        public int FrmoDocSum(int docId)
        {
            teethLabEntities db = new teethLabEntities();
            int FromDoctorSum = 0;
            if (db.moneys.Where(d => d.doctorId == docId).Sum(o => o.value) != null)
            {
                FromDoctorSum = db.moneys.Where(d => d.doctorId == docId).Sum(o => o.value);
            }
            return FromDoctorSum;
        }
        public int toDocSum(int docId)
        {
            teethLabEntities db = new teethLabEntities();
            int toDoctorSum = 0;
            if (db.transactions.Where(d => d.doctorId == docId).Sum(o => o.price) != null)
            {
                toDoctorSum = db.transactions.Where(d => d.doctorId == docId).Sum(o => o.price);
            }
            return toDoctorSum;
        }

        private teethLabEntities context;

        public doctormodel()
        {
            this.context = new teethLabEntities();
        }
        public void deletealldoctorphones(int doctorid)
        {
            List<doctorPhone> phones = this.context.doctorPhones.Where(d => d.doctorId == doctorid).ToList();
            if (phones != null && phones.Count > 0)
            {
                foreach (doctorPhone phone in phones)
                {
                    this.context.Entry(phone).State = System.Data.EntityState.Deleted;
                }
                this.context.SaveChanges();
            }
        }
        public bool addnewcaseprice(caseDoctorPrice cp)
        {
            this.context.caseDoctorPrices.Add(cp);
            this.context.SaveChanges();
            this.context = new teethLabEntities();
            return true;
        }
        public bool addnewdoctorphone(doctorPhone dp)
        {
            this.context.doctorPhones.Add(dp);
            this.context.SaveChanges();
            this.context = new teethLabEntities();
            return true;
        }
        public bool addnewdoctor(doctor doc)
        {
            this.context.doctors.Add(doc);
            this.context.SaveChanges();
            this.context = new teethLabEntities();
            return true;
        }
        public List<doctor> getdoctorbyname(string name)
        {

            List<doctor> docs = new List<doctor>();
            if (this.context.doctors.Where(o => o.name.Contains(name)).Count() > 0)
            {
                docs = this.context.doctors.Where(o => o.name.Contains(name)).ToList();
            }
            return docs;
        }

        public doctor getdoctorbyid(int id)
        {
            return this.context.doctors.Find(id);
        }

        public List<caseDoctorPrice> getDoctorPrices(int docId)
        {
            if (this.context.caseDoctorPrices.Where(c => c.doctorId == docId).Count() > 0)
            {
                return this.context.caseDoctorPrices.Where(c => c.doctorId == docId).ToList();
            }
            return null;
        }
        public List<@case> getallcases()
        {
            if (this.context.cases.Count() > 0)
            {
                return this.context.cases.ToList();
            }
            return null;
        }
    }
}
