using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FritidsTjanser.Controllers
{
    public class ServiceController : Controller
    {
        // GET: Service
        public ActionResult Index()
        {
            return View();
        }

        //Gets called when you search for a zipcode on the main site. Returns a partial view with all services found from the database
        [HttpGet]
        public ActionResult GetServices(Models.ServiceModel serviceModel)
        {
            Models.DALManager manager = new Models.DALManager();
            return PartialView("_SearchedServices", manager.GetServicesFromDatabase(serviceModel.ZipCode));
        }
    }
}