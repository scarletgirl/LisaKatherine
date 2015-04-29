using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LisaKatherine.Models;

namespace LisaKatherine.Controllers
{
    public class UserController : Controller
    {
        readonly UserService _userService = new UserService();

        [Authorize]
        public ActionResult Index()
        {

            IEnumerable<Users> users = _userService.GetList();
            return View(users);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([Bind(Exclude = "UserId")] Users user)
        {
            try
            {
                _userService.CreateUser(user);
                // TODO: Add insert logic here 

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
            return View(_userService.GetUser(id));
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Users user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _userService.EditUser(user);
                }

                return RedirectToAction("Index");
            }

            catch
            {
                return View();
            }

        }

        [Authorize]
        public ActionResult Delete(Guid id)
        {
            _userService.DeleteUser(id);
            return RedirectToAction("Index");
        }

        public ActionResult LogOn() {
            return View();
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LogOn(Users user) {
            Users authUser = _userService.AuthenticateUser(user.username, user.password);
            if (authUser != null)
            {
                return Redirect("~/Admin");
            }
            return View();
        
        }

        [Authorize]
        public ActionResult LogOff()
        {
            _userService.LogOffUser();
            return Redirect("~/");
        }
    }
}
