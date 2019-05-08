using CA_ShoppingCart.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Data.SqlClient;

namespace CA_ShoppingCart.Controllers
{
    [HandleError]
    [HandleError(ExceptionType = typeof(SqlException), View = "DatabaseError")]
    [HandleError(ExceptionType = typeof(NullReferenceException), View = "LameErrorHandling")]
    public class LogoutController : Controller
    {
        // GET: Logout
        public ActionResult Index(string sessionId)
        {
            SessionData.RemoveSession(sessionId);
            return RedirectToAction("Index", "Login");
        }
    }
}