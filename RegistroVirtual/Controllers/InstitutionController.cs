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
        public ActionResult Index(string NameFilter)
        {
            List<InstitutionModel> institutions = new List<InstitutionModel>();
            Institution institution = new Institution();
            institutions = institution.GetInstitutionsList().ToList();

            if (!string.IsNullOrEmpty(NameFilter))
            {
                institutions = institutions.Where(x => x.Name.ToLowerInvariant().Contains(NameFilter.ToLowerInvariant())).ToList();
            }

            return View(institutions);
        }

        public ActionResult Create(string id)
        {
            InstitutionModel institution = new InstitutionModel();

            if (!string.IsNullOrEmpty(id))
            {
                Institution institutionDomain = new Institution();
                institution = institutionDomain.Get(id);
            }

            return View(institution);
        }

        public ActionResult Save(InstitutionModel institutionModel)
        {
            Institution institution = new Institution();

            if (institution.Save(institutionModel))
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