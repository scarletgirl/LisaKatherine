namespace LisaKatherine.DataEntitiesRepository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Objects;
    using System.Linq;
    using System.Xml.Linq;

    using LisaKatherine.Interface;

    public class PhotoFactoryEntities : IPhotoFactory
    {
        private readonly LisaKatherineEntities dataModel;

        public PhotoFactoryEntities()
        {
            this.dataModel = new LisaKatherineEntities();
        }

        public PhotoFactoryEntities(LisaKatherineEntities dataModel)
        {
            this.dataModel = dataModel;
        }

        public List<IPhoto> GetImages(string feedUrl)
        {
            var photos = new List<IPhoto>();

            XDocument xml = this.GetXmlDocument(feedUrl);
            var n = (XNamespace)"http://search.yahoo.com/mrss/";
            var list = from x in xml.Descendants("item")
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

            foreach (Photo photo in
                list.Select(
                    image =>
                    new Photo
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
                if (photo.Height > photo.Width)
                {
                    photo.Orientation = Orientation.Portrait;
                }
                else if (photo.Height < photo.Width)
                {
                    photo.Orientation = Orientation.Landscape;
                }
                else
                {
                    photo.Orientation = Orientation.Square;
                }
                photos.Add(photo);
            }
            return photos;
        }

        public IEnumerable<IPhoto> GetNumberOfImages(int count, string feedUrl)
        {
            IEnumerable<IPhoto> photos = this.GetImages(feedUrl);
            return photos.Take(count);
        }

        public XDocument GetXmlDocument(string feedUrl)
        {
            return XDocument.Load(feedUrl);
        }

        public IPhoto GetFirstImage(string feedUrl)
        {
            return this.GetNumberOfImages(1, feedUrl).First();
        }

        public IEnumerable<IPhotoSet> GetList()
        {
            ObjectSet<FlickrSets> sets = this.dataModel.FlickrSets;
            return
                Enumerable.Cast<IPhotoSet>(
                    sets.Select(
                        set => new PhotoSet { Description = set.Description, SetId = set.SetId, SetName = set.SetName }))
                          .ToList();
        }

        public IPhotoSet GetSet(long id)
        {
            FlickrSets set = (from s in this.dataModel.FlickrSets where s.SetId == id select s).First();
            return new PhotoSet { Description = set.Description, SetId = set.SetId, SetName = set.SetName };
        }

        public void CreatePhotoSet(IPhotoSet f)
        {
            this.dataModel.AddToFlickrSets(
                new FlickrSets { Description = f.Description, SetId = f.SetId, SetName = f.SetName });
            this.dataModel.SaveChanges();
        }

        public void Edit(IPhotoSet photoSet)
        {
            FlickrSets originalPhotoSet =
                (from a in this.dataModel.FlickrSets where a.SetId == photoSet.SetId select a).First();

            this.dataModel.ApplyCurrentValues(originalPhotoSet.EntityKey.EntitySetName, photoSet);
            this.dataModel.SaveChanges();
        }
    }
}