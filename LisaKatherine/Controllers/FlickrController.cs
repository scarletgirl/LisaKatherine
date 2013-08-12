namespace LisaKatherine.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using LisaKatherine.Interface;
    using LisaKatherine.Services;

    public class FlickrController : Controller
    {
        private const string FlickrPhotostreamUrl =
            "http://api.flickr.com/services/feeds/photos_public.gne?id=48225317@N08&lang=en-us&format=rss_200";

        private readonly PhotoService photoService = new PhotoService();

        public PartialViewResult FlickrPhotostream()
        {
            IEnumerable<IPhoto> flickrImageList = this.photoService.GetNumberOfImages(3, FlickrPhotostreamUrl);
            return this.PartialView("_Flickr", flickrImageList);
        }

        [Authorize]
        public ActionResult Index()
        {
            return this.View(this.photoService.GetList());
        }

        [Authorize]
        public ActionResult Edit(long id)
        {
            return this.View(this.photoService.GetSet(id));
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection form)
        {
            var photoSet = new PhotoSet
                               {
                                   SetId = Convert.ToInt64(form["SetId"]),
                                   SetName = form["SetName"],
                                   Description = form["Description"]
                               };

            this.photoService.Edit(photoSet);
            return this.RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Create()
        {
            return this.View();
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection form)
        {
            try
            {
                var photoSet = new PhotoSet
                                   {
                                       SetId = Convert.ToInt64(form["SetId"]),
                                       SetName = form["SetName"],
                                       Description = form["Description"]
                                   };
                this.photoService.CreatePhotoSet(photoSet);
                return this.RedirectToAction("Index");
            }
            catch
            {
                return this.View();
            }
        }
    }
}