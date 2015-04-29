namespace LisaKatherine.Interface
{
    using System;
    using System.ComponentModel.DataAnnotations;

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

        public User()
        {
        }

        public Guid UserId { get; set; }

        [Display(Name = "User Name")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter a password")]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}