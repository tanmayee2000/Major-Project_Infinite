using ETradingSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace ETradingSystem.Controllers.E_Trading.CustomerFun
{
    public class BalanceController : Controller
    {
        private readonly E_TradingDBEntities5 db;

        public BalanceController()

        {

            db = new E_TradingDBEntities5();

        }


        // GET: Balance

        public ActionResult Index()

        {

            return View(db.Customers.ToList());

        }


        // GET: Balance/Details/5

        public ActionResult Details(int? id)

        {

            if (id == null)

            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            Customer customer = db.Customers.Find(id);

            if (customer == null)

            {

                return HttpNotFound();

            }

            return View(customer);

        }

        //[Authorize(Roles = "Customer")]
        public ActionResult MyBalance()
        {
            var customerId = User.Identity.GetUserId();
            var customer = db.Customers.Find(customerId);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer.Balance);
        }

        // GET: Balance/Edit/5

        public ActionResult Edit(int? id)

        {

            if (id == null)

            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            Customer customer = db.Customers.Find(id);

            if (customer == null)

            {

                return HttpNotFound();

            }

            return View(customer);

        }


        // POST: Balance/Edit/5

        [HttpPost]

        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include = "Customer_Id,Balance")] Customer customer)

        {

            if (ModelState.IsValid)

            {

                db.Entry(customer).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index");

            }

            return View(customer);

        }

    
}
}