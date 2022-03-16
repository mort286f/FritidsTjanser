using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FritidsTjanser.Controllers
{
    public class HomeController : Controller
    {
        //Returns the front page
        public ActionResult Index()
        {
            return View();
        }
    }
}