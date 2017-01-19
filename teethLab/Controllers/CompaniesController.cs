using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace teethLab.Controllers
{
    public class CompaniesController : Controller
    {
        private teethLabEntities db = new teethLabEntities();

        //
        // GET: /Companies/

        public ActionResult Index()
        {
            return View(db.companies.ToList());
        }
        public ActionResult CompanyProfile(int id) {
            company com = db.companies.FirstOrDefault(o => o.id == id);
            if (com != null)
            {
                return View(com);
            }
            return HttpNotFound();
        }
        //
        // GET: /Companies/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Companies/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(company company)
        {
            company.enterDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.companies.Add(company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(company);
        }

        //
        // GET: /Companies/Edit/5

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
        // POST: /Companies/Edit/5

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
        // GET: /Companies/Delete/5

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
        // POST: /Companies/Delete/5

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
    }
}