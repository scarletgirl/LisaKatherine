using System;
using System.Web.Mvc;
using LisaKatherine.Interface;
using LisaKatherine.Services;

namespace LisaKatherine.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService userService = new UserService();

        public ActionResult Index()
        {
            var users = userService.GetList();
            return View(users);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([Bind(Exclude = "UserId")] User user)
        {
            try
            {
                userService.CreateUser(user);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult Edit(Guid id)
        {
            return View(userService.GetUser(id));
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                userService.EditUser(user);
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(Guid id)
        {
            userService.DeleteUser(id);
            return RedirectToAction("Index");
        }

        public ActionResult LogOn()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LogOn(User user)
        {
            var authUser = userService.AuthenticateUser(user.Username, user.Password);
            if (authUser != null)
            {
                return Redirect("~/Admin");
            }

            return View();
        }

        [Authorize]
        public ActionResult LogOff()
        {
            userService.LogOffUser();
            return Redirect("~/");
        }
    }
}