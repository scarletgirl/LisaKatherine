namespace LisaKatherine.Interface
{
    using System;

    public interface IUser
    {
        Guid UserId { get; set; }

        string Username { get; set; }

        string Password { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }
    }
}