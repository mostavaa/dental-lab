using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace teethLab
{
    public class CompanyClass : company
    {
        teethLabEntities db;
        string validationErrors;

        public string ValidationErrors
        {
            get { return validationErrors; }
            set { validationErrors = value; }
        }
        public CompanyClass() { 
        
        }
        public bool deleteDof3a(int id) {
            db = new teethLabEntities();
                db.Entry(db.productPays.Where(o => o.id == id).First()).State = System.Data.EntityState.Deleted;
                db.SaveChanges();
            return true;
        }
        public bool AddDof3a( int productId ,int cost ,DateTime date  , out int productPayId) {
            db = new teethLabEntities();
            productPay pp  = new productPay();
            pp.cost = cost;
            pp.productId = productId;
            pp.payDate = date;
            db.productPays.Add(pp);
            db.SaveChanges();
            productPayId = pp.id;
            return true;
        }
        public company getCompanyByName(string name){
            name = tools.trimMoreThanOneSpace(name);

            db = new teethLabEntities();
            if (db.companies.Where(o => o.name.Contains(name)).Count() > 0)
                return db.companies.Where(o => o.name.Contains(name)).First();
            else
                return null;
        }
    }
}