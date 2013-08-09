using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LisaKatherine.Models;

namespace LisaKatherine.Controllers
{
    public class AdminController : Controller
    {

        [Authorize]
        public ActionResult Index()
        {
                return View();
        }

       
    }
}
