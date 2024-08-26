using System;

using System.Collections.Generic;

using System.Data;

using System.Data.Entity;

using System.Linq;

using System.Net;

using System.Web;

using System.Web.Mvc;

using ETradingSystem.Models;
using Microsoft.AspNet.Identity;

namespace ETradingSystem.Controllers.E_Trading.VendorFun

{


    public class CustomerProductsController : Controller

    {

        private E_TradingDBEntities5 db = new E_TradingDBEntities5();

        public ActionResult Index(decimal? id)

        {

            var products = db.Products.ToList();
            return View(products);

        }

        public ActionResult Details(int? id)

        {

            if (id == null)

            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            // Retrieve the product details based on the provided product ID

            var product = db.Products.Find(id);

            if (product == null)

            {

                return HttpNotFound();

            }

            return View(product);

        }

        public ActionResult Buy(Decimal id)

        {

            if (id == null)

            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            // Retrieve the product details based on the provided product ID

            var product = db.Products.Find(id);

            if (product == null)

            {

                return HttpNotFound();

            }

            // Add the product to the bucket list

            // Assuming you have a BucketList model and a BucketListController

            var bucketList = new BucketList { Product_Id = id, Quantity = 1 };

            db.BucketLists.Add(bucketList);

            db.SaveChanges();

            return RedirectToAction("Index", "BucketList");

        }

        public ActionResult AddToCart(Decimal id)

        {

            if (id == null)

            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            // Retrieve the product details based on the provided product ID

            var product = db.Products.Find(id);

            if (product == null)

            {

                return HttpNotFound();

            }

            // Add the product to the bucket list

            // Assuming you have a BucketList model and a BucketListController

            var bucketList = new BucketList { Product_Id = id, Quantity = 1 };

            db.BucketLists.Add(bucketList);

            db.SaveChanges();

            TempData["Message"] = "Product successfully added to cart!";
            return RedirectToAction("Index");

        }

        public ActionResult ViewBucketList()

        {

            // Redirect to the BucketListController

            return RedirectToAction("Index", "BucketList");

        }

        
    }

}
