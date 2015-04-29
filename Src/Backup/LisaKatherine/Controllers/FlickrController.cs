using System;
using System.Collections.Generic;
using System.Web.Mvc;
using LisaKatherine.Models;

namespace LisaKatherine.Controllers
{
    public class FlickrController : Controller
    {
        readonly FlickrImageService _flickrImageService = new FlickrImageService();
        private const string FlickrPhotostreamUrl = "http://api.flickr.com/services/feeds/photos_public.gne?id=48225317@N08&lang=en-us&format=rss_200";


        public PartialViewResult FlickrPhotostream()
        {
            IEnumerable<FlickrImage> flickrImageList = _flickrImageService.GetNumberOfImages(3, FlickrPhotostreamUrl);
            return PartialView("_Flickr", flickrImageList);
        }
        [Authorize]
        public ActionResult Index() {

            return View(_flickrImageService.GetList());
        }
        [Authorize]
        public ActionResult Edit(long id) {
            return View(_flickrImageService.GetSet(id));
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection form)
        {
            var flickrSet = new FlickrSets
                                {
                SetId = Convert.ToInt64(form["SetId"]),
                SetName = form["SetName"],
                Description = form["Description"]
            };

            _flickrImageService.Edit(flickrSet);
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
                var flickrSet = new FlickrSets
                                    {
                    SetId = Convert.ToInt64(form["SetId"]),
                    SetName = form["SetName"],
                    Description = form["Description"]
                };
                _flickrImageService.CreateFlickrSet(flickrSet);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
