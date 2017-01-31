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


        public JsonResult all()
        {
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            int totalRows, page = 1, size, offset, numOfPages;
            if (Request.Params["page"] != null && Request.Params["page"] != "")
            {
                page = int.Parse(Request.Params["page"]);
            }
            page--;
            size = 5;
            offset = page * size;
            totalRows = db.CurrentCases.Count();
            var models = db.CurrentCases.OrderBy(o => o.id).Skip(offset).Take(size).ToList();
            numOfPages = totalRows / size;
            if (totalRows % size != 0)
            {
                numOfPages++;
            }


            return Json(new { models = models, numOfPages = numOfPages }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Add(CurrentCas model)
        {
            if (ModelState.IsValid)
            {
                db.CurrentCases.Add(model);
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
        public JsonResult Edit(CurrentCas model)
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
        public JsonResult Delete(CurrentCas model)
        {
            CurrentCas obj = db.CurrentCases.Find(model.id);
            db.CurrentCases.Remove(obj);
            db.SaveChanges();
            return Json(new { success = true });
        }


        ////
        //// GET: /Cases/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        ////
        //// POST: /Cases/Create

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(CurrentCas currentcas)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.CurrentCases.Add(currentcas);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(currentcas);
        //}

        ////
        //// GET: /Cases/Edit/5

        //public ActionResult Edit(int id = 0)
        //{
        //    CurrentCas currentcas = db.CurrentCases.Find(id);
        //    if (currentcas == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(currentcas);
        //}

        ////
        //// POST: /Cases/Edit/5

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(CurrentCas currentcas)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(currentcas).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(currentcas);
        //}

        ////
        //// GET: /Cases/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    CurrentCas currentcas = db.CurrentCases.Find(id);
        //    if (currentcas == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(currentcas);
        //}

        ////
        //// POST: /Cases/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    CurrentCas currentcas = db.CurrentCases.Find(id);
        //    db.CurrentCases.Remove(currentcas);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}