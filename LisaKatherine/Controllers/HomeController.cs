namespace LisaKatherine.Controllers
{
    using System.Web.Mvc;

    using LisaKatherine.Interface;
    using LisaKatherine.Services;

    public class HomeController : Controller
    {
        private readonly PublishedArticleService publishedArticleService = new PublishedArticleService();

        public ActionResult Index()
        {
            IPublishedArticle article = this.publishedArticleService.GetArticleByArticleType(2);
            if (article != null)
            {
                this.ViewBag.Headline = article.Headline;
                this.ViewBag.Strapline = article.Strapline;
                this.ViewBag.Body = article.Body;
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
            IPublishedArticle article = this.publishedArticleService.GetArticleByArticleType(5);
            this.ViewBag.ShowPartial = "Twitter";
            this.ViewBag.Message = "Contact";
            var cmv = new ContactArticle { PublishedArticle = (PublishedArticle) article, Contact = new Contact()};
            return View();
        }

        [HttpPost]
        public ActionResult Contact(Contact contactArticle)
        {
            IPublishedArticle article = this.publishedArticleService.GetArticleByArticleType(5);
            this.ViewBag.ShowPartial = "Twitter";
            this.ViewBag.Message = "Contact";
            if (!this.ModelState.IsValid)
            {
                return View(contactArticle);
            }

            var contact = new Contact
                              {
                                  From = contactArticle.From,
                                  Subject = contactArticle.Subject,
                                  Message = contactArticle.Message
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

        public PartialViewResult RenderContactArticle()
        {
            IPublishedArticle article = this.publishedArticleService.GetArticleByArticleType(5);
            return this.PartialView("_Article", article);
        }
    }
}