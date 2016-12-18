using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace teethLab.Controllers
{
    public class SupplierController : Controller
    {
        //
        // GET: /Supplier/
        // all suppliers , add and delete
        public ActionResult Index()
        {
            teethLabEntities db = new teethLabEntities();
            ViewData["suppliers"] = db.suppliers.ToList();
            return View();
        }
        public ActionResult AddSup()
        {
            teethLabEntities db = new teethLabEntities();
            string name = Request.Form["name"];
            supplier sup = new supplier();
            sup.name = name;
            db.suppliers.Add(sup);
            db.SaveChanges();
            return Redirect(Url.Action("Index", "Supplier"));

        }
        public void DeleteSup()
        {
            int supId = int.Parse( Request.Form["id"]);
            teethLabEntities db = new teethLabEntities();
            db.Entry(db.suppliers.Find(supId)).State = System.Data.EntityState.Deleted;
            db.SaveChanges();
            Response.Write("success");

        }

    }
}
