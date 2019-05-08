using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CA_ShoppingCart.Models
{
    public class Order
    {
        public string ProductName
        {
            get; set;
        }
        public string Description
        {
            get; set;
        }
        public double Price
        {
            get; set;
        }
        public int OrderId
        {
            get; set;
        }
        public string ActivationCode
        {
            get; set;
        }
        public int OrderDate
        {
            get; set;
        }
        public string OrderStatus
        {
            get; set;
        }
        public int Quantity
        {
            get; set;
        }
        public int ProductId
        {
            get;set;
        }
        public string ProductSerialNo
        {
            get; set;
        }

        public List<string> ActivationCodeList
        {
            get; set;
        }
    }
}