using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CA_ShoppingCart.DB;
using CA_ShoppingCart.Models;
using CA_ShoppingCart.Util;
using System.Data.SqlClient;
namespace CA_ShoppingCart.Controllers
{
    [HandleError]
    [HandleError(ExceptionType = typeof(SqlException), View = "DatabaseError")]
    [HandleError(ExceptionType = typeof(NullReferenceException), View = "LameErrorHandling")]
    public class CheckoutController : Controller
    {
        // GET: Checkout

        //From Products View -> Redirect to checkout controller -> Purchase History View
        public ActionResult Checkout(int Id, string sessionId)
        {
            int date = GetTodayDate();
            int NewOrderId = OrdersData.NewOrder(Id, date, "NULL");

            List<Order> OrderSummary = (List<Order>)Session[sessionId + "_Orders"];
            string OrdersConfirmation = OrdersData.EnterOrderDetails(OrderSummary, NewOrderId);

            return RedirectToAction("ActivationCode", "Checkout", new { NewOrderId = NewOrderId, Id = Id, sessionId = sessionId });
        }
        public int GetTodayDate()
        {
            DateTime now = DateTime.Now;
            int date = (int)Timestamp.unixTimestamp();
            return date;
        }
        public ActionResult ActivationCode(int NewOrderId, int Id, string sessionId) //enter activation code in each product
        {
            //define the variable needed first
            string activationCode = "";

            List<Order> OrderSummary = (List<Order>)Session[sessionId + "_Orders"];
            foreach (Order order in OrderSummary)
            {
                for (int i = 0; i < order.Quantity; i++)
                {
                    activationCode = Add.getCode();
                    string message = ProductsData.EnterActivationCode(NewOrderId, order.ProductId, activationCode);
                }
            }

            return RedirectToAction("ViewPurchaseHistory", "Checkout", new { NewOrderId = NewOrderId, Id = Id, sessionId = sessionId });
        }
        public ActionResult ViewPurchaseHistory(int? NewOrderId, int Id, string sessionId)
        {
            List<Order> Orders = OrdersData.GetOrders(Id);

            List<Order> OrderSummary = (List<Order>)Session[sessionId + "_Orders"];
            OrderSummary.Clear();

            //send back to view
            ViewData["PurchaseHistory"] = Orders;
            ViewData["sessionId"] = sessionId;
            ViewData["Id"] = Id;

            //Return View Purchase History View
            return View();
        }
    }
}
