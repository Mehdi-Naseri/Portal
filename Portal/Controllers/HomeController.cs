using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Who am I?";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact me if you need any help.";

            return View();
        }
    }
}