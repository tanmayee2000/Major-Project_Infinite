using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using ETradingSystem.Models;
using ETradingSystem.Controllers;
using System.Web.Mvc;

namespace UnitTesting
{
    [TestFixture]
    public class AdminLoginTest
    {
        [Test]
        public void Login_ValidAdmin()
        {
            // Arrange
            var controller = new AdminLoginTest();
            var email = "raghu@gmail.com";
            var password = "password";

            // Act
            var result = controller.Login(email, password) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Dashboard", result.RouteValues["controller"]);
        }

        [Test]
        public void Login_InvalidAdmin()
        {
            // Arrange
            var controller = new AdminLoginTest();
            var email = "raghu@gmail.com";
            var password = "wrongpassword";

            // Act
            var result = controller.Login(email, password) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Login", result.ViewName);
            Assert.AreEqual("Invalid Admin Email or Password.", result.ViewBag.InvalidLogin);
        }
    }
}
