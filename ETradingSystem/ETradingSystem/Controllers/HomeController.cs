using ETradingSystem.Models;

using System;

using System.Collections.Generic;

using System.Data.SqlClient;

using System.Linq;

using System.Web;

using System.Web.Mvc;

namespace ETradingSystem.Controllers

{

    public class HomeController : Controller

    {

        public ActionResult Index()

        {

            ViewBag.Title = "Home Page";

            return View();

        }

        public ActionResult Help()

        {

            ViewBag.Message = "Help page";

            return View();

        }

        public ActionResult Welcome()

        {

            ViewBag.Message = "Welcome page";

            return View();

        }

        public ActionResult Login()

        {

            ViewBag.Message = "Login page";

            return View();

        }

        public ActionResult AboutUs()

        {

            ViewBag.Message = "About Us page";

            return View();

        }

        public ActionResult BucketList()

        {

            if (Session["CustomerEmail"] == null)

            {

                return RedirectToAction("Login", "CustomerValidation");

            }

            return RedirectToAction("Index", "BucketList");

        }
        public ActionResult Search(string searchTerm)

        {

            List<Product> products = new List<Product>();

            string connectionString = "Server=ICS-LT-7X2B693\\SQLEXPRESS;Database=E_TradingDB;user id=sa;password=Sky@9531";

            string query = "SELECT * FROM Products WHERE Product_Name LIKE '%' + @SearchTerm + '%'";

            using (SqlConnection connection = new SqlConnection(connectionString))

            {

                using (SqlCommand command = new SqlCommand(query, connection))

                {

                    command.Parameters.AddWithValue("@SearchTerm", searchTerm);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())

                    {

                        Product product = new Product

                        {

                            Product_Id = reader.GetDecimal(0), // Assuming Product_Id is a decimal in your database

                            Product_Name = reader.GetString(1),

                            Brand = reader.GetString(2),

                            Color = reader.GetString(3),

                            Available_Stock = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),

                            Price = reader.IsDBNull(5) ? (double?)null : Convert.ToDouble(reader.GetDecimal(5)), // Handle price conversion

                            Status = reader.GetString(6),

                            ImageFileName = reader.GetString(7)

                        };

                        products.Add(product);

                    }

                }

            }

            return View(products);

        }





        public ActionResult Profile()

        {

            if (Session["CustomerEmail"] == null)

            {

                return RedirectToAction("Login", "CustomerValidation");

            }

            string customerEmail = Session["CustomerEmail"].ToString();

            Customer customer = null;

            string connectionString = "Server=ICS-LT-7X2B693\\SQLEXPRESS;Database=E_TradingDB;user id=sa;password=Sky@9531";

            string query = "SELECT * FROM Customer WHERE Customer_Email = @Email";

            using (SqlConnection connection = new SqlConnection(connectionString))

            {

                using (SqlCommand command = new SqlCommand(query, connection))

                {

                    command.Parameters.AddWithValue("@Email", customerEmail);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())

                    {

                        customer = new Customer

                        {

                            Customer_Id = reader.GetDecimal(0),

                            Customer_Name = reader.GetString(1),

                            Customer_Email = reader.GetString(2),

                            Date_Of_Birth = reader.GetDateTime(3),

                            Address = reader.GetString(4),

                            Balance = reader.IsDBNull(5) ? (double?)null : reader.GetDouble(5),

                            Mobile_Number = reader.IsDBNull(6) ? (decimal?)null : reader.GetDecimal(6),

                            Password = reader.GetString(7),

                            Hint_Id = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),

                            Hint_Answer = reader.GetString(9),

                            Status = reader.GetString(10)

                        };

                    }

                }

            }

            return View(customer);

        }

        // GET: Home/EditProfile
        public ActionResult EditProfile()
        {
            if (Session["CustomerEmail"] == null)
            {
                return RedirectToAction("Login", "CustomerValidation");
            }

            string customerEmail = Session["CustomerEmail"].ToString();
            Customer customer = null;

            string connectionString = "Server=ICS-LT-7X2B693\\SQLEXPRESS;Database=E_TradingDB;user id=sa;password=Sky@9531";
            string query = "SELECT * FROM Customer WHERE Customer_Email = @Email";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", customerEmail);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        customer = new Customer
                        {
                            Customer_Id = reader.GetDecimal(0),
                            Customer_Name = reader.GetString(1),
                            Customer_Email = reader.GetString(2),
                            Date_Of_Birth = reader.GetDateTime(3),
                            Address = reader.GetString(4),
                            Balance = reader.IsDBNull(5) ? (double?)null : reader.GetDouble(5),
                            Mobile_Number = reader.IsDBNull(6) ? (decimal?)null : reader.GetDecimal(6),
                            Password = reader.GetString(7),
                            Hint_Id = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                            Hint_Answer = reader.GetString(9),
                            Status = reader.GetString(10)
                        };
                    }
                }
            }

            return View(customer);
        }

        // POST: Home/EditProfile
        [HttpPost]
        public ActionResult EditProfile(Customer model)
        {
            if (Session["CustomerEmail"] == null)
            {
                return RedirectToAction("Login", "CustomerValidation");
            }

            string connectionString = "Server=ICS-LT-7X2B693\\SQLEXPRESS;Database=E_TradingDB;user id=sa;password=Sky@9531";
            string query = "UPDATE Customer SET Customer_Name = @Name, Address = @Address, Mobile_Number = @Phone WHERE Customer_Email = @Email";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", model.Customer_Name);
                    command.Parameters.AddWithValue("@Address", model.Address);
                    command.Parameters.AddWithValue("@Phone", model.Mobile_Number);
                    command.Parameters.AddWithValue("@Email", model.Customer_Email);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Profile");
        }

        public ActionResult Wallet()

        {

            if (Session["CustomerEmail"] == null)

            {

                return RedirectToAction("Login", "CustomerValidation");

            }

            string customerEmail = Session["CustomerEmail"].ToString();

            double balance = 0;

            string connectionString = "Server=ICS-LT-7X2B693\\SQLEXPRESS;Database=E_TradingDB;user id=sa;password=Sky@9531";

            string query = "SELECT Balance FROM Customer WHERE Customer_Email = @Email";

            using (SqlConnection connection = new SqlConnection(connectionString))

            {

                using (SqlCommand command = new SqlCommand(query, connection))

                {

                    command.Parameters.AddWithValue("@Email", customerEmail);

                    connection.Open();

                    balance = (double)command.ExecuteScalar();

                }

            }

            ViewBag.Balance = balance;

            return View();

        }

    }

}

