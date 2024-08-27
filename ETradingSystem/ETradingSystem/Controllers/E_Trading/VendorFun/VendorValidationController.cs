using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETradingSystem.Models;
namespace ETradingSystem.Controllers.E_Trading.VendorFun
{
    public class VendorValidationController : Controller
    {
        public decimal Vendor_ID;
        private readonly E_TradingDBEntities7 db;
        public VendorValidationController()
        {
            db = new E_TradingDBEntities7();
        }
        // GET: Vendor
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Login(string email, string password)

        {

            var vendor = GetVendorByEmailAndPassword(email, password);

            if (vendor != null)

            {

                // Check if the vendor's account is active

                if (vendor.Status == "Active")

                {

                    Vendor_ID = vendor.Vendor_Id;

                    return RedirectToAction("Index/" + Vendor_ID, "Products");

                }

                else

                {

                    // Account is deactivated, show an appropriate message

                    ViewBag.InvalidLogin = "Your account has been deactivated. Please contact the admin.";

                    return View();

                }

            }

            else

            {

                ViewBag.InvalidLogin = "Invalid Admin Email or Password.";

                return View();

            }

        }

        // Helper method to retrieve vendor based on email and password

        private Vendor GetVendorByEmailAndPassword(string email, string password)

        {

            return db.Vendors.FirstOrDefault(v => v.Vendor_Email == email && v.Passowrd == password);

        }
        public ActionResult VendorRegister()

        {

            ViewBag.HintList = new SelectList(db.Hints, "Hint_Id", "Question");

            return View();

        }

        [HttpPost]

        [ValidateAntiForgeryToken]

        public ActionResult VendorRegister(Vendor vendor)

        {

            if (ModelState.IsValid)

            {

                if (IsVendorEmailAvailable(vendor.Vendor_Email))

                {

                    vendor.Status = "Active"; // Assuming new vendors are active by default

                    db.Vendors.Add(vendor);

                    db.SaveChanges();

                    return RedirectToAction("Login", "VendorValidation");

                }

                else

                {

                    ViewBag.Error = "Email already exists.";

                    ViewBag.HintList = new SelectList(db.Hints, "Hint_Id", "Hint_Question");

                    return View(vendor);

                }

            }

            else

            {

                ViewBag.HintList = new SelectList(db.Hints, "Hint_Id", "Hint_Question");

                return View(vendor);

            }

        }

        // Helper method to check if the email is available

        private bool IsVendorEmailAvailable(string email)

        {

            return !db.Vendors.Any(v => v.Vendor_Email == email);

        }


    }
}