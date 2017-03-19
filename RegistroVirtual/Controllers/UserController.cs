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
        static List<SubjectViewModel> RelatedSubjects;

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
            List<SelectListItem> institutionsOptions = new List<SelectListItem>();
            List<InstitutionModel> institutions = new Institution().GetInstitutionsList().ToList();
            InitStaticVariables();

            foreach (InstitutionModel institution in institutions)
            {
                institutionsOptions.Add(new SelectListItem
                {
                    Text = institution.Name,
                    Value = institution.Id.ToString()
                });
            }

            if (!string.IsNullOrEmpty(id))
            {
                User userDomain = new User();
                user = userDomain.Get(id);

                //user.Subjects = new MultiSelectList(subjects, "Id", "Name", user.SelectedSubjects.ToArray());
            }

            user.Institutions = institutionsOptions;

            if (error)
            {
                ViewBag.ErrorMessage = "Debido a un error no se ha podido guardar el usuario.";
            }

            return View(user);
        }

        public PartialViewResult LoadSubjects(int institutionId)
        {
            List<ClassModel> classes = new Class().GetClassesListByInstitution(institutionId).ToList();
            List<SubjectModel> subjects = new Subject().GetList().ToList();
            List<SubjectViewModel> subjectViewModels = new List<SubjectViewModel>();

            foreach (SubjectModel subject in subjects) {
                SubjectViewModel sub = RelatedSubjects.Where(x => x.Id.Equals(subject.Id)).FirstOrDefault();

                if (sub != null)
                {
                    sub.Name = subject.Name;
                    sub.Classes = new MultiSelectList(classes, "Id", "Name", sub.SelectedClasses.ToArray());
                }
                else
                {
                    sub = new SubjectViewModel();
                    sub.Id = subject.Id;
                    sub.Name = subject.Name;
                    sub.Classes = new MultiSelectList(classes, "Id", "Name");
                }

                subjectViewModels.Add(sub);
            }

            return PartialView(subjectViewModels);
        }

        public void SaveSubjects(List<SubjectViewModel> subjectAndClasses)
        {
            if (subjectAndClasses != null)
            {
                foreach (SubjectViewModel subjClass in subjectAndClasses)
                {
                    SubjectViewModel sub = RelatedSubjects.Where(x => x.Id.Equals(subjClass.Id)).FirstOrDefault();

                    if (sub != null && sub.Id > 0)
                    {
                        foreach (int classId in subjClass.SelectedClasses)
                        {
                            if (!sub.SelectedClasses.Contains(classId))
                            {
                                sub.SelectedClasses.Add(classId);
                            }
                        }
                    }
                    else
                    {
                        RelatedSubjects.Add(subjClass);
                    }
                }
            }
        }

        private void InitStaticVariables()
        {
            RelatedSubjects = new List<SubjectViewModel>();
            List<ClassesBySubjectModel> classesAndSubjects = ((UserModel)Session["User"]).RelatedSubjectsAndClasses;

            foreach (ClassesBySubjectModel relatedSubject in classesAndSubjects) {

                SubjectViewModel subViewOption = RelatedSubjects.Where(x => x.Id.Equals(relatedSubject.Subject.Id)).FirstOrDefault();

                if (subViewOption != null && subViewOption.Id > 0)
                {
                    foreach (int classId in relatedSubject.SelectedClasses.Select( x => x.Id).ToList())
                    {
                        if (!subViewOption.SelectedClasses.Contains(classId))
                        {
                            subViewOption.SelectedClasses.Add(classId);
                        }
                    }
                }
                else
                {
                    subViewOption = new SubjectViewModel();
                    subViewOption.Id = relatedSubject.Subject.Id;
                    subViewOption.Name = relatedSubject.Subject.Name;
                    subViewOption.SelectedClasses = relatedSubject.SelectedClasses.Select(x => x.Id).ToList();

                    RelatedSubjects.Add(subViewOption);
                }

            }

        }

        public ActionResult ImportTask()
        {
            return View();
        }

        public ActionResult Save(UserModel userModel)
        {
            User user = new User();

            foreach (SubjectViewModel relatedSubject in RelatedSubjects)
            {
                if (relatedSubject.SelectedClasses.Count() > 0 && relatedSubject.Id > 0)
                {
                    ClassesBySubjectModel classesBySubject = new ClassesBySubjectModel();
                    SubjectModel sub = new SubjectModel
                    {
                        Id = relatedSubject.Id
                    };

                    classesBySubject.Subject = sub;

                    foreach (int classId in relatedSubject.SelectedClasses)
                    {
                        ClassModel @class = new ClassModel
                        {
                            Id = classId
                        };

                        classesBySubject.SelectedClasses.Add(@class);
                    }

                    userModel.RelatedSubjectsAndClasses.Add(classesBySubject);
                }
            }

            if (userModel.Password.Equals(userModel.ConfirmPassword))
            {
                if (user.Save(userModel))
                {
                    //update user instance in the session variable
                    Session["User"] = userModel;

                    return RedirectToAction("Index", new { success = true });
                }
                else
                {
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
            else
            {
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