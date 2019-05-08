using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CA_ShoppingCart.DB;
using CA_ShoppingCart.Models;
using System.Diagnostics;
using System.Web.Helpers;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace CA_ShoppingCart.Controllers
{
    //Filters
    [HandleError]
    [HandleError(ExceptionType = typeof(SqlException), View = "DatabaseError")]
    [HandleError(ExceptionType = typeof(NullReferenceException), View = "LameErrorHandling")]

    public class ProductsController : Controller
    {
        // GET: Products
        //Main: ViewProducts
        public ActionResult ViewProducts(int Id, string sessionId, int productId, int quantity)
        {
            //send to view;
            ViewData["sessionId"] = sessionId;
            ViewData["Id"] = Id;
            ViewData["productId"] = productId;
            ViewData["quantity"] = quantity;

            //To obtain user information at top partial view and send to View
            PartialViewUserProfile(sessionId);
            PartialViewCartCount();

            //only if there is productId and quantity change then update the cart
            if (productId != 0 || quantity != 0)
            {
                AddtoOrder(sessionId, productId, quantity);
                CountCartUpdate(sessionId);
            }

            //send to View
            List<Products> products = ProductsData.GetAllProducts();
            ViewData["products"] = products;


            return View();
        }
        public ActionResult PartialViewCartCount()
        {
            int CartCount = 0;
            ViewData["CartCount"] = CartCount;
            return RedirectToAction("ViewProducts");
        }
        public ActionResult PartialViewUserProfile(string sessionId)
        {
            User user = new User();
            user = UserData.GetUserBySessionId(sessionId);
            ViewData["user"] = user;
            return RedirectToAction("ViewProducts");
        }
        public ActionResult AddtoOrder(string sessionId, int productId, int quantity)
        {
            List<Order> OrderSummary = (List<Order>)Session[sessionId + "_Orders"];
            Order order = new Order
            {
                ProductId = productId,
                Quantity = quantity
            };
            OrderSummary.Add(order);
            return RedirectToAction("ViewProducts");
        }
        public ActionResult CountCartUpdate(string sessionId)
        {
            List<Order> OrderSummary = (List<Order>)Session[sessionId + "_Orders"];

            //start default from zero because count all orders
            int CartCount = 0;

            foreach (Order o in OrderSummary)
            {
                CartCount = o.Quantity + CartCount;
            }
            ViewData["CartCount"] = CartCount;
            return RedirectToAction("ViewProducts");
        }
    }
}
