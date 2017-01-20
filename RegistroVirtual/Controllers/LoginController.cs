using Domain;
using Models;
using RegistroVirtual.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RegistroVirtual.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Validate(UserModel userModel)
        {
            User user = new User();

            if (user.Authenticate(userModel))
            {
                User userDomain = new User();
                userModel = userDomain.GetUserByUsername(userModel.Username);
                userModel.Password = string.Empty;

                Session["User"] = userModel;
                return RedirectToAction("Index", "Dashboard");
            }

            return View("Index");
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return View("Index");
        }

    }
}