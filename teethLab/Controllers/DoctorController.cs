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
            totalRows = db.doctors.Count();
            var models = db.doctors.OrderBy(o => o.id).Skip(offset).Take(size).ToList();
            numOfPages = totalRows / size;
            if (totalRows % size != 0)
            {
                numOfPages++;
            }


            return Json(new { models = models, numOfPages = numOfPages }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Add(doctor model)
        {
            if (ModelState.IsValid)
            {
                db.doctors.Add(model);
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
        public JsonResult Edit(doctor model)
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
        public JsonResult Delete(doctor model)
        {
            doctor obj = db.doctors.Find(model.id);
            db.doctors.Remove(obj);
            db.SaveChanges();
            return Json(new { success = true });
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

        ////
        //// GET: /Doctor/Details/5

        //public ActionResult Details(int id = 0)
        //{
        //    doctor doctor = db.doctors.Find(id);
        //    if (doctor == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(doctor);
        //}

        ////
        //// GET: /Doctor/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        ////
        //// POST: /Doctor/Create

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(doctor doctor)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.doctors.Add(doctor);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(doctor);
        //}

        ////
        //// GET: /Doctor/Edit/5

        //public ActionResult Edit(int id = 0)
        //{
        //    doctor doctor = db.doctors.Find(id);
        //    if (doctor == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(doctor);
        //}

        ////
        //// POST: /Doctor/Edit/5

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(doctor doctor)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(doctor).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(doctor);
        //}

        ////
        //// GET: /Doctor/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    doctor doctor = db.doctors.Find(id);
        //    if (doctor == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(doctor);
        //}

        ////
        //// POST: /Doctor/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    doctor doctor = db.doctors.Find(id);
        //    db.doctors.Remove(doctor);
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