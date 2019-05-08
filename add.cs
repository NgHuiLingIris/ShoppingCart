using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CA_ShoppingCart.Util
{
    public class Add
    {
        public static string getCode()
        {
            string activationCode = Guid.NewGuid().ToString();
            return activationCode;
        }
    }
}