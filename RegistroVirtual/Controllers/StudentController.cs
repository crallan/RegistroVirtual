using Domain;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RegistroVirtual.Controllers
{
    public class StudentController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Save(StudentModel studentModel)
        {
            Student student = new Student();
            studentModel.ClassId = 1;
            student.Save(studentModel);

            return View("Index");
        }

    }
}