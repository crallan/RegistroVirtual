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
    [SessionAuthorize]
    public class RegisterProfileController : Controller
    {

        public ActionResult Index(string NameFilter)
        {
            List<RegisterProfileModel> profiles = new List<RegisterProfileModel>();
            RegisterProfile registerProfile = new RegisterProfile();
            profiles = registerProfile.GetProfiles().ToList();

            if (!string.IsNullOrEmpty(NameFilter))
            {
                profiles = profiles.Where(x => x.Name.ToLowerInvariant().Contains(NameFilter.ToLowerInvariant())).ToList();
            }

            return View(profiles);
        }

        public ActionResult Create(string id)
        {
            RegisterProfileModel registerProfile = new RegisterProfileModel();

            List<SelectListItem> trimesterOptions = new List<SelectListItem>();
            List<SelectListItem> schoolYearsOptions = new List<SelectListItem>();
            List<TrimesterModel> trimesters = new Trimester().GetTrimesters().ToList();
            List<SchoolYearModel> schoolYears = new SchoolYear().GetSchoolYears().ToList();

            if (!string.IsNullOrEmpty(id))
            {
                RegisterProfile registerProfileDomain = new RegisterProfile();
                registerProfile = registerProfileDomain.Get(id);
            }

            foreach (TrimesterModel trimester in trimesters)
            {
                trimesterOptions.Add(new SelectListItem
                {
                    Text = trimester.Name,
                    Value = trimester.Id.ToString()
                });
            }

            registerProfile.Trimesters = trimesterOptions;

            foreach (SchoolYearModel year in schoolYears)
            {
                schoolYearsOptions.Add(new SelectListItem
                {
                    Text = year.Name,
                    Value = year.Id.ToString()
                });
            }

            registerProfile.SchoolYears = schoolYearsOptions;

            return View(registerProfile);
        }

        public ActionResult Save(RegisterProfileModel profileModel)
        {
            RegisterProfile registerProfile = new RegisterProfile();
            profileModel.UserId = ((UserModel)Session["User"]).Id;

            if (registerProfile.Save(profileModel))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Create");
            }
        }

    }
}