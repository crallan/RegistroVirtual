using Domain;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RegistroVirtual.Controllers
{
    public class ClassController : Controller
    {
        public ActionResult Index()
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

            @class.Institutions = institutionsOptions;

            return View(@class);
        }

        public ActionResult Save(ClassModel classModel)
        {
            Class @class = new Class();
            @class.Save(classModel);

            return View("Index");
        }
    }
}