namespace LisaKatherine.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using LisaKatherine.DataEntitiesRepository;

    public class FlickrImageService
    {
        private readonly LisaKatherineEntities dataModel;

        public FlickrImageService(LisaKatherineEntities dataModel)
        {
            this.dataModel = dataModel;
        }

        public FlickrImageService()
        {
            this.dataModel = new LisaKatherineEntities();
        }

        public List<FlickrImage> GetImages(string feedUrl)
        {
            var flickrImages = new List<FlickrImage>();

            XDocument xml = this.GetXmlDocument(feedUrl);
            var n = (XNamespace)"http://search.yahoo.com/mrss/";
            var images = from x in xml.Descendants("item")
                         select
                             new
                                 {
                                     ImageUrl = x.Element(n + "content").Attribute("url").Value,
                                     Height = x.Element(n + "content").Attribute("height").Value,
                                     Width = x.Element(n + "content").Attribute("width").Value,
                                     Title = x.Element("title").Value,
                                     Link = x.Element("link").Value,
                                     ThumbnailUrl = x.Element(n + "thumbnail").Attribute("url").Value,
                                     ThumbHeight = x.Element(n + "thumbnail").Attribute("height").Value,
                                     ThumbWidth = x.Element(n + "thumbnail").Attribute("width").Value
                                 };

            foreach (FlickrImage flickrImage in
                images.Select(
                    image =>
                    new FlickrImage
                        {
                            LargeUrl = image.ImageUrl,
                            MediumUrl = image.ImageUrl.Replace("_b.", "_m."),
                            Height = Int32.Parse(image.Height, 0),
                            Width = Int32.Parse(image.Width, 0),
                            Title = image.Title,
                            Link = image.Link,
                            ThumbnailUrl = image.ThumbnailUrl,
                            ThumbWidth = Int32.Parse(image.ThumbWidth, 0),
                            ThumbHeight = Int32.Parse(image.ThumbHeight, 0)
                        }))
            {
                if (flickrImage.Height > flickrImage.Width)
                {
                    flickrImage.Orientation = Orientation.Portrait;
                }
                else if (flickrImage.Height < flickrImage.Width)
                {
                    flickrImage.Orientation = Orientation.Landscape;
                }
                else
                {
                    flickrImage.Orientation = Orientation.Square;
                }
                flickrImages.Add(flickrImage);
            }
            return flickrImages;
        }

        public IEnumerable<FlickrImage> GetNumberOfImages(int count, string feedUrl)
        {
            IEnumerable<FlickrImage> flickerImages = this.GetImages(feedUrl);
            return flickerImages.Take(count);
        }

        public XDocument GetXmlDocument(string feedUrl)
        {
            return XDocument.Load(feedUrl);
        }

        public FlickrImage GetFirstImage(string feedUrl)
        {
            return this.GetNumberOfImages(1, feedUrl).First();
        }

        public IEnumerable<FlickrSets> GetList()
        {
            return this.dataModel.FlickrSets;
        }

        public FlickrSets GetSet(long id)
        {
            return (from s in this.dataModel.FlickrSets where s.SetId == id select s).First();
        }

        public void CreateFlickrSet(FlickrSets f)
        {
            this.dataModel.AddToFlickrSets(f);
            this.dataModel.SaveChanges();
        }

        public void Edit(FlickrSets flickrSet)
        {
            FlickrSets originalFlickrSet =
                (from a in this.dataModel.FlickrSets where a.SetId == flickrSet.SetId select a).First();

            this.dataModel.ApplyCurrentValues(originalFlickrSet.EntityKey.EntitySetName, flickrSet);
            this.dataModel.SaveChanges();
        }
    }
}