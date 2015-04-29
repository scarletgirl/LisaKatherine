namespace LisaKatherine.Interface
{
    using System.Collections.Generic;
    using System.Xml.Linq;

    public interface IPhotoFactory
    {
        List<IPhoto> GetImages(string feedUrl);

        IEnumerable<IPhoto> GetNumberOfImages(int count, string feedUrl);

        XDocument GetXmlDocument(string feedUrl);

        IPhoto GetFirstImage(string feedUrl);

        IEnumerable<IPhotoSet> GetList();

        IPhotoSet GetSet(long id);

        void CreatePhotoSet(IPhotoSet f);

        void Edit(IPhotoSet photoSet);
    }
}