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
    public class InstitutionController : Controller
    {
        public ActionResult Index(string NameFilter, bool success = false)
        {
            List<InstitutionModel> institutions = new List<InstitutionModel>();
            Institution institution = new Institution();
            institutions = institution.GetInstitutionsList().ToList();

            if (!string.IsNullOrEmpty(NameFilter))
            {
                institutions = institutions.Where(x => x.Name.ToLowerInvariant().Contains(NameFilter.ToLowerInvariant())).ToList();
            }

            if (success)
            {
                ViewBag.SuccessMessage = "La institucion se ha guardado de manera exitosa";
            }

            return View(institutions);
        }

        public ActionResult Create(string id, bool error = false)
        {
            InstitutionModel institution = new InstitutionModel();

            if (!string.IsNullOrEmpty(id))
            {
                Institution institutionDomain = new Institution();
                institution = institutionDomain.Get(id);
            }

            if (error)
            {
                ViewBag.ErrorMessage = "Debido a un error no se ha podido guardar el institucion.";
            }
            
            return View(institution);
        }

        public ActionResult Save(InstitutionModel institutionModel)
        {
            Institution institution = new Institution();

            if (institution.Save(institutionModel))
            {
                return RedirectToAction("Index", new { success = true });
            }
            else
            {
                if (!institutionModel.Id.Equals(0))
                {
                    return RedirectToAction("Create", new { id = institutionModel.Id, error = true });
                }
                else
                {
                    return RedirectToAction("Create", new { error = true });
                }
            }
        }

    }
}