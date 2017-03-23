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
    public class ClassController : Controller
    {

        public ActionResult Index(string NameFilter, bool success = false)
        {
            List<ClassModel> classes = new List<ClassModel>();
            Class @class = new Class();
            classes = @class.GetClassesList().ToList();

            if (!string.IsNullOrEmpty(NameFilter))
            {
                classes = classes.Where(x => x.Name.ToLowerInvariant().Contains(NameFilter.ToLowerInvariant())).ToList();
            }

            if (success)
            {
                ViewBag.SuccessMessage = "La clase se ha guardado de manera exitosa";
            }

            return View(classes);
        }

        public ActionResult Create(string id, bool error = false)
        {
            ClassModel @class = new ClassModel();
            List<SelectListItem> institutionsOptions = new List<SelectListItem>();
            List<InstitutionModel> institutions = new Institution().GetInstitutionsList().ToList();
            List<SelectListItem> schoolYearsOptions = new List<SelectListItem>();
            List<SchoolYearModel> schoolYears = new SchoolYear().GetSchoolYears().ToList();

            foreach (InstitutionModel institution in institutions)
            {
                institutionsOptions.Add(new SelectListItem
                {
                    Text = institution.Name,
                    Value = institution.Id.ToString()
                });
            }

            foreach (SchoolYearModel schoolYear in schoolYears)
            {
                schoolYearsOptions.Add(new SelectListItem
                {
                    Text = schoolYear.Name,
                    Value = schoolYear.Id.ToString()
                });
            }

            if (!string.IsNullOrEmpty(id))
            {
                Class classDomain = new Class();
                @class = classDomain.Get(id);
            }

            @class.Institutions = institutionsOptions;
            @class.SchoolYears = schoolYearsOptions;

            if (error)
            {
                ViewBag.ErrorMessage = "Debido a un error no se ha podido guardar la clase.";
            }

            return View(@class);
        }

        public ActionResult Save(ClassModel classModel)
        {
            Class @class = new Class();

            if (@class.Save(classModel))
            {
                return RedirectToAction("Index", new { success = true });
            }
            else
            {
                if (!classModel.Id.Equals(0))
                {
                    return RedirectToAction("Create", new { id = classModel.Id, error = true });
                }
                else
                {
                    return RedirectToAction("Create", new { error = true });
                }
            }

        }
    }
}