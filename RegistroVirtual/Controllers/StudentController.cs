using Domain;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RegistroVirtual.Controllers
{
    public class StudentController : Controller
    {
        public ActionResult Index()
        {
            StudentModel student = new StudentModel();
            List<SelectListItem> classOptions = new List<SelectListItem>();
            List<ClassModel> classes = new Class().GetClassesList().ToList();

            foreach (ClassModel @class in classes) {
                classOptions.Add(new SelectListItem
                {
                    Text = @class.Name,
                    Value = @class.Id.ToString()
                });
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

            return View();
        }

        public ActionResult Save(StudentModel studentModel)
        {
            Student student = new Student();
            student.Save(studentModel);

            return View("Index");
        }

    }
}