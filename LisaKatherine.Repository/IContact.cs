namespace LisaKatherine.Interface
{
    public interface IContact
    {
        string From { get; set; }

        string Subject { get; set; }

        string Message { get; set; }
    }
}