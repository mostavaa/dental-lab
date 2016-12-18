using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace teethLab.Controllers
{
    public class CompController : Controller
    {
        private teethLabEntities db = new teethLabEntities();

        //
        // GET: /Comp/

        public ActionResult Index()
        {
            return View(db.companies.ToList());
        }

        //
        // GET: /Comp/Details/5

        public ActionResult Details(int id = 0)
        {
            company company = db.companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        //
        // GET: /Comp/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Comp/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(company company)
        {
            if (ModelState.IsValid)
            {
                db.companies.Add(company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(company);
        }

        //
        // GET: /Comp/Edit/5

        public ActionResult Edit(int id = 0)
        {
            company company = db.companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        //
        // POST: /Comp/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(company company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(company);
        }

        //
        // GET: /Comp/Delete/5

        public ActionResult Delete(int id = 0)
        {
            company company = db.companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        //
        // POST: /Comp/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            company company = db.companies.Find(id);
            db.companies.Remove(company);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }



        public void deleteDof3a()
        {

            int Id = int.Parse(Request.Form["id"]);
            CompanyClass com = new CompanyClass();
            if (com.deleteDof3a(Id))
            {
                Response.Write("success");
            }
            else
            {
                Response.Write("fail");
            }
        }
        public void AddDof3a()
        {
            int productId = int.Parse(Request.Form["id"]);
            int cost = int.Parse(Request.Form["cost"]);

            DateTime date;
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime.TryParseExact(Request.Form["date"], "yyyy-MM-dd", enUS,
                        DateTimeStyles.None, out date);
            CompanyClass com = new CompanyClass();
            int productPayId = 0;
            if (com.AddDof3a(productId, cost, date, out productPayId))
            {
                Response.Write("success," + productPayId);
            }
            else
            {
                Response.Write("fail");
            }

        }

    }
}