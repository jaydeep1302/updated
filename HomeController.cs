using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PJSv2_Test1_MVC.Models;
using PJSv2_Test1_MVC.Repository;

namespace PJSv2_Test1_MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            AuthorizeSessionResponse asr = new AuthorizeSessionResponse();

            var message = new AuthorizeSession()
            {
                Gateway = "PAYEEZY",
                ApiKey = "aRRMPmVZ1YcYg1TYPDTShbF7MTHyINy1",
                ApiSecret = "fcaccf88badd97b8e436035f074c5a1139dfb0528722dad7ad7c651fc5fb852",
                AuthToken = "fdoa-b5074351a146da5885d6648325b09e16b5074351a146da56",
                TransarmorToken = "NOIW",
                ZeroDollarAuth = true
            };



            var pv2Repo = new P_Repository();
            var payeezyResponse = pv2Repo.AuthorizeSession(message);
            if (payeezyResponse != null)
            {
                asr = payeezyResponse;
            }

            return View(asr);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}