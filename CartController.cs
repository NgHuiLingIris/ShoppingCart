using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CA_ShoppingCart.Models;
using CA_ShoppingCart.DB;
using System.Data.SqlClient;
namespace CA_ShoppingCart.Controllers
{
    [HandleError]
    [HandleError(ExceptionType = typeof(SqlException), View = "DatabaseError")]
    [HandleError(ExceptionType = typeof(NullReferenceException), View = "LameErrorHandling")]
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult ViewCart(int Id, string sessionId, int? productId, int? quantity)
        {
            //send to view
            ViewData["sessionId"] = sessionId;
            ViewData["Id"] = Id;

            //second view cart (after user edit cart) there is a productId
            if (productId != null)
            {
                UpdateQuantityPerProductId(sessionId, (int)productId, (int)quantity);
            }

            //Retreieve OrderSummary and CartProducts and send to view
            List<Order> OrderSummary = (List<Order>)Session[sessionId + "_Orders"];
            List<Order> cartProducts = ProductsData.GetProductsByProductId(OrderSummary);
            ViewData["cartProducts"] = cartProducts;
            ViewData["OrderSummary"] = OrderSummary;

            //to calculate total price
            RefreshPrice(cartProducts);

            return View();
        }
        public ActionResult UpdateQuantityPerProductId(string sessionId, int productId, int quantity)
        {
            List<Order> OrderSummary = (List<Order>)Session[sessionId + "_Orders"];
            foreach (Order order in OrderSummary)
            {
                if (order.ProductId == productId)
                {
                    order.Quantity = (int)quantity;
                }
            }
            return RedirectToAction("ViewCart");
        }
        public ActionResult RefreshPrice(List<Order> cartProducts)
        {
            double totalPrice = 0;
            foreach (Order order in cartProducts)
            {
                totalPrice = (order.Price * order.Quantity) + totalPrice;
            }
            ViewData["totalPrice"] = totalPrice;
            return RedirectToAction("ViewCart");
        }
    }
}
