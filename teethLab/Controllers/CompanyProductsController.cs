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
            totalRows = db.companyProducts.Count();
            var models = db.companyProducts.OrderBy(o => o.id).Skip(offset).Take(size).ToList().Select(S => new
            {
                companyId = S.companyId,
                enterDate = S.enterDate,
                id = S.id,
                productId = S.productId,
                quantity = S.quantity,
                unitId = S.unitId,
                unitPrice = S.unitPrice
            });;
            numOfPages = totalRows / size;
            if (totalRows % size != 0)
            {
                numOfPages++;
            }
            var companies = db.companies.ToList().Select(s => new
            {
                name = s.name,
                id = s.id,
                enterDate = s.enterDate,
                credit = s.credit,
            });

            var products = db.products.Select(s => new
            {
                name = s.name,
                id = s.id,
                enterDate = s.enterDate,
                notes = s.notes,
            });
            var units = db.Units.Select(s => new
            {
                unit1 = s.unit1,
                id = s.id,
            });
            return Json(new { models = models, companies = companies, products = products, units = units, numOfPages = numOfPages }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Add(companyProduct model)
        {
            model.enterDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.companyProducts.Add(model);
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
        public JsonResult Edit(companyProduct model)
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
        public JsonResult Delete(companyProduct model)
        {
            companyProduct obj = db.companyProducts.Find(model.id);
            db.companyProducts.Remove(obj);
            db.SaveChanges();
            return Json(new { success = true });
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
            /*
            if (Request.QueryString["productid"] != null)
            {
                string _productid = Request.QueryString["productid"];
                if (_productid != null && _productid != string.Empty)
                {
                    int productid = int.Parse(_productid);
                    List<ProductUnit> model = db.ProductUnits.Where(pu => pu.productId == productid).ToList();
                    ViewBag.ProductUnits = model;

                    companyproduct.enterDate = DateTime.Now;
                    company comp = db.companies.Find(companyproduct.companyId);

                    foreach (var item in model)
                    {
                    string notes = "";
                    int price = 0;

                        if (Request.Form["unit" + item.Unit.unit1] != null && Request.Form["unit" + item.Unit.unit1] != string.Empty
                            && Request.Form["quan" + item.Unit.unit1] != null && Request.Form["quan" + item.Unit.unit1] != string.Empty
                            )
                        {
                            int unitprice = int.Parse(Request.Form["unit" + item.Unit.unit1]);
                            int quantity = int.Parse(Request.Form["quan" + item.Unit.unit1]);
                            companyProduct cp = new companyProduct { companyId = companyproduct.companyId, enterDate = DateTime.Now, productId = companyproduct.productId, quantity = quantity, unitPrice = unitprice  , unit = item.Unit.unit1};
                            db.companyProducts.Add(cp);
                            notes+="Product="+item.product.name+" ; Unit="+item.Unit.unit1+" ; Quantity="+quantity+" ; unitPrice="+unitprice+" ; ";
                            price += (quantity * unitprice);
                            money newMoney = new money { companyId = companyproduct.companyId, import = true, recieveDate = DateTime.Now, value = price, notes = notes, payed = false, currentCredit = comp.credit };
                            db.moneys.Add(newMoney);

                        }
                    }
                    
                    comp.credit++;
                    db.Entry(comp).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");                    
                }
            }
           
            */

            ViewBag.companyId = new SelectList(db.companies, "id", "name", companyproduct.companyId);
            ViewBag.productId = new SelectList(db.products, "id", "name", companyproduct.productId);
            //ViewBag.unit = new SelectList(db.Units, "unit1", "unit1",companyproduct.unit); 
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
            //ViewBag.unit = new SelectList(db.Units, "unit1", "unit1", companyproduct.unit); 

            return View(companyproduct);
        }

        //
        // POST: /CompanyProducts/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        /*
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
            //ViewBag.unit = new SelectList(db.Units, "unit1", "unit1", companyproduct.unit); 
            return View(companyproduct);
        }*/

        //
        // GET: /CompanyProducts/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    companyProduct companyproduct = db.companyProducts.Find(id);
        //    if (companyproduct == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(companyproduct);
        //}

        //
        // POST: /CompanyProducts/Delete/5

        public ActionResult DeleteConfirmed(int id)
        {
            companyProduct cp = db.companyProducts.Find(id);

            
            money newMoney = new money { companyId = cp.companyId, import = false, recieveDate = DateTime.Now, value = cp.unitPrice*cp.quantity , notes = "back", payed = false, currentCredit = cp.company.credit };
            company comp = db.companies.Find(cp.companyId);
            comp.credit--;
            db.Entry(comp).State = EntityState.Modified;
            db.moneys.Add(newMoney);

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