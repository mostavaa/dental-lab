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
            totalRows = db.companies.Count();
            var models = db.companies.OrderBy(o => o.id).Skip(offset).Take(size).ToList();
            numOfPages = totalRows / size;
            if (totalRows % size != 0)
            {
                numOfPages++;
            }


            return Json(new { models = models, numOfPages = numOfPages }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Add(company model)
        {
            model.enterDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.companies.Add(model);
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
        public JsonResult Edit(company model)
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
        public JsonResult Delete(company model)
        {
            company obj = db.companies.Find(model.id);
            db.companies.Remove(obj);
            db.SaveChanges();
            return Json(new { success = true });
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

        //public ActionResult Create()
        //{
        //    return View();
        //}

        ////
        //// POST: /Companies/Create

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(company company)
        //{
        //    company.enterDate = DateTime.Now;
        //    if (ModelState.IsValid)
        //    {
        //        db.companies.Add(company);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(company);
        //}

        ////
        //// GET: /Companies/Edit/5

        //public ActionResult Edit(int id = 0)
        //{
        //    company company = db.companies.Find(id);
        //    if (company == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(company);
        //}

        ////
        //// POST: /Companies/Edit/5

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(company company)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(company).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(company);
        //}

        ////
        //// GET: /Companies/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    company company = db.companies.Find(id);
        //    if (company == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(company);
        //}

        ////
        //// POST: /Companies/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    company company = db.companies.Find(id);
        //    db.companies.Remove(company);
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