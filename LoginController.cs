using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CA_ShoppingCart.DB;
using CA_ShoppingCart.Models;
using System.Diagnostics;
using System.Data.SqlClient;

namespace CA_ShoppingCart.Controllers
{
    [HandleError]
    [HandleError(ExceptionType = typeof(SqlException), View = "DatabaseError")]
    [HandleError(ExceptionType = typeof(NullReferenceException), View = "LameErrorHandling")]

    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index(string username, string password)
        {
            if (username == null)
            {
                return View();
            }

            User user = UserData.GetUserByUsername(username);
            if (password != user.Password)
            {
                return View();
            }

            string sessionId = SessionData.CreateSession(user.Id);
            Session[sessionId + "_Orders"] = new List<Order>();

            //this is to cater to the first time login
            int productId = 0;
            int quantity = 0;

            return RedirectToAction("ViewProducts", "Products", new { sessionId = sessionId, Id = user.Id, productId = productId, quantity = quantity });
        }
    }
}
