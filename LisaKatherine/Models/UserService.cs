namespace LisaKatherine.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Security;

    public class UserService
    {
        private readonly LisaKatherineEntities dataModel;

        public UserService(LisaKatherineEntities dataModel)
        {
            this.dataModel = dataModel;
        }

        public UserService()
        {
            this.dataModel = new LisaKatherineEntities();
        }

        public IEnumerable<Users> GetList()
        {
            return this.dataModel.Users1;
        }

        public void CreateUser(Users user)
        {
            user.userId = Guid.NewGuid();
            this.dataModel.AddToUsers1(user);
            this.dataModel.SaveChanges();
        }

        public Users GetUser(Guid userId)
        {
            Users user = (from u in this.dataModel.Users1 where u.userId == userId select u).First();
            return user;
        }

        public void EditUser(Users user)
        {
            Users originalUser = (from u in this.dataModel.Users1 where u.userId == user.userId select u).First();

            this.dataModel.ApplyCurrentValues(originalUser.EntityKey.EntitySetName, user);
            this.dataModel.SaveChanges();
        }

        public void DeleteUser(Guid userId)
        {
            Users user = (from u in this.dataModel.Users1 where u.userId == userId select u).First();
            this.dataModel.DeleteObject(user);
            this.dataModel.SaveChanges();
        }

        public Users AuthenticateUser(string username, string password)
        {
            IQueryable<Users> users =
                (from u in this.dataModel.Users1 where u.username == username && u.password == password select u);
            if (users.Any())
            {
                Users user = users.First();
                var authTicket = new FormsAuthenticationTicket(
                    1, user.username, DateTime.Now, DateTime.Now.AddMinutes(400), true, user.userId.ToString());
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

        public Users GetUserFromTicket(string userData)
        {
            var userId = new Guid(userData);
            return this.GetUser(userId);
        }

        public string CreateUserData(Users user)
        {
            return user.userId.ToString();
        }

        public Users CheckSession()
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
                Users user = new UserService().GetUserFromTicket(ticket.UserData);
                return user;
            }
            catch
            {
                return null;
            }
        }
    }
}