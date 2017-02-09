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
        public ActionResult Index(string returnUrl, bool error = false)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (error)
            {
                ViewBag.ErrorMessage = "No se ha podido realizar la autenticacion, valide sus credenciales.";
            }

            return View();
        }

        public ActionResult Validate(UserModel userModel)
        {
            User user = new User();
            string returnUrl = Request.Form["returnUrl"];

            if (user.Authenticate(userModel))
            {
                User userDomain = new User();
                userModel = userDomain.GetUserByUsername(userModel.Username);
                userModel.Password = string.Empty;

                Session["User"] = userModel;
               
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else {
                    return RedirectToAction("Index", "Dashboard");
                }
                
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("Index", new { error = true });
            }
            else
            {
                return RedirectToAction("Index", new { returnUrl = returnUrl, error = true });
            }

            
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return View("Index");
        }

    }
}