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
        public ActionResult Index(string FirstNameFilter, string LastNameFilter)
        {
            List<StudentModel> students = new List<StudentModel>();
            Student student = new Student();
            students = student.GetList().ToList();

            if (!string.IsNullOrEmpty(FirstNameFilter))
            {
                students = students.Where(x => x.FirstName.ToLowerInvariant().Contains(FirstNameFilter.ToLowerInvariant())).ToList();
            }

            if (!string.IsNullOrEmpty(LastNameFilter))
            {
                students = students.Where(x => x.LastName.ToLowerInvariant().Contains(LastNameFilter.ToLowerInvariant())).ToList();
            }

            return View(students);
        }

        public ActionResult Create(string id)
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

            return View(student);
        }

        public ActionResult ImportTask()
        {
            return View();
        }

        public ActionResult Import()
        {
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

                    Student student = new Student();
                    student.Import(importModel);
                }
            }

            return View("ImportTask");
        }

        public ActionResult Save(StudentModel studentModel)
        {
            Student student = new Student();

            if (student.Save(studentModel))
            {
                return RedirectToAction("Index");
            }
            else {
                return RedirectToAction("Create");
            } 
        }

    }
}