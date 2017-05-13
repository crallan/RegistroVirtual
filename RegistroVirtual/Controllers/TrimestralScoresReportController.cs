using Domain;
using Models;
using RegistroVirtual.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace RegistroVirtual.Controllers
{
    [SessionAuthorize]
    public class TrimestralScoresReportController : Controller
    {
        public ActionResult Index(bool success = false, bool error = false)
        {
            ScoreViewModel scoreViewModel = new ScoreViewModel();
            List<SelectListItem> classOptions = new List<SelectListItem>();
            List<SelectListItem> trimesterOptions = new List<SelectListItem>();
            List<TrimesterModel> trimesters = new Trimester().GetTrimesters().ToList();
            List<SelectListItem> subjectOptions = new List<SelectListItem>();
            UserModel contextUser = new User().GetUserByUsername(((UserModel)Session["User"]).Username);

            foreach (ClassesBySubjectModel classesBySubject in contextUser.RelatedSubjectsAndClasses)
            {
                SelectListItem subjectOption = new SelectListItem
                {
                    Text = classesBySubject.Subject.Name,
                    Value = classesBySubject.Subject.Id.ToString()
                };

                if(subjectOptions.Where( x=> x.Value.ToString() == subjectOption.Value.ToString()).Count() == 0){
                    subjectOptions.Add(subjectOption);
                }

                foreach (ClassModel @class in classesBySubject.SelectedClasses)
                {
                    SelectListItem classOption = new SelectListItem
                    {
                        Text = @class.Name,
                        Value = @class.Id.ToString()
                    };

                    if(classOptions.Where( x => x.Value.Equals(@class.Id.ToString())).Count() == 0)
                    {
                        classOptions.Add(classOption);
                    }
                }
            }

            foreach (TrimesterModel trimester in trimesters)
            {
                trimesterOptions.Add(new SelectListItem
                {
                    Text = trimester.Name,
                    Value = trimester.Id.ToString()
                });
            }

            scoreViewModel.Subjects = subjectOptions;
            scoreViewModel.Classes = classOptions;
            scoreViewModel.Trimesters = trimesterOptions;


            if (success)
            {
                ViewBag.SuccessMessage = "El registro de calificaciones trimestral se ha generado de manera exitosa";
            }
            else if (error) {
                 ViewBag.ErrorMessage = "Debido a un error no se ha podido generar el registro de calificaciones trimestral.";
            }

            return View(scoreViewModel);
        }

        public JsonResult GetRelatedClasses(int selectedSubject)
        {
            UserModel contextUser = new User().GetUserByUsername(((UserModel)Session["User"]).Username);
            List<ClassModel> relatedClasses = new List<ClassModel>();
            List<ClassesBySubjectModel> classesBySubjectList = contextUser.RelatedSubjectsAndClasses.Where(x => x.Subject.Id == selectedSubject).ToList();

            if(classesBySubjectList != null && classesBySubjectList.Count() > 0)
            {
                foreach (ClassesBySubjectModel classesBySubject in classesBySubjectList)
                {
                    foreach (ClassModel @class in classesBySubject.SelectedClasses)
                    {
                        if (relatedClasses.Where(x => x.Id.Equals(@class.Id.ToString())).Count() == 0)
                        {
                            relatedClasses.Add(@class);
                        }
                    }
                }
            }

            return Json(new { classes = relatedClasses }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GeneratePDF(string selectedClass, string selectedYear, string selectedTrimester, string selectedSubject)
        {
            try
            {
                FileResult result = null;
                var generator = new NReco.PdfGenerator.HtmlToPdfConverter();
                StringBuilder htmlBuilder = new StringBuilder();

                List<StudentModel> students = new Student().GetListByClass(Convert.ToInt32(selectedClass)).ToList();
                ClassModel @class = new Class().Get(selectedClass);

                RegisterProfile profileDomain = new RegisterProfile();
                TrimesterModel trimester = new Trimester().Get(selectedTrimester);

                int classId = Convert.ToInt32(selectedClass);
                int year = Convert.ToInt32(selectedYear);
                int trimesterId = Convert.ToInt32(selectedTrimester);
                int subject = Convert.ToInt32(selectedSubject);

                RegisterProfileModel selectedRegisterProfile = profileDomain.GetProfile(@class.SchoolYearId, year, trimesterId, subject);
                List<ScoreModel> currentScores = new Score().GetScores(classId, year, trimesterId, subject).ToList();
                selectedRegisterProfile.Exams = new Exams().GetExamsByRegisterProfile(selectedRegisterProfile.Id).ToList();
                selectedRegisterProfile.ExtraclassWorks = new ExtraclassWork().GetExtraclassWorksByRegisterProfile(selectedRegisterProfile.Id).ToList();
                
                foreach (ScoreModel score in currentScores)
                {
                    float average = 0;
                    StudentModel student = students.Where(x => x.Id.ToString().Equals(score.StudentId.ToString())).FirstOrDefault();
                    if (student != null)
                    {
                        StringBuilder headerBuilder = new StringBuilder();
                        StringBuilder bodyBuilder = new StringBuilder();
                        string columnStyle = "border: 1px solid #2b5569;text-align:center;font-size: 11px;page-break-inside: avoid;";

                        htmlBuilder.Append("<table class='table table-responsive scores-table webGrid' style='margin-top:10px;width:100%;'>");
                        headerBuilder.Append("<thead>");
                        headerBuilder.Append("<tr class='header' style='background-color: #337ab7;font-size: 14px;color: #fff;page-break-inside: avoid;'>");
                        bodyBuilder.Append("<tbody>");
                        bodyBuilder.Append("<tr class='score-item' style='page-break-inside: avoid;'>");

                        headerBuilder.AppendFormat("<th scope='col' style='{0}'>{1}</th>", columnStyle, "Estudiante");
                        bodyBuilder.AppendFormat("<td class='col2Width' style='{0}'>{1} {2}</td>", columnStyle, student.FirstName, student.LastName);

                        foreach (ExamModel exam in selectedRegisterProfile.Exams)
                        {
                            float examResult = score.ExamResults.Where(e => e.ExamId.Equals(exam.Id)).FirstOrDefault().ExamPercentage;
                            average += examResult;
                            headerBuilder.AppendFormat("<th scope='col' style='{0}'> {1} - {2}%</th>", columnStyle, exam.Name, exam.Percentage);
                            bodyBuilder.AppendFormat("<td class='col1Width' style='{0}'>{1}</td>", columnStyle, examResult);
                        }

                        foreach (ExtraclassWorkModel extraclass in selectedRegisterProfile.ExtraclassWorks)
                        {
                            float extraclassResult = score.ExtraclasWorkResults.Where(e => e.ExtraclassWorkId.Equals(extraclass.Id)).FirstOrDefault().ExtraclassWorkPercentage;
                            average += extraclassResult;
                            headerBuilder.AppendFormat("<th scope='col' style='{0}'>{1} - {2}%</th>", columnStyle, extraclass.Name, extraclass.Percentage);
                            bodyBuilder.AppendFormat("<td class='col1Width' style='{0}'>{1}</td>", columnStyle, extraclassResult);
                        }

                        average += score.DailyWorkPercentage;
                        headerBuilder.AppendFormat("<th scope='col' style='{0}'>{1}</th>", columnStyle, "Trabajo Cotidiano");
                        bodyBuilder.AppendFormat("<td class='col1Width' style='{0}'>{1}</td>", columnStyle, score.DailyWorkPercentage);

                        average += score.AssistancePercentage;
                        headerBuilder.AppendFormat("<th scope='col' style='{0}'>{1}</th>", columnStyle, "Asistencia");
                        bodyBuilder.AppendFormat("<td class='col1Width' style='{0}'>{1}</td>", columnStyle, score.AssistancePercentage);

                        average += score.ConceptPercentage;
                        headerBuilder.AppendFormat("<th scope='col' style='{0}'>{1}</th>", columnStyle, "Concepto");
                        bodyBuilder.AppendFormat("<td class='col1Width' style='{0}'>{1}</td>", columnStyle, score.ConceptPercentage);

                        headerBuilder.AppendFormat("<th scope='col' style='{0}'>{1}</th>", columnStyle, "Promedio");
                        bodyBuilder.AppendFormat("<td class='col1Width' style='{0}'>{1}</td>", columnStyle, average);

                        bodyBuilder.Append("</tr>");
                        bodyBuilder.Append("</tbody>");
                        headerBuilder.Append("</tr>");
                        headerBuilder.Append("</thead>");

                        //Concat header and body
                        htmlBuilder.Append(headerBuilder);
                        htmlBuilder.Append(bodyBuilder);

                        htmlBuilder.Append("</table>");
                    }
                }
                
                var pdfBytes = generator.GeneratePdf(htmlBuilder.ToString());

                string fileName = string.Format("Reporte-Calificaciones-Trimestrales-{0}.pdf", @class.Name.Replace(" ", ""));
                HttpContext.Response.ContentType = "application/pdf";
                HttpContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
                //HttpContext.Response.AppendHeader("content-disposition", string.Format("attachment; filename='{0}'", fileName));
                result = new FileContentResult(pdfBytes, "application/pdf");
                result.FileDownloadName = fileName;

                var uri = Request.Url.AbsoluteUri;

                return result;
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { error = true });
            }
        }
    }
}