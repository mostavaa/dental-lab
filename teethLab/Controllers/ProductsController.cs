using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace teethLab.Controllers
{
    public class ProductsController : Controller
    {
        private teethLabEntities db = new teethLabEntities();

        //
        // GET: /Products/

        public ActionResult Index()
        {
            return View(db.products.ToList());
        }

        //
        // GET: /Products/Details/5

        public ActionResult Details(int id = 0)
        {
            product product = db.products.Find(id);
            ViewBag.units = product.ProductUnits.ToList();

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // GET: /Products/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Products/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(product product)
        {
            product.enterDate = DateTime.Now;

            //   product.ProductUnits.Add()
            string[] units = Request.Form["units[]"].Split(',');

            foreach (var item in units)
            {
                ProductUnit pu = new ProductUnit();
                pu.unitId = int.Parse(item);
                product.ProductUnits.Add(pu);
            }


            if (ModelState.IsValid)
            {
                db.products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        //
        // GET: /Products/Edit/5

        public ActionResult Edit(int id = 0)
        {
            product product = db.products.Find(id);

            ViewBag.units = product.ProductUnits.ToList();

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Products/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(product product)
        {
            product.enterDate = DateTime.Now;
            product editproduct = db.products.Find(product.id);

            List<ProductUnit> produnits = editproduct.ProductUnits.ToList();
            string[] units = Request.Form["units[]"].Split(',');

            foreach (var item in db.ProductUnits.Where(o => o.productId == product.id))
            {
                db.Entry(item).State = EntityState.Deleted;
            }
            foreach (var item in units)
            {
                ProductUnit pu = new ProductUnit();
                pu.unitId = int.Parse(item);
                pu.productId = editproduct.id;
                editproduct.ProductUnits.Add(pu);
            }

            db.products.AddOrUpdate(product);
            if (ModelState.IsValid)
            {
                // db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        //
        // GET: /Products/Delete/5

        public ActionResult Delete(int id = 0)
        {
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Products/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            product product = db.products.Find(id);
            db.products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void addNewUnit()
        {
            string newUnit = Request["newUnit"];
            if (newUnit != null && newUnit != string.Empty)
            {
                db = new teethLabEntities();
                if (db.Units.Where(o => o.unit1.ToLower() == newUnit.ToLower()).Count() == 0)
                {
                    Unit unit = new Unit { unit1 = newUnit };
                    db.Units.Add(unit);
                    db.SaveChanges();
                    Response.Write("success,"+unit.id);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}