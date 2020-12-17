using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AspIdentitydemo.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles="Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Accounts,Admin")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize(Roles = "Sales")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize(Roles = "Manager")]
        public ActionResult Manager()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}