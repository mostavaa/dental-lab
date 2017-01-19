using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace teethLab.Controllers
{
    public class caseDoctorPricesController : Controller
    {
        private teethLabEntities db = new teethLabEntities();

        //
        // GET: /caseDoctorPrices/

        public ActionResult Index()
        {
            var casedoctorprices = db.caseDoctorPrices.Include(c => c.CurrentCas).Include(c => c.doctor);
            return View(casedoctorprices.ToList());
        }

        //
        // GET: /caseDoctorPrices/Details/5

        public ActionResult Details(int id = 0)
        {
            caseDoctorPrice casedoctorprice = db.caseDoctorPrices.Find(id);
            if (casedoctorprice == null)
            {
                return HttpNotFound();
            }
            return View(casedoctorprice);
        }

        //
        // GET: /caseDoctorPrices/Create

        public ActionResult Create()
        {
            ViewBag.caseId = new SelectList(db.CurrentCases, "id", "name");
            ViewBag.doctorId = new SelectList(db.doctors, "id", "name");
            return View();
        }

        //
        // POST: /caseDoctorPrices/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(caseDoctorPrice casedoctorprice)
        {
            if (ModelState.IsValid)
            {
                db.caseDoctorPrices.Add(casedoctorprice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.caseId = new SelectList(db.CurrentCases, "id", "name", casedoctorprice.caseId);
            ViewBag.doctorId = new SelectList(db.doctors, "id", "name", casedoctorprice.doctorId);
            return View(casedoctorprice);
        }

        //
        // GET: /caseDoctorPrices/Edit/5

        public ActionResult Edit(int id = 0)
        {
            caseDoctorPrice casedoctorprice = db.caseDoctorPrices.Find(id);
            if (casedoctorprice == null)
            {
                return HttpNotFound();
            }
            ViewBag.caseId = new SelectList(db.CurrentCases, "id", "name", casedoctorprice.caseId);
            ViewBag.doctorId = new SelectList(db.doctors, "id", "name", casedoctorprice.doctorId);
            return View(casedoctorprice);
        }

        //
        // POST: /caseDoctorPrices/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(caseDoctorPrice casedoctorprice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(casedoctorprice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.caseId = new SelectList(db.CurrentCases, "id", "name", casedoctorprice.caseId);
            ViewBag.doctorId = new SelectList(db.doctors, "id", "name", casedoctorprice.doctorId);
            return View(casedoctorprice);
        }

        //
        // GET: /caseDoctorPrices/Delete/5

        public ActionResult Delete(int id = 0)
        {
            caseDoctorPrice casedoctorprice = db.caseDoctorPrices.Find(id);
            if (casedoctorprice == null)
            {
                return HttpNotFound();
            }
            return View(casedoctorprice);
        }

        //
        // POST: /caseDoctorPrices/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            caseDoctorPrice casedoctorprice = db.caseDoctorPrices.Find(id);
            db.caseDoctorPrices.Remove(casedoctorprice);
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