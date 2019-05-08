using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CA_ShoppingCart.Controllers
{
    public class ViewController : Controller
    {
        // GET: View
        public ActionResult Product()
        {
            return View();
        }
        public ActionResult Price()
        {
            return View();
        }
        public ActionResult Checkout()
        {
            return View();
        }
        public ActionResult Main()
        {
            return View();
        }

        public ActionResult ViewCart()
        {


            return View();

        }
    }
}