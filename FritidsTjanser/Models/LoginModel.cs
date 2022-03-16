using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FritidsTjanser.Models
{
    //Login model that matches the table in Users table in the database
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

    }
}