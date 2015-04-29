using System;
using System.Web.Mvc;
using LisaKatherine.Interface;
using LisaKatherine.Services;

namespace LisaKatherine.Controllers
{
    public class FlickrController : Controller
    {
        private const string FlickrPhotostreamUrl =
            "http://api.flickr.com/services/feeds/photos_public.gne?id=48225317@N08&lang=en-us&format=rss_200";

        private readonly PhotoService photoService = new PhotoService();

        public PartialViewResult FlickrPhotostream()
        {
            var flickrImageList = photoService.GetNumberOfImages(3, FlickrPhotostreamUrl);
            return PartialView("_Flickr", flickrImageList);
        }

        [Authorize]
        public ActionResult Index()
        {
            return View(photoService.GetList());
        }

        [Authorize]
        public ActionResult Edit(long id)
        {
            return View(photoService.GetSet(id));
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

            photoService.Edit(photoSet);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
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
                photoService.CreatePhotoSet(photoSet);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}