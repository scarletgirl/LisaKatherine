namespace LisaKatherine.Interface
{
    public interface IFacebookUser
    {
        long UserId { get; set; }

        string AccessToken { get; set; }

        string UserName { get; set; }

        string Gender { get; set; }
    }
}