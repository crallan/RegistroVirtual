using Domain;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RegistroVirtual.Controllers
{
    public class InstitutionController : Controller
    {
        // GET: Institution
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Save(InstitutionModel institutionModel)
        {
            Institution institution = new Institution();
            institution.Save(institutionModel);

            return View("Index");
        }
    }
}