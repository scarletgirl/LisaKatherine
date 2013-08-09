namespace LisaKatherine.Controllers
{
    using System.Web.Mvc;

    using LisaKatherine.Models;
    using LisaKatherine.Models.Extensions;

    public class HomeController : Controller
    {
        private readonly PublishedArticleService publishedArticleService = new PublishedArticleService();

        public ActionResult Index()
        {
            PublishedArticles article = this.publishedArticleService.GetArticleByArticleType(2);
            if (article != null)
            {
                this.ViewBag.headline = article.headline;
                this.ViewBag.strapline = article.strapline;
                this.ViewBag.body = article.body;
            }
            this.ViewBag.ShowPartial = "Twitter";

            return this.View();
        }

        public ActionResult About()
        {
            this.ViewBag.ShowPartial = "Twitter";
            this.ViewBag.Message = "About Me";
            return this.View();
        }

        public ActionResult Contact()
        {
            PublishedArticles article = this.publishedArticleService.GetArticleByArticleType(5);
            this.ViewBag.ShowPartial = "Twitter";
            this.ViewBag.Message = "Contact";
            var cmv = new ContactViewModel();
            cmv.article = article;
            return View(cmv);
        }

        [HttpPost]
        public ActionResult Contact(ContactViewModel contactVM)
        {
            PublishedArticles article = this.publishedArticleService.GetArticleByArticleType(5);
            this.ViewBag.ShowPartial = "Twitter";
            this.ViewBag.Message = "Contact";
            contactVM.article = article;
            if (!this.ModelState.IsValid)
            {
                return View(contactVM);
            }

            var contact = new Contact
                              {
                                  From = contactVM.From,
                                  Subject = contactVM.Subject,
                                  Message = contactVM.Message
                              };

            new Email().Send(contact);

            return this.RedirectToAction("ContactConfirm");
        }

        public ActionResult ContactConfirm()
        {
            return this.View();
        }

        public ActionResult Holding()
        {
            return this.View();
        }
    }
}