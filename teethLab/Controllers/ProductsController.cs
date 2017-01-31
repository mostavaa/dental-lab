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

        public JsonResult all()
        {
            db.Configuration.ProxyCreationEnabled = false;

            int totalRows, page = 1, size, offset, numOfPages;
            if (Request.Params["page"] != null && Request.Params["page"] != "")
            {
                page = int.Parse(Request.Params["page"]);
            }
            page--;
            size = 5;
            offset = page * size;
            totalRows = db.products.Count();
            var models = db.products.OrderBy(o => o.id).Skip(offset).Take(size).ToList();
            numOfPages = totalRows / size;
            if (totalRows % size != 0)
            {
                numOfPages++;
            }
            return Json(new { models = models, numOfPages = numOfPages }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Add(product model)
        {
            model.enterDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.products.Add(model);
                db.SaveChanges();
                return Json(new { success = true, model = model });
            }
            return Json(new { success = false });

        }


        [HttpPost]
        public JsonResult Edit(product model)
        {
            if (ModelState.IsValid)
            {    
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult Delete(product model)
        {
            product obj = db.products.Find(model.id);
            db.products.Remove(obj);
            db.SaveChanges();
            return Json(new { success = true });
        }

        public ActionResult ProductProfile(int id)
        {
            product pro = db.products.FirstOrDefault(o => o.id == id);
            if (pro != null)
            {
                return View(pro);
            }
            return HttpNotFound();
        }
        //
        


        //public void addNewUnit()
        //{
        //    string newUnit = Request["newUnit"];
        //    if (newUnit != null && newUnit != string.Empty)
        //    {
        //        db = new teethLabEntities();
        //        if (db.Units.Where(o => o.unit1.ToLower() == newUnit.ToLower()).Count() == 0)
        //        {
        //            Unit unit = new Unit { unit1 = newUnit };
        //            db.Units.Add(unit);
        //            db.SaveChanges();
        //            Response.Write("success,"+unit.id);
        //        }
        //    }
        //}

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}