namespace LisaKatherine.Interface
{
    public interface IPhoto
    {
        string Title { get; set; }

        string Link { get; set; }

        string LargeUrl { get; set; }

        string MediumUrl { get; set; }

        int Height { get; set; }

        int Width { get; set; }

        string ThumbnailUrl { get; set; }

        int ThumbHeight { get; set; }

        int ThumbWidth { get; set; }

        Orientation Orientation { get; set; }
    }
}