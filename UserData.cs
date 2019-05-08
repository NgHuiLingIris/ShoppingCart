using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using CA_ShoppingCart.Models;

namespace CA_ShoppingCart.DB
{
    public class UserData : Data
    {
        public static User GetUserByUsername(string username)
        {
            User User = null;
            

            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();

                string sql = @"SELECT Id, Username, Password, FirstName, LastName from Customer
                    WHERE Username = '" + username + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    User = new User()
                    {
                        Id = (int)reader["Id"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"],
                        Username = (string)reader["Username"],
                        Password = (string)reader["Password"]
                    };
                }
            }
            return User;
        }

        public static User GetUserBySessionId(string sessionId)
        {
            User User = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string q = @"SELECT Customer.Id, Customer.Firstname, 
                    Customer.Lastname FROM
                        Customer, CustomerDetails
                            WHERE Customer.Id = CustomerDetails.Id
                                AND Customer.SessionId = '" + sessionId + "'";

                SqlCommand cmd = new SqlCommand(q, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    User = new User()
                    {
                        Id = (int)reader["Id"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"]
                       
                    };
                }
            }

            return User;
        }
    }
}
     