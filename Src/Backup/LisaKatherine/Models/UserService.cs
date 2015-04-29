using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web;

namespace LisaKatherine.Models
{
    public class UserService
    {
        private readonly LisaKatherineEntities _dataModel;

        public UserService(LisaKatherineEntities dataModel) {
            _dataModel = dataModel;
        }

        public UserService() {
            _dataModel = new LisaKatherineEntities();
        }

        public IEnumerable<Users> GetList()
        {
             return _dataModel.Users1;
        }


        public void CreateUser(Users user)
        {
            user.userId = Guid.NewGuid();
            _dataModel.AddToUsers1(user);
            _dataModel.SaveChanges();
        }

        public Users GetUser(Guid userId)
        {
            var user = (from u in _dataModel.Users1 where u.userId == userId select u).First();
            return user;
        }

        public void EditUser(Users user) {
            var originalUser = (from u in _dataModel.Users1 where u.userId == user.userId select u).First();

            _dataModel.ApplyCurrentValues(originalUser.EntityKey.EntitySetName, user);
            _dataModel.SaveChanges();
        }

        public void DeleteUser(Guid userId)
        {
            var user = (from u in _dataModel.Users1 where u.userId == userId select u).First();
            _dataModel.DeleteObject(user);
            _dataModel.SaveChanges();
        }

        public Users AuthenticateUser(string username, string password)
        {
            var users = (from u in _dataModel.Users1 where u.username == username && u.password == password select u);
            if (users.Count() > 0) {
                var user = users.First();
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, user.username, DateTime.Now, DateTime.Now.AddMinutes(400), true, user.userId.ToString());
                string encryptedticket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedticket);
                authCookie.Expires = DateTime.Now.AddHours(4);
                HttpContext.Current.Response.Cookies.Add(authCookie);
               
                return user;
            }
            return null;
        }

        public void LogOffUser() {
            string cookie = FormsAuthentication.FormsCookieName;
            HttpCookie httpcookie = HttpContext.Current.Request.Cookies[cookie];
            if (httpcookie == null) return;
            httpcookie.Expires = DateTime.Now.AddMinutes(-10.0);
            HttpContext.Current.Response.SetCookie(httpcookie);
        }

        public Users GetUserFromTicket(string userData)
        {
            Guid userId = new Guid(userData);
            return GetUser(userId);
        }

        public string CreateUserData(Users user) {
            return user.userId.ToString();

        }

        public Users CheckSession()
        {
            string cookie = FormsAuthentication.FormsCookieName;
            HttpCookie httpcookie = HttpContext.Current.Request.Cookies[cookie];
            if (httpcookie == null) return null;
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(httpcookie.Value);
            if (ticket == null || ticket.Expired) return null;
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