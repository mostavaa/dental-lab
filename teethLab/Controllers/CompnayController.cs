using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace teethLab.Controllers
{
    public class CompanyController : Controller
    {
        //
        // GET: /Compnay/
        /// <summary>
        /// company account
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// addd , delete company
        /// </summary>
        /// <returns></returns>
        public ActionResult viewAllCompanies()
        {
            teethLabEntities db = new teethLabEntities();
            ViewData["companies"] = db.companies.ToList();
            return View();
        }
        public void DeleteComp()
        {
            int compId = int.Parse(Request.Form["id"]);
            teethLabEntities db = new teethLabEntities();
            db.Entry(db.companies.Find(compId)).State = System.Data.EntityState.Deleted;
            db.SaveChanges();
            Response.Write("success");
        }
        public ActionResult AddCom()
        {
            teethLabEntities db = new teethLabEntities();
            string name = Request.Form["name"];
            company comp = new company();
            comp.name = name;
            db.companies.Add(comp);
            db.SaveChanges();
            return Redirect(Url.Action("viewAllCompanies", "Company"));

        }
        public void deleteDof3a() {

            int Id = int.Parse(Request.Form["id"]);
            CompanyClass com = new CompanyClass();
            if (com.deleteDof3a(Id))
            {
                Response.Write("success");
            }
            else
            {
                Response.Write("fail");
            }
        }
        public void AddDof3a() {
            int productId = int.Parse(Request.Form["id"]);
            int cost = int.Parse(Request.Form["cost"]);

            DateTime date;
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime.TryParseExact(Request.Form["date"], "yyyy-MM-dd", enUS,
                        DateTimeStyles.None, out date);
            CompanyClass com = new CompanyClass();
            int productPayId = 0;
            if (com.AddDof3a(productId, cost, date , out productPayId))
            {
                Response.Write("success," + productPayId);
            }
            else
            {
                Response.Write("fail");
            }
            
        }
        public void GetCompany() {
            CompanyClass com = new CompanyClass();
          company comp =com.getCompanyByName(Request.Form["name"]);
          if (comp != null)
          {
              Response.Write("success," + comp.name + "," );
              foreach (product p in comp.products)
              {
                   
                  Response.Write(p.name+"&&"+p.price+" ج &&"+p.enterDate.ToString("yyyy-MM-dd")+"&&");

                  int currentCost = 0;
                  foreach (productPay pp in p.productPays)
                  {
                      currentCost += pp.cost;
                      Response.Write(pp.cost + " ج %%" + pp.payDate.ToString("yyyy-MM-dd")+"%%"+pp.id+"##");
                  }
              
                  Response.Write("&&" + currentCost + " ج &&" + p.notes + "&&"+p.id+ "^^");
              }
          }
          else
          {
              Response.Write("fail");
          }
        }

    }
}
