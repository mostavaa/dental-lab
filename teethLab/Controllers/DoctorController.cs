using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace teethLab.Controllers
{
    public class DoctorController : Controller
    {
        private teethLabEntities db = new teethLabEntities();

        //
        // GET: /Doctor/

        public ActionResult Index()
        {
            return View(db.doctors.ToList());
        }
        public ActionResult DoctorProfile(int id)
        {
            doctor doc = db.doctors.FirstOrDefault(o => o.id == id);
            if (doc != null)
            {
                return View(doc);
            }
            return HttpNotFound();
        }

        //
        // GET: /Doctor/Details/5

        public ActionResult Details(int id = 0)
        {
            doctor doctor = db.doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        //
        // GET: /Doctor/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Doctor/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.doctors.Add(doctor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(doctor);
        }

        //
        // GET: /Doctor/Edit/5

        public ActionResult Edit(int id = 0)
        {
            doctor doctor = db.doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        //
        // POST: /Doctor/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(doctor);
        }

        //
        // GET: /Doctor/Delete/5

        public ActionResult Delete(int id = 0)
        {
            doctor doctor = db.doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        //
        // POST: /Doctor/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            doctor doctor = db.doctors.Find(id);
            db.doctors.Remove(doctor);
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