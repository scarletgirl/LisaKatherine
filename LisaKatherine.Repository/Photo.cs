namespace LisaKatherine.Interface
{
    public class Photo : IPhoto
    {
        public string Title { get; set; }

        public string Link { get; set; }

        public string LargeUrl { get; set; }

        public string MediumUrl { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public string ThumbnailUrl { get; set; }

        public int ThumbHeight { get; set; }

        public int ThumbWidth { get; set; }

        public Orientation Orientation { get; set; }
    }
}