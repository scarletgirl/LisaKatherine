namespace LisaKatherine.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using LisaKatherine.Interface;
    using LisaKatherine.Services;

    public class UserController : Controller
    {
        private readonly UserService userService = new UserService();

        public ActionResult Index()
        {
            IEnumerable<IUser> users = this.userService.GetList();
            return this.View(users);
        }

        [Authorize]
        public ActionResult Create()
        {
            return this.View();
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([Bind(Exclude = "UserId")] User user)
        {
            try
            {
                this.userService.CreateUser(user);

                return this.RedirectToAction("Index");
            }

            catch
            {
                return this.View();
            }
        }

        [Authorize]
        public ActionResult Edit(Guid id)
        {
            return this.View(this.userService.GetUser(id));
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(User user)
        {
            if (this.ModelState.IsValid)
            {
                this.userService.EditUser(user);
            }

            return this.RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(Guid id)
        {
            this.userService.DeleteUser(id);
            return this.RedirectToAction("Index");
        }

        public ActionResult LogOn()
        {
            return this.View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LogOn(User user)
        {
            IUser authUser = this.userService.AuthenticateUser(user.Username, user.Password);
            if (authUser != null)
            {
                return this.Redirect("~/Admin");
            }
            return this.View();
        }

        [Authorize]
        public ActionResult LogOff()
        {
            this.userService.LogOffUser();
            return this.Redirect("~/");
        }
    }
}