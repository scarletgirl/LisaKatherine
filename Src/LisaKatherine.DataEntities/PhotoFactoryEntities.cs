using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LisaKatherine.Interface;

namespace LisaKatherine.DataEntitiesRepository
{
    public class PhotoFactoryEntities : IPhotoFactory
    {
        private readonly LisaKatherineEntities dataModel;

        public PhotoFactoryEntities()
        {
            dataModel = new LisaKatherineEntities();
        }

        public PhotoFactoryEntities(LisaKatherineEntities dataModel)
        {
            this.dataModel = dataModel;
        }

        public List<IPhoto> GetImages(string feedUrl)
        {
            var photos = new List<IPhoto>();

            var xml = GetXmlDocument(feedUrl);
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

            foreach (var photo in
                list.Select(
                    image =>
                        new Photo
                        {
                            LargeUrl = image.ImageUrl,
                            MediumUrl = image.ImageUrl.Replace("_b.", "_m."),
                            Height = int.Parse(image.Height, 0),
                            Width = int.Parse(image.Width, 0),
                            Title = image.Title,
                            Link = image.Link,
                            ThumbnailUrl = image.ThumbnailUrl,
                            ThumbWidth = int.Parse(image.ThumbWidth, 0),
                            ThumbHeight = int.Parse(image.ThumbHeight, 0)
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
            IEnumerable<IPhoto> photos = GetImages(feedUrl);
            return photos.Take(count);
        }

        public XDocument GetXmlDocument(string feedUrl)
        {
            return XDocument.Load(feedUrl);
        }

        public IPhoto GetFirstImage(string feedUrl)
        {
            return GetNumberOfImages(1, feedUrl).First();
        }

        public IEnumerable<IPhotoSet> GetList()
        {
            var sets = dataModel.FlickrSets;
            return
                Enumerable.Cast<IPhotoSet>(
                    sets.Select(
                        set => new PhotoSet { Description = set.Description, SetId = set.SetId, SetName = set.SetName }))
                    .ToList();
        }

        public IPhotoSet GetSet(long id)
        {
            var set = (from s in dataModel.FlickrSets where s.SetId == id select s).First();
            return new PhotoSet { Description = set.Description, SetId = set.SetId, SetName = set.SetName };
        }

        public void CreatePhotoSet(IPhotoSet f)
        {
            dataModel.AddToFlickrSets(
                new FlickrSets { Description = f.Description, SetId = f.SetId, SetName = f.SetName });
            dataModel.SaveChanges();
        }

        public void Edit(IPhotoSet photoSet)
        {
            var originalPhotoSet =
                (from a in dataModel.FlickrSets where a.SetId == photoSet.SetId select a).First();

            dataModel.ApplyCurrentValues(originalPhotoSet.EntityKey.EntitySetName, photoSet);
            dataModel.SaveChanges();
        }
    }
}