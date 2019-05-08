using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using CA_ShoppingCart.DB;
using CA_ShoppingCart.Models;

namespace CA_ShoppingCart.DB
{
    public class ProductsData : Data
    {

        public static List<Products> QueryProducts(string keyword)
        {
            List<Products> Products = new List<Products>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"select ProductId,ProductName,Price,Description from ProductDetails
                WHERE ProductName LIKE '%"+keyword+"%' or Description LIKE '%"+keyword+"%'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Products Product = new Products()
                    {
                        Id = (int)reader["ProductId"],
                        Name = (string)reader["ProductName"],
                        Price = (double)reader["Price"],
                        Description = (string)reader["Description"],
                    };
                    Products.Add(Product);
                }
            }
            return Products;
        }

        public static List<Order> GetProductsByProductId(List<Order> ProductIDs)
        {
            List<Order> orders = new List<Order>();
           
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
              
                foreach (var order in ProductIDs)
                {
                    conn.Open();
                    string sql1 = @"SELECT ProductId, ProductName, Price, Description FROM ProductDetails WHERE ProductId = '" + order.ProductId + "'";
                    SqlCommand cmd = new SqlCommand(sql1, conn);
                    SqlDataReader sdr = cmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        Order O = new Order
                        {
                            ProductName = (string)sdr["ProductName"],
                            Price = (double)sdr["Price"],
                            ProductId = (int)sdr["ProductId"],
                            Description = (string)sdr["Description"],
                            Quantity = order.Quantity
                        };
                        orders.Add(O);
                    }
                    conn.Close();
                }
                return orders;
            }

        }

        public static List<Products> GetAllProducts()
        {
            List<Products> products = new List<Products>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT ProductId, ProductName, Price, Description FROM ProductDetails;";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    Products product = new Products()
                    {
                        Id = (int)sdr["ProductId"],
                        Name = (string)sdr["ProductName"],
                        Price = (double)sdr["Price"],
                        Description = (string)sdr["Description"],
                    };
                    products.Add(product);
                }
                return products;
            }
        }

        public static string EnterActivationCode(int NewOrderId, int ProductId, string ActivationCode)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO ActivationCode
                                (OrderId,ProductId,ActivationCode)
                                VALUES ('"+NewOrderId+"','" +ProductId +"','" +ActivationCode+"')";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            return "Activation Code is entered";
        }

    }
}
