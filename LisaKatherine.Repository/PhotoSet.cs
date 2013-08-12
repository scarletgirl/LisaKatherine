namespace LisaKatherine.Interface
{
    public class PhotoSet : IPhotoSet
    {
        public long SetId { get; set; }

        public string SetName { get; set; }

        public string Description { get; set; }
    }
}