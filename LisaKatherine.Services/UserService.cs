namespace LisaKatherine.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Security;

    using LisaKatherine.DataEntitiesRepository;
    using LisaKatherine.Interface;

    public class UserService
    {
        private readonly IUserFactory userFactory;

        public UserService(IUserFactory userFactory)
        {
            this.userFactory = userFactory;
        }

        public UserService()
        {
            this.userFactory = new UserFactoryEntities();
        }

        public IEnumerable<IUser> GetList()
        {
            return this.userFactory.GetList();
        }

        public IUser CreateUser(string username, string firstname, string lastname, string password)
        {
            var user = new User(Guid.NewGuid(), username, password, firstname, lastname);
            this.userFactory.Add(user);
            return user;
        }

        public IUser GetUser(Guid userId)
        {
            return this.userFactory.Get(userId);
        }

        public void EditUser(IUser user)
        {
            this.userFactory.Update(user);
        }

        public void DeleteUser(Guid userId)
        {
            this.userFactory.Delete(userId);
        }

        public IUser AuthenticateUser(string username, string password)
        {
            IEnumerable<IUser> users =
                (from u in this.GetList() where u.Username == username && u.Password == password select u);
            if (users.Any())
            {
                IUser user = users.First();
                var authTicket = new FormsAuthenticationTicket(
                    1, user.Username, DateTime.Now, DateTime.Now.AddMinutes(400), true, user.UserId.ToString());
                string encryptedticket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedticket)
                                     {
                                         Expires =
                                             DateTime
                                             .Now
                                             .AddHours(
                                                 4)
                                     };
                HttpContext.Current.Response.Cookies.Add(authCookie);

                return user;
            }
            return null;
        }

        public void LogOffUser()
        {
            string cookie = FormsAuthentication.FormsCookieName;
            HttpCookie httpcookie = HttpContext.Current.Request.Cookies[cookie];
            if (httpcookie == null)
            {
                return;
            }
            httpcookie.Expires = DateTime.Now.AddMinutes(-10.0);
            HttpContext.Current.Response.SetCookie(httpcookie);
        }

        public IUser GetUserFromTicket(string userData)
        {
            var userId = new Guid(userData);
            return this.GetUser(userId);
        }

        public string CreateUserData(IUser user)
        {
            return user.UserId.ToString();
        }

        public IUser CheckSession()
        {
            string cookie = FormsAuthentication.FormsCookieName;
            HttpCookie httpcookie = HttpContext.Current.Request.Cookies[cookie];
            if (httpcookie == null)
            {
                return null;
            }
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(httpcookie.Value);
            if (ticket == null || ticket.Expired)
            {
                return null;
            }
            try
            {
                IUser user = new UserService().GetUserFromTicket(ticket.UserData);
                return user;
            }
            catch
            {
                return null;
            }
        }
    }
}