using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace teethLab.Controllers
{
    public class UserController : Controller
    {
        private teethLabEntities db = new teethLabEntities();

        //
        // GET: /User/

        public ActionResult Index()
        {
            return View(db.users.ToList());
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id = 0)
        {
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(user user)
        {
            if (ModelState.IsValid)
            {
                db.users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id = 0)
        {
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(user user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id = 0)
        {
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            user user = db.users.Find(id);
            db.users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        public ActionResult viewRegister()
        {
            if (!tools.isAdminLogged())
            {
                return Redirect(Url.Action("viewLogin", "user"));
            }

            ViewData["error"] = TempData["error"];
            return View();
        }
        //view login
        public ActionResult viewLogin()
        {

            ViewData["error"] = TempData["error"];
            return View();
        }
        public ActionResult logOut()
        {
            Session["userId"] = 0;
            return Redirect(Url.Action("Index", "Home"));
        }
        public ActionResult login()
        {

            string name = Request.Form["name"];
            string password = Request.Form["password"];
            teethLabEntities db = new teethLabEntities();
            if (db.users.Where(u => u.name == name && u.password == password).Count() > 0)
            {
                user user = db.users.Where(u => u.name == name && u.password == password).First();
                Session.Add("userId", user.id);
                return Redirect(Url.Action("Index", "Home"));
            }

            TempData["error"] = "الاسم او الرقم السري غير صحيح";
            return Redirect(Url.Action("viewLogin", "user"));
        }
        public ActionResult register()
        {
            string name = Request.Form["name"];
            string password = Request.Form["password"];
            string passwordConfirm = Request.Form["confirmPassword"];
            teethLabEntities db = new teethLabEntities();
            if (db.users.Where(o => o.name == name).Count() > 0)
            {

                TempData["error"] = "الاسم موجود مسبقآ , رجاء اختيار اسم اخر";

                return Redirect(Url.Action("viewRegister", "user"));
            }
            if (password != passwordConfirm)
            {
                TempData["error"] = "الرقم السري غير مطابق";
                return Redirect(Url.Action("viewRegister", "user"));
            }

            user user = new user();
            user.name = name;
            user.password = password;
            db.users.Add(user);
            db.SaveChanges();
            TempData["error"] = "تمت الاضافه بنجاح";
            return Redirect(Url.Action("viewRegister", "user"));
        }
    }
}