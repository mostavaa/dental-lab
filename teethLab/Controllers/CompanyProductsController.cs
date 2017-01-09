using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace teethLab.Controllers
{
    public class CompanyProductsController : Controller
    {
        private teethLabEntities db = new teethLabEntities();

        //
        // GET: /CompanyProducts/

        public ActionResult Index()
        {
            var companyproducts = db.companyProducts.Include(c => c.company).Include(c => c.product);
            return View(companyproducts.ToList());
        }

        

        //
        // GET: /CompanyProducts/Create

        public ActionResult Create()
        {
            if (Request.QueryString["productid"] != null)
            {
                string _productid = Request.QueryString["productid"];
                if (_productid != null && _productid != string.Empty)
                {
                    int productid = int.Parse(_productid);
                    List<ProductUnit> model = db.ProductUnits.Where(pu => pu.productId == productid).ToList();
                    ViewBag.ProductUnits = model;                    
                }
            }

            
            ViewBag.companyId = new SelectList(db.companies, "id", "name");
            ViewBag.productId = new SelectList(db.products, "id", "name");
    // empty untill select product
            ViewBag.unit = new SelectList(db.Units, "unit1", "unit1"); 
            return View();
        }

        //
        // POST: /CompanyProducts/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(companyProduct companyproduct)
        {
            if (Request.QueryString["productid"] != null)
            {
                string _productid = Request.QueryString["productid"];
                if (_productid != null && _productid != string.Empty)
                {
                    int productid = int.Parse(_productid);
                    List<ProductUnit> model = db.ProductUnits.Where(pu => pu.productId == productid).ToList();
                    ViewBag.ProductUnits = model;

                    companyproduct.enterDate = DateTime.Now;
                    foreach (var item in model)
                    {
                        if (Request.Form["unit" + item.Unit.unit1] != null && Request.Form["unit" + item.Unit.unit1] != string.Empty
                            && Request.Form["quan" + item.Unit.unit1] != null && Request.Form["quan" + item.Unit.unit1] != string.Empty
                            )
                        {
                            int unitprice = int.Parse(Request.Form["unit" + item.Unit.unit1]);
                            int quantity = int.Parse(Request.Form["quan" + item.Unit.unit1]);
                            companyProduct cp = new companyProduct { companyId = companyproduct.companyId, enterDate = DateTime.Now, productId = companyproduct.productId, quantity = quantity, unitPrice = unitprice  , unit = item.Unit.unit1};
                            db.companyProducts.Add(cp);

                        }

                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");                    
                }
            }
           


            ViewBag.companyId = new SelectList(db.companies, "id", "name", companyproduct.companyId);
            ViewBag.productId = new SelectList(db.products, "id", "name", companyproduct.productId);
            ViewBag.unit = new SelectList(db.Units, "unit1", "unit1",companyproduct.unit); 
            return View(companyproduct);
        }

        //
        // GET: /CompanyProducts/Edit/5

        public ActionResult Edit(int id = 0)
        {
            companyProduct companyproduct = db.companyProducts.Find(id);
            if (companyproduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.companyId = new SelectList(db.companies, "id", "name", companyproduct.companyId);
            ViewBag.productId = new SelectList(db.products, "id", "name", companyproduct.productId);
            ViewBag.unit = new SelectList(db.Units, "unit1", "unit1", companyproduct.unit); 

            return View(companyproduct);
        }

        //
        // POST: /CompanyProducts/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(companyProduct companyproduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(companyproduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.companyId = new SelectList(db.companies, "id", "name", companyproduct.companyId);
            ViewBag.productId = new SelectList(db.products, "id", "name", companyproduct.productId);
            ViewBag.unit = new SelectList(db.Units, "unit1", "unit1", companyproduct.unit); 
            return View(companyproduct);
        }

        //
        // GET: /CompanyProducts/Delete/5

        public ActionResult Delete(int id = 0)
        {
            companyProduct companyproduct = db.companyProducts.Find(id);
            if (companyproduct == null)
            {
                return HttpNotFound();
            }
            return View(companyproduct);
        }

        //
        // POST: /CompanyProducts/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            companyProduct companyproduct = db.companyProducts.Find(id);
            db.companyProducts.Remove(companyproduct);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult GetProductUnits(int productid)
        {
            List<ProductUnit> model = db.ProductUnits.Where(pu => pu.productId == productid).ToList();
          /*  List<Unit> unitlist = new List<Unit>();
            foreach (var item in model)
            {
                unitlist.Add(item.Unit);
            }
            ViewBag.unit = new SelectList(unitlist, "unit1", "unit1");*/
             ViewBag.ProductUnits= model;
            return PartialView("ProductUnits");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}