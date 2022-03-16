using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FritidsTjanser.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        
        //TODO: Use methods from DALManager to make this method validate the user
        //This gets called whenever a new user tries to login. This method tries to make a new user right now, this should be changed
        [HttpPost]
        public ActionResult Login(Models.LoginModel user)
        {
            Models.DALManager manager = new Models.DALManager();
            manager.GenerateSalt();
            manager.StoreHashedPassword(user.Username, manager.CreateHashedPassword(user.Password));
            return View();
        }
    }
}