using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ETradingSystem.Models;

namespace ETradingSystem.Controllers.E_Trading.Admin
{
    public class AdminVendorsController : Controller
    {
        private E_TradingDBEntities7 db = new E_TradingDBEntities7();

        public ActionResult Index()
        {
            var vendors = db.Vendors.Include(v => v.Hint);
            return View(vendors.ToList());
        }
        public ActionResult GetVendorsByVendorName(string vendorName)
        {
            var vendors = db.Vendors.Where(v => v.Vendor_Name == vendorName).ToList();

            return View("Index", vendors);
        }
        public ActionResult Details(decimal? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }


        public ActionResult Edit(decimal? id)

        {

            if (id == null)

            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            Vendor vendor = db.Vendors.Find(id);

            if (vendor == null)

            {

                return HttpNotFound();

            }

            return View(vendor);

        }

        [HttpPost]

        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include = "Vendor_Id,Category")] Vendor vendor)

        {

            if (ModelState.IsValid)

            {

                try

                {

                    // Fetch the existing vendor from the database

                    var existingVendor = db.Vendors.Find(vendor.Vendor_Id);

                    if (existingVendor == null)

                    {

                        return HttpNotFound();

                    }

                    // Update only the Category field

                    existingVendor.Category = vendor.Category;

                    // Save changes to the database

                    db.SaveChanges();

                    return RedirectToAction("Index");

                }

                catch (DbEntityValidationException ex)

                {

                    // Log the detailed validation errors

                    foreach (var validationErrors in ex.EntityValidationErrors)

                    {

                        foreach (var validationError in validationErrors.ValidationErrors)

                        {

                            Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);

                        }

                    }

                    throw;

                }

            }

            return View(vendor);

        }






        public ActionResult Delete(decimal? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Vendor vendor = db.Vendors.Find(id);
            if (vendor != null)
            {
                if (vendor.Status == "Active")
                {
                    vendor.Status = "InActive";
                    db.SaveChanges();
                }
                else
                {
                    vendor.Status = "Active";
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
