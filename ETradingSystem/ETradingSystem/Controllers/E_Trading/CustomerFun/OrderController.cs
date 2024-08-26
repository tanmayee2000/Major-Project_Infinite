using System;
using System.Web.Mvc;
using ETradingSystem.Models;

namespace ETradingSystem.Controllers
{
    public class OrdersController : Controller
    {
        private readonly E_TradingDBEntities5 db = new E_TradingDBEntities5();

        [HttpPost]
        public ActionResult PlaceOrder(int Custemer_Id, int Product_Id, int Quantity, DateTime Delivery_Date, string Payment_Mode, string Address)
        {
            try
            {
                // Call stored procedure to place order
                db.PlaceOrder(Custemer_Id,Product_Id, Quantity, Delivery_Date, Payment_Mode, Address);

                // Redirect to order confirmation page
                return RedirectToAction("OrderConfirmation");
            }
            catch (Exception ex)
            {
                // Handle errors (e.g., database errors)
                ViewBag.ErrorMessage = "An error occurred while processing your order: " + ex.Message;
                return View("Error");
            }
        }

        public ActionResult OrderConfirmation()
        {
            // Assuming orderDetails is an instance of Order_Details with proper data
            Order_Details orderDetails = new Order_Details();
            // Populate orderDetails with necessary data
            return View(orderDetails);
        }

    }
}
