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
    public class StudentController : Controller
    {
        public ActionResult Index(string CardIdFilter, string FirstNameFilter, string LastNameFilter, bool success = false)
        {
            List<StudentModel> students = new List<StudentModel>();
            Student student = new Student();
            students = student.GetList().ToList();

            if (!string.IsNullOrEmpty(CardIdFilter))
            {
                students = students.Where(x => !string.IsNullOrEmpty(x.CardId) && x.CardId.ToLowerInvariant().Contains(CardIdFilter.ToLowerInvariant())).ToList();
            }

            if (!string.IsNullOrEmpty(FirstNameFilter))
            {
                students = students.Where(x => x.FirstName.ToLowerInvariant().Contains(FirstNameFilter.ToLowerInvariant())).ToList();
            }

            if (!string.IsNullOrEmpty(LastNameFilter))
            {
                students = students.Where(x => x.LastName.ToLowerInvariant().Contains(LastNameFilter.ToLowerInvariant())).ToList();
            }

            if (success)
            {
                ViewBag.SuccessMessage = "El estudiante se ha guardado de manera exitosa";
            }

            return View(students);
        }

        public ActionResult Create(string id, bool error = false)
        {
            StudentModel student = new StudentModel();
            List<SelectListItem> classOptions = new List<SelectListItem>();
            List<ClassModel> classes = new Class().GetClassesList().ToList();

            foreach (ClassModel @class in classes)
            {
                classOptions.Add(new SelectListItem
                {
                    Text = @class.Name,
                    Value = @class.Id.ToString()
                });
            }

            if (!string.IsNullOrEmpty(id))
            {
                Student studentDomain = new Student();
                student = studentDomain.Get(id);
            }

            student.Classes = classOptions;

            if (error)
            {
                ViewBag.ErrorMessage = "Debido a un error no se ha podido guardar el estudiante.";
            }

            return View(student);
        }

        public ActionResult ImportTask(bool success = false, bool error = false)
        {
            if (success)
            {
                ViewBag.SuccessMessage = "La importación de estudiantes se ha realizado de manera exitosa";
            }
            else if (error)
            {
                ViewBag.ErrorMessage = "Debido a un error no se ha podido realizar la importación de estudiantes. Valide que el archivo contiene la información y estructura correcta.";
            }

            return View();
        }

        public ActionResult Import()
        {
            bool result = false;

            if (Request.Files.Count > 0)
            {
                var importFile = Request.Files[0];

                if (importFile != null && importFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(importFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Imports/"), fileName);
                    importFile.SaveAs(path);

                    ImportModel importModel = new ImportModel();
                    importModel.FilePath = path;
                    importModel.InstitutionId = ((UserModel)Session["User"]).InstitutionId.ToString();

                    Student student = new Student();
                    result = student.Import(importModel);
                }
            }

            if (result)
            {
                return RedirectToAction("ImportTask", new { success = true });
            }
            else
            {
                return RedirectToAction("ImportTask", new { error = true });
            }
        }

        public string DownloadImportTemplate()
        {
            return "/Content/documents/template.xls";
        }

        public ActionResult Save(StudentModel studentModel)
        {
            Student student = new Student();

            if (student.Save(studentModel))
            {
                return RedirectToAction("Index", new { success = true });
            }
            else
            {
                if (!studentModel.Id.Equals(0))
                {
                    return RedirectToAction("Create", new { id = studentModel.Id, error = true });
                }
                else
                {
                    return RedirectToAction("Create", new { error = true });
                }
            } 
        }

    }
}