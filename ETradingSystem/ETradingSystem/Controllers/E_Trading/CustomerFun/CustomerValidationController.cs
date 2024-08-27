using System;

using System.Linq;

using System.Web;

using System.Web.Mvc;

using System.Web.Security;

using ETradingSystem.Models;

namespace ETradingSystem.Controllers.E_Trading.CustomerFun

{

    public class CustomerValidationController : Controller

    {

        private readonly E_TradingDBEntities7 db;

        public CustomerValidationController()

        {

            db = new E_TradingDBEntities7();

        }

        public string Cust_email { get; private set; }

        // GET: Customer/Login

        public ActionResult Login()

        {

            return View();

        }

        [HttpPost]

        public ActionResult Login(string email, string password)

{

    // Retrieve the customer based on email and password

    var customer = GetCustomerByEmailAndPassword(email, password);
 
    if (customer != null)

    {

        // Check if the customer's account is active

        if (customer.Status == "Active")

        {

            // Store customer email in session

            Session["CustomerEmail"] = email;
 
            // Set authentication cookie

            FormsAuthentication.SetAuthCookie(email, false); // false indicates that the cookie is not persistent
 
            // Check if there's a return URL stored in TempData

            var returnUrl = TempData["ReturnUrl"] as string;
 
            if (!string.IsNullOrEmpty(returnUrl))

            {

                return Redirect(returnUrl);

            }
 
            return RedirectToAction("Index", "CustomerProducts");

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

        ViewBag.InvalidLogin = "Invalid Customer Email or Password.";

        return View();

    }

}
 
// Helper method to retrieve customer based on email and password

private Customer GetCustomerByEmailAndPassword(string email, string password)

{

    return db.Customers.FirstOrDefault(c => c.Customer_Email == email && c.Password == password);

}

 

        // GET: Customer/Register

        public ActionResult Register()

        {

            ViewBag.HintList = db.Hints.ToList();

            return View();

        }

        [HttpPost]

        public ActionResult Register(Customer customer)

        {

            if (ModelState.IsValid)

            {

                if (IsEmailAvailable(customer.Customer_Email))

                {

                    db.Customers.Add(customer);

                    db.SaveChanges();

                    return RedirectToAction("Login", "CustomerValidation");

                }

                else

                {

                    ViewBag.Error = "Email already exists.";

                    ViewBag.HintList = db.Hints.ToList();

                    return View();

                }

            }

            else

            {

                ViewBag.HintList = db.Hints.ToList();

                return View();

            }

        }

        private bool IsEmailAvailable(string email)

        {

            return !db.Customers.Any(x => x.Customer_Email == email);

        }

        private bool IsValidCustomer(string email, string password)

        {

            string Email = db.Customers.Where(x => x.Customer_Email == email).Select(x => x.Customer_Email).FirstOrDefault();

            string Password = db.Customers.Where(x => x.Customer_Email == email).Select(x => x.Password).FirstOrDefault();

            return email == Email && password == Password;

        }

    }

}

