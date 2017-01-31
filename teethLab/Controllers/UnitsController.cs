using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace teethLab.Controllers
{
    public class UnitsController : Controller
    {
        private teethLabEntities db = new teethLabEntities();

        //
        // GET: /Units/

        public ActionResult Index()
        {
            return View(db.Units.ToList());
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
            totalRows = db.Units.Count();
            var units = db.Units.OrderBy(o => o.id).Skip(offset).Take(size).ToList();
            numOfPages = totalRows / size;
            if (totalRows % size != 0)
            {
                numOfPages++;
            }
            return Json(new { models = units, numOfPages = numOfPages }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Add(Unit model)
        {
            if (ModelState.IsValid)
            {
                db.Units.Add(model);
                db.SaveChanges();
                return Json(new { success = true, model = model });
            }
            else
            {
                var errors = getErrors(ModelState);
                var res = new { success = false, errors = errors };

                return Json(res);
            }
        }

        private Dictionary<string, List<string>> getErrors(ModelStateDictionary ModelState)
        {
            Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
            foreach (var key in ModelState.Keys)
            {
                if (ModelState[key].Errors.Count > 0)
                {
                    List<string> keyErrors = new List<string>();
                    foreach (var error in ModelState[key].Errors)
                    {
                        keyErrors.Add(error.ErrorMessage);
                    }
                    errors.Add(key, keyErrors);
                }
            }
            return errors;
        }


        [HttpPost]
        public JsonResult Edit(Unit model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                var errors = getErrors(ModelState);
                var res = new { success = false, errors = errors };

                return Json(res);
            }
        }
        [HttpPost]
        public JsonResult Delete(Unit model)
        {
            Unit unit = db.Units.Find(model.id);
            db.Units.Remove(unit);
            db.SaveChanges();
            return Json(new { success = true });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}