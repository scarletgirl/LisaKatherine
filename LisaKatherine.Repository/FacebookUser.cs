namespace LisaKatherine.Interface
{
    public class FacebookUser : IFacebookUser
    {
        public FacebookUser(long userId, string accessToken, string userName, string gender)
        {
            this.UserId = userId;
            this.AccessToken = accessToken;
            this.UserName = userName;
            this.Gender = gender;
        }

        public long UserId { get; set; }

        public string AccessToken { get; set; }

        public string UserName { get; set; }

        public string Gender { get; set; }
    }
}