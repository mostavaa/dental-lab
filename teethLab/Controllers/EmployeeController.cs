﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace teethLab.Controllers
{
    public class EmployeeController : Controller
    {
        private teethLabEntities db = new teethLabEntities();

        //
        // GET: /Employee/

        public ActionResult Index()
        {
            return View(db.employees.ToList());
        }

        public ActionResult paySalary(int id)
        {
            employee emp = db.employees.FirstOrDefault(o => o.id == id);
            if (emp != null)
            {
                money money;
                int dept = emp.defaultSalary;
                foreach (var item in emp.employeeMoneys.Where(o=>o.payed==false))
                {

                    if (item.isOff)
                    {
                        dept -= (int)item.val;
                        money = new money { employee = emp, currentCredit = emp.credit, import = true, notes = "Off", value = (int)item.val, recieveDate = DateTime.Now };

                    }
                    else
                    {
                        dept += (int)item.val;
                        money = new money { employee = emp, currentCredit = emp.credit, import = false, notes = "Reward", value = (int)item.val, recieveDate = DateTime.Now };

                    }
                    item.payed = true;
                    db.Entry(item).State = EntityState.Modified;
                    db.moneys.Add(money);
                }
                foreach (var item in emp.moneys.Where(o=>o.payed==false))
                {
                    dept -= item.value;
                    item.payed = true;
                    db.Entry(item).State = EntityState.Modified;
                }
                 money = new money { employee = emp, currentCredit = emp.credit, import = false, notes = "Salary", value = dept, recieveDate = DateTime.Now };
                 db.moneys.Add(money);
                db.SaveChanges();
                return RedirectToAction("EmployeeProfile", emp);
                }
            return HttpNotFound();

            }

        
        public ActionResult EmployeeProfile(int id)
        {
            employee emp = db.employees.FirstOrDefault(o => o.id == id);
            if (emp != null)
            {
                return View(emp);
            }
            return HttpNotFound();
        }

        
        
        //
        // GET: /Employee/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Employee/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(employee employee)
        {
            if (ModelState.IsValid)
            {
                db.employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        //
        // GET: /Employee/Edit/5

        public ActionResult Edit(int id = 0)
        {
            employee employee = db.employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        //
        // POST: /Employee/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        //
        // GET: /Employee/Delete/5

        public ActionResult Delete(int id = 0)
        {
            employee employee = db.employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        //
        // POST: /Employee/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            employee employee = db.employees.Find(id);
            db.employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult offAndRewards()
        {
            return View();
        }
        public ActionResult offAndRewardsAction()
        {
            string _EMP_ID = Request.Form["employee"];
            string _VALUE = Request.Form["value"];
            string _OFF = Request.Form["off"];
            string _REWARD = Request.Form["reward"];
            bool off = false;
            if (_OFF == "on")
            {
                off = true;
                
            }
            else if (_REWARD == "on")
            {
                off = false;
            }
            if (_VALUE != null && _VALUE != string.Empty)
            {
                if (_EMP_ID != null && _EMP_ID != string.Empty)
                {
                    int empId = int.Parse(_EMP_ID);
                    employee emp = db.employees.FirstOrDefault(o => o.id == empId);
                    if (emp != null)
                    {
                        int value = int.Parse(_VALUE);
                        employeeMoney em = new employeeMoney() { employeeId = empId, isOff = off, payed = false, val = value, recieveDate = DateTime.Now };
                        db.employeeMoneys.Add(em);
                        db.SaveChanges(); 
                    }
                }
            }
            return RedirectToAction("offAndRewards");
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}