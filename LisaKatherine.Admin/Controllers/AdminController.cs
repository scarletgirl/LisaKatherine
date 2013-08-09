namespace LisaKatherine.Admin.Controllers
{
    using System.Web.Mvc;

    public class AdminController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return this.View();
        }
    }
}