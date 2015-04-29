namespace LisaKatherine.Services
{
    using System.Collections.Generic;
    using System.Xml.Linq;

    using LisaKatherine.DataEntitiesRepository;
    using LisaKatherine.Interface;

    public class PhotoService
    {
        //private readonly LisaKatherineEntities dataModel;
        private readonly IPhotoFactory photoFactory;

        public PhotoService(IPhotoFactory photoFactory)
        {
            this.photoFactory = photoFactory;
        }

        public PhotoService()
        {
            this.photoFactory = new PhotoFactoryEntities();
        }

        public List<IPhoto> GetImages(string feedUrl)
        {
            return this.photoFactory.GetImages(feedUrl);
        }

        public IEnumerable<IPhoto> GetNumberOfImages(int count, string feedUrl)
        {
            return this.photoFactory.GetNumberOfImages(count, feedUrl);
        }

        public XDocument GetXmlDocument(string feedUrl)
        {
            return this.photoFactory.GetXmlDocument(feedUrl);
        }

        public IPhoto GetFirstImage(string feedUrl)
        {
            return this.photoFactory.GetFirstImage(feedUrl);
        }

        public IEnumerable<IPhotoSet> GetList()
        {
            return this.photoFactory.GetList();
        }

        public IPhotoSet GetSet(long id)
        {
            return this.photoFactory.GetSet(id);
        }

        public void CreatePhotoSet(IPhotoSet f)
        {
            this.photoFactory.CreatePhotoSet(f);
        }

        public void Edit(IPhotoSet photoSet)
        {
            this.photoFactory.Edit(photoSet);
        }
    }
}