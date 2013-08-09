namespace LisaKatherine.Interface
{
    using System;

    public class User : IUser
    {
        public User(Guid userId, string username, string password, string firstname, string lastname)
        {
            this.UserId = userId;
            this.Username = username;
            this.Password = password;
            this.FirstName = firstname;
            this.LastName = lastname;
        }

        public Guid UserId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}