﻿using Domain;
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
        static List<ExamModel> Exams;
        static List<ExtraclassWorkModel> Extraclasses;
        
        public ActionResult Index(string NameFilter)
        {
            List<RegisterProfileModel> profiles = new List<RegisterProfileModel>();
            RegisterProfile registerProfile = new RegisterProfile();
            profiles = registerProfile.GetProfiles().ToList();

            if (!string.IsNullOrEmpty(NameFilter))
            {
                profiles = profiles.Where(x => x.Name.ToLowerInvariant().Contains(NameFilter.ToLowerInvariant())).ToList();
            }

            foreach(RegisterProfileModel profile in profiles)
            {
                profile.Exams = new Exams().GetExamsByRegisterProfile(profile.Id).ToList();
                profile.ExtraclassWorks = new ExtraclassWork().GetExtraclassWorksByRegisterProfile(profile.Id).ToList();
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
            InitStaticVariables();

            if (!string.IsNullOrEmpty(id))
            {
                int registerProfileId = Convert.ToInt32(id);

                Exams = new Exams().GetExamsByRegisterProfile(registerProfileId).ToList();
                Exams.Add(new ExamModel() { Percentage = 0, Score = 0, Id = 0 });
                Extraclasses = new ExtraclassWork().GetExtraclassWorksByRegisterProfile(registerProfileId).ToList();
                Extraclasses.Add(new ExtraclassWorkModel() { Percentage = 0, Id = 0 });
            }

            if (!string.IsNullOrEmpty(id))
            {
                RegisterProfile registerProfileDomain = new RegisterProfile();
                registerProfile = registerProfileDomain.Get(id);

                if (registerProfile == null) {
                    registerProfile = new RegisterProfileModel();
                    registerProfile.AssistancePercentage = 1;
                    registerProfile.ConceptPercentage = 1;
                    registerProfile.DailyWorkPercentage = 1;
                }

            }
            else {
                registerProfile.AssistancePercentage = 1;
                registerProfile.ConceptPercentage = 1;
                registerProfile.DailyWorkPercentage = 1;
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

        public PartialViewResult LoadExams()
        {
            return PartialView(Exams.OrderByDescending( x => x.Percentage));
        }

        public ActionResult AddExam(string Id,  string Percentage, string Score)
        {
            if (!Percentage.Equals("0") && !Score.Equals("0"))
            {
                if (Id.Equals("0"))
                {
                    ExamModel newExam = new ExamModel(float.Parse(Percentage), Convert.ToInt32(Score));
                    newExam.Id = Exams.OrderBy(x => x.Id).Last().Id + 1000;

                    Exams.Add(newExam);
                }
                else {
                    foreach (var item in Exams.Where(w => w.Id.Equals(Convert.ToInt32(Id))))
                    {
                        item.Percentage = float.Parse(Percentage);
                        item.Score = Convert.ToInt32(Score);
                    }
                }
                
            }

            return RedirectToAction("LoadExams");
        }

        public PartialViewResult LoadExtraclasses()
        {
            return PartialView(Extraclasses.OrderByDescending(x => x.Percentage));
        }

        public ActionResult AddExtraclass(string Id, string Percentage)
        {
            if (!Percentage.Equals("0"))
            {
                if (Id.Equals("0"))
                {
                    ExtraclassWorkModel newExtraclass = new ExtraclassWorkModel(float.Parse(Percentage));
                    newExtraclass.Id = Exams.OrderBy(x => x.Id).Last().Id + 1;

                    Extraclasses.Add(newExtraclass);
                }
                else {
                    foreach (var item in Extraclasses.Where(w => w.Id.Equals(Convert.ToInt32(Id))))
                    {
                        item.Percentage = float.Parse(Percentage);
                    }
                }
            }

            return RedirectToAction("LoadExtraclasses");
        }

        public ActionResult Save(RegisterProfileModel profileModel)
        {
            RegisterProfile registerProfile = new RegisterProfile();
            profileModel.UserId = ((UserModel)Session["User"]).Id;

            profileModel.ExtraclassWorks = Extraclasses.Where( x => x.Percentage != 0).ToList();
            profileModel.Exams = Exams.Where(x => x.Percentage != 0 && x.Score != 0).ToList(); ;

            if (registerProfile.Save(profileModel))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Create");
            }
        }

        private void InitStaticVariables() {
            Exams = new List<ExamModel>();
            Exams.Add(new ExamModel() { Percentage = 0, Score = 0, Id = 0 });
            Extraclasses = new List<ExtraclassWorkModel>();
            Extraclasses.Add(new ExtraclassWorkModel() { Percentage = 0, Id = 0 });
        } 

    }
}