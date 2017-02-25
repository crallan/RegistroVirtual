using Domain;
using Models;
using RegistroVirtual.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RegistroVirtual.Controllers
{
    [SessionAuthorize]
    public class UserController : Controller
    {
        public ActionResult Index(string FirstNameFilter, string LastNameFilter, bool success = false)
        {
            List<UserModel> users = new List<UserModel>();
            User user = new User();
            users = user.GetList().ToList();

            if (!string.IsNullOrEmpty(FirstNameFilter))
            {
                users = users.Where(x => x.FirstName.ToLowerInvariant().Contains(FirstNameFilter.ToLowerInvariant())).ToList();
            }

            if (!string.IsNullOrEmpty(LastNameFilter))
            {
                users = users.Where(x => x.LastName.ToLowerInvariant().Contains(LastNameFilter.ToLowerInvariant())).ToList();
            }

            if (success)
            {
                ViewBag.SuccessMessage = "El usuario se ha guardado de manera exitosa";
            }

            return View(users);
        }

        public ActionResult Create(string id, bool error = false)
        {
            UserModel user = new UserModel();
            List<SubjectModel> subjects = new Subject().GetList().ToList();

            if (!string.IsNullOrEmpty(id))
            {
                User userDomain = new User();
                user = userDomain.Get(id);
            }

            user.Subjects = new MultiSelectList(subjects, "Id", "Name");

            if (error)
            {
                ViewBag.ErrorMessage = "Debido a un error no se ha podido guardar el usuario.";
            }

            return View(user);
        }

        public ActionResult ImportTask()
        {
            return View();
        }

        public ActionResult Save(UserModel userModel)
        {
            User user = new User();

            if (user.Save(userModel))
            {
                return RedirectToAction("Index", new { success = true });
            }
            else {
                if (!userModel.Id.Equals(0))
                {
                    return RedirectToAction("Create", new { id = userModel.Id, error = true });
                }
                else
                {
                    return RedirectToAction("Create", new { error = true });
                }
            } 
        }

    }
}