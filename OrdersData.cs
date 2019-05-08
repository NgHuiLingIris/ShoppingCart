using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using CA_ShoppingCart.DB;
using CA_ShoppingCart.Models;
using System.Diagnostics;

namespace CA_ShoppingCart.DB
{
    public class OrdersData : Data
    {
        public static List<Order> GetOrders(int Id)

        {
            List<Order> Orders = new List<Order>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string getUniqueSerialNo = @"select concat(OrderDetails.OrderId,' ',OrderDetails.ProductId) as ProductSerialNo, 
	OrderDetails.OrderId, 
	OrderDetails.ProductId, 
	Orders.OrderDate, 
	ProductDetails.Description, 
	ProductDetails.ProductName, 
	sum(OrderDetails.Quantity) as Quantity, 
	ProductDetails.Price
from OrderDetails join orders on orders.orderid = orderdetails.orderid
join ProductDetails on ProductDetails.ProductId = OrderDetails.ProductId
where orders.Id = '"+ Id + "'"+
@"group by concat(OrderDetails.OrderId,' ',OrderDetails.ProductId), 
	OrderDetails.OrderId, 
	OrderDetails.ProductId, 
	Orders.OrderDate, 
	ProductDetails.Description, 
	ProductDetails.ProductName,  
	ProductDetails.Price";
                SqlCommand cmd = new SqlCommand(getUniqueSerialNo, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Order order = new Order()
                    {
                        ProductSerialNo = (string)reader["ProductSerialNo"],
                        OrderId = (int)reader["OrderId"],
                        ProductId = (int)reader["ProductId"],
                        OrderDate = (int)reader["OrderDate"],
                        Description = (string)reader["Description"],
                        ProductName = (string)reader["ProductName"],
                        Quantity = (int)reader["Quantity"],
                        Price = (double)reader["Price"],
                    };
                    Orders.Add(order);
                }
            }
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                foreach (Order eachOrder in Orders)
                {
                    conn.Open();

                    List<string> ActivationCodeList = new List<string>();
                    string getEachActivationCode = @"select concat(ActivationCode.OrderId,' ',ActivationCode.ProductId) as ProductSerialNo, ActivationCode from ActivationCode
where ActivationCode.OrderId = '" + eachOrder.OrderId + "' and ActivationCode.ProductId = '" + eachOrder.ProductId + "'";
                    SqlCommand ACcmd = new SqlCommand(getEachActivationCode, conn);
                    SqlDataReader ACreader = ACcmd.ExecuteReader();

                    while (ACreader.Read())
                    {
                        Order order = new Order()
                        {
                            ActivationCode = (string)ACreader["ActivationCode"],
                        };
                        ActivationCodeList.Add(order.ActivationCode);
                    }
                    eachOrder.ActivationCodeList = ActivationCodeList;

                    conn.Close();
                }
            }


            return Orders;
        }

        public static int NewOrder(int Id, int OrderDate, string Coupon)
        {
            int NewOrderId = 0;
            Coupon = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"insert into orders
(OrderId,Id,OrderDate,OrderStatus,Coupon)
select count(OrderId)+1 ," + Id + "" +
@",'" + OrderDate + "','Confirmed',null from orders select count(OrderId) as NewOrderId from orders";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Order NewOrder = new Order()
                    {
                        OrderId = (int)reader["NewOrderId"]
                    };
                    NewOrderId = NewOrder.OrderId;
                }
            }
            return NewOrderId;
        }
        public static string EnterOrderDetails(List<Order> summary, int NewOrderId)
        {
           
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                foreach (Order order in summary)
                {
                    string sql = @"INSERT INTO OrderDetails" +
                                    @"(OrderId,ProductId,Quantity,Discount,Price)" +
                                    @" SELECT " +
                                    @"" + NewOrderId + "," + order.ProductId + "," + order.Quantity + ",'123',ProductDetails.Price" +
                                    @" from OrderDetails, productdetails" +
                                    @" where ProductDetails.ProductId = '" + order.ProductId + "'" +
                                    @" group by ProductDetails.price";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
                return "Order Details have been entered";
            }
        }
    }
}