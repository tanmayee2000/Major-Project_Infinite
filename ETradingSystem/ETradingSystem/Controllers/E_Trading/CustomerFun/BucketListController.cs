using ETradingSystem.Models;

using System.Web.Mvc;

using System.Linq;

using System.Data.Entity;

public class BucketListController : Controller

{

    private readonly E_TradingDBEntities7 db;

    public BucketListController()

    {

        db = new E_TradingDBEntities7();

    }

    // GET: BucketList

    public ActionResult Index()

    {

        if (Session["CustomerEmail"] == null)

        {

            return RedirectToAction("Login", "CustomerValidation");

        }

        string customerEmail = Session["CustomerEmail"].ToString();

        var customer = db.Customers.FirstOrDefault(c => c.Customer_Email == customerEmail);

        if (customer != null)

        {

            var bucketListItems = db.BucketLists

                .Where(b => b.Customer_Id == customer.Customer_Id)

                .Include(b => b.Product)

                .ToList();

            ViewBag.TotalCartValue = bucketListItems.Sum(item => item.Product.Price);

            return View(bucketListItems);

        }

        else

        {

            return RedirectToAction("Login", "CustomerValidation");

        }

    }

    // GET: AddToCart

    public ActionResult AddToCart(int productId)
    {
        if (Session["CustomerEmail"] == null)
        {
            return RedirectToAction("Login", "CustomerValidation");
        }

        string customerEmail = Session["CustomerEmail"].ToString();
        var customer = db.Customers.FirstOrDefault(c => c.Customer_Email == customerEmail);

        if (customer != null)
        {
            var product = db.Products.Find(productId);

            if (product != null)
            {
                var existingItem = db.BucketLists
                    .FirstOrDefault(b => b.Customer_Id == customer.Customer_Id && b.Product_Id == product.Product_Id);

                if (existingItem != null)
                {
                    // Increment quantity if item already exists in the bucket list
                    existingItem.Quantity++;
                }
                else
                {
                    // Add a new item with quantity 1
                    var bucketListItem = new BucketList
                    {
                        Customer_Id = customer.Customer_Id,
                        Product_Id = product.Product_Id,
                        Quantity = 1 // Assuming you have a Quantity property
                    };
                    db.BucketLists.Add(bucketListItem);
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        return RedirectToAction("Index");
    }




    // POST: UpdateQuantity
    [HttpPost]
    public ActionResult UpdateQuantity(int id, int quantity)
    {
        var bucketListItem = db.BucketLists.Find(id);
        if (bucketListItem != null)
        {
            if (quantity == 0)
            {
                db.BucketLists.Remove(bucketListItem);
            }
            else if (quantity <= bucketListItem.Product.Available_Stock)
            {
                bucketListItem.Quantity = quantity;
            }
 
            db.SaveChanges();
        }
 
        return RedirectToAction("Index");
    }

    // GET: BuyNow

    public ActionResult BuyNow(int productId)

    {

        if (Session["CustomerEmail"] == null)

        {

            return RedirectToAction("Login", "CustomerValidation");

        }

        string customerEmail = Session["CustomerEmail"].ToString();

        var customer = db.Customers.FirstOrDefault(c => c.Customer_Email == customerEmail);

        if (customer != null)

        {

            var product = db.Products.Find(productId);

            if (product != null)

            {

                if (customer.Balance >= product.Price)

                {

                    // Deduct the product price from the customer's wallet balance

                    customer.Balance -= product.Price;

                    db.SaveChanges();

                    ViewBag.Message = $"Purchase successful! Your new balance is {customer.Balance:C}.";

                }

                else

                {

                    ViewBag.Message = "Insufficient balance. Please top up your wallet.";

                }

            }

        }

        return View();

    }

    // GET: RemoveFromBucketList

    public ActionResult RemoveFromBucketList(int id)

    {

        var bucketListItem = db.BucketLists.Find(id);

        return View(bucketListItem);

    }

    // POST: RemoveFromBucketList

    [HttpPost, ActionName("RemoveFromBucketList")]

    public ActionResult RemoveFromBucketListConfirmed(int id)

    {

        if (Session["CustomerEmail"] == null)

        {

            return RedirectToAction("Login", "CustomerValidation");

        }

        var item = db.BucketLists.Find(id);

        if (item != null)

        {

            db.BucketLists.Remove(item);

            db.SaveChanges();

        }

        return RedirectToAction("Index");

    }

}

