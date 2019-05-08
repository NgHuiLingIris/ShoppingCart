using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace CA_ShoppingCart.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Please enter your username")]
        public string username
        {
            get; set;
        }

        [Required(ErrorMessage = "Please enter your password")]
        public string password
        {
            get; set;
        }
    }
}