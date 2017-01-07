using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace teethLab.Controllers
{
    public class CasesController : Controller
    {
        private teethLabEntities db = new teethLabEntities();

        //
        // GET: /Cases/

        public ActionResult Index()
        {
            return View(db.CurrentCases.ToList());
        }



        //
        // GET: /Cases/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Cases/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CurrentCas currentcas)
        {
            if (ModelState.IsValid)
            {
                db.CurrentCases.Add(currentcas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(currentcas);
        }

        //
        // GET: /Cases/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CurrentCas currentcas = db.CurrentCases.Find(id);
            if (currentcas == null)
            {
                return HttpNotFound();
            }
            return View(currentcas);
        }

        //
        // POST: /Cases/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CurrentCas currentcas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(currentcas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(currentcas);
        }

        //
        // GET: /Cases/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CurrentCas currentcas = db.CurrentCases.Find(id);
            if (currentcas == null)
            {
                return HttpNotFound();
            }
            return View(currentcas);
        }

        //
        // POST: /Cases/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CurrentCas currentcas = db.CurrentCases.Find(id);
            db.CurrentCases.Remove(currentcas);
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