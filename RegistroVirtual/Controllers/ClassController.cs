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

        public ActionResult Index(string NameFilter)
        {
            List<ClassModel> classes = new List<ClassModel>();
            Class @class = new Class();
            classes = @class.GetClassesList().ToList();

            if (!string.IsNullOrEmpty(NameFilter))
            {
                classes = classes.Where(x => x.Name.ToLowerInvariant().Contains(NameFilter.ToLowerInvariant())).ToList();
            }

            return View(classes);
        }

        public ActionResult Create(string id)
        {
            ClassModel @class = new ClassModel();
            List<SelectListItem> institutionsOptions = new List<SelectListItem>();
            List<InstitutionModel> institutions = new Institution().GetInstitutionsList().ToList();

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
                Class classDomain = new Class();
                @class = classDomain.Get(id);
            }

            @class.Institutions = institutionsOptions;

            return View(@class);
        }

        public ActionResult Save(ClassModel classModel)
        {
            Class @class = new Class();

            if (@class.Save(classModel))
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