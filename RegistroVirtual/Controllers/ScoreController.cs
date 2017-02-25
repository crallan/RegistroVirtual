using Domain;
using Models;
using RegistroVirtual.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace RegistroVirtual.Controllers
{
    [SessionAuthorize]
    public class ScoreController : Controller
    {
        public ActionResult Index(bool success = false, bool error = false)
        {
            ScoreViewModel scoreViewModel = new ScoreViewModel();
            List<SelectListItem> classOptions = new List<SelectListItem>();
            List<SelectListItem> trimesterOptions = new List<SelectListItem>();
            List<ClassModel> classes = new Class().GetClassesList().ToList();
            List<TrimesterModel> trimesters = new Trimester().GetTrimesters().ToList();

            foreach (ClassModel @class in classes)
            {
                classOptions.Add(new SelectListItem
                {
                    Text = @class.Name,
                    Value = @class.Id.ToString()
                });
            }

            foreach (TrimesterModel trimester in trimesters)
            {
                trimesterOptions.Add(new SelectListItem
                {
                    Text = trimester.Name,
                    Value = trimester.Id.ToString()
                });
            }

            scoreViewModel.Classes = classOptions;
            scoreViewModel.Trimesters = trimesterOptions;


            if (success)
            {
                ViewBag.SuccessMessage = "El registro de calificaciones se ha guardado de manera exitosa";
            }
            else if (error) {
                 ViewBag.ErrorMessage = "Debido a un error no se ha podido guardar el el registro de calificaciones.";
            }

            return View(scoreViewModel);
        }

        public ActionResult LoadScores(string selectedClass, string selectedYear, string selectedTrimester)
        {
            List<ScoreModel> scores = new List<ScoreModel>();

            try
            {
                List<StudentModel> students = new Student().GetListByClass(Convert.ToInt32(selectedClass)).ToList();
                ClassModel @class = new Class().Get(selectedClass);

                RegisterProfile profileDomain = new RegisterProfile();
                TrimesterModel trimester = new Trimester().Get(selectedTrimester);
                RegisterProfileModel selectedRegisterProfile = new RegisterProfileModel();

                if (trimester.Name.Equals("Primer"))
                {
                    selectedRegisterProfile = profileDomain.Get(@class.FirstTrimesterProfileId.ToString());
                }
                else if (trimester.Name.Equals("Segundo"))
                {
                    selectedRegisterProfile = profileDomain.Get(@class.SecondTrimesterProfileId.ToString());
                }
                else
                {
                    selectedRegisterProfile = profileDomain.Get(@class.ThirdTrimesterProfileId.ToString());
                }

                foreach (StudentModel student in students)
                {
                    ScoreModel scoreEntry = new ScoreModel();
                    scoreEntry.StudentId = student.Id;
                    scoreEntry.StudentName = string.Format("{0} {1}", student.FirstName, student.LastName);

                    scores.Add(scoreEntry);
                }

                List<ScoreModel> currentScores = new Score().GetScores(Convert.ToInt32(selectedClass), Convert.ToInt32(selectedYear)).ToList();

                List<WebGridColumn> columns = new List<WebGridColumn>();
                columns.Add(new WebGridColumn() { ColumnName = "StudentName", Header = "Estudiante", Format = (item) => { return new HtmlString(string.Format("<label id='StudentName'>{0}</label>", item.StudentName)); }, Style = "col3Width" });

                int examIndex = 0;

                selectedRegisterProfile.Exams = new Exams().GetExamsByRegisterProfile(selectedRegisterProfile.Id).ToList();
                selectedRegisterProfile.ExtraclassWorks = new ExtraclassWork().GetExtraclassWorksByRegisterProfile(selectedRegisterProfile.Id).ToList();

                foreach (ExamModel exam in selectedRegisterProfile.Exams)
                {
                    string examScoreFieldId = string.Format("exam-score-{0}", examIndex);
                    string examPercentageFieldId = string.Format("exam-percentage-{0}", examIndex);
                    columns.Add(new WebGridColumn() { Header = string.Format("{0} - {1}pts ", exam.Name, exam.Score), Format = (item) => { return new HtmlString(string.Format("<input type='number' min=0 max={0} id={1} value='0' />", exam.Score, examScoreFieldId)); }, Style = "col1Width" });
                    columns.Add(new WebGridColumn() { Header = string.Format("{0} - {1}%", exam.Name, exam.Percentage), Format = (item) => { return new HtmlString(string.Format("<input type='number' min=0 max={0} id={1} readonly value='0' />", exam.Percentage, examPercentageFieldId)); }, Style = "col1Width" });

                    examIndex++;
                }

                int extraclassIndex = 0;

                foreach (ExtraclassWorkModel extraclass in selectedRegisterProfile.ExtraclassWorks)
                {
                    string extraclassPercentageFieldId = string.Format("exam-percentage-{0}", extraclassIndex);
                    columns.Add(new WebGridColumn() { Header = string.Format("{0} - {1}%", extraclass.Name, extraclass.Percentage), Format = (item) => { return new HtmlString(string.Format("<input type='number' min=0 max={0} id={1} value='0' />", extraclass.Percentage, extraclassPercentageFieldId)); }, Style = "col1Width" });

                    extraclassIndex++;
                }

                columns.Add(new WebGridColumn() { ColumnName = "DailyWorkPercentage", Header = "Trabajo Cotidiano", Format = (item) => { return new HtmlString(string.Format("<input type='number' min=0 max={0} id='DailyWorkPercentage' value='{1}' />", selectedRegisterProfile.DailyWorkPercentage, currentScores.SingleOrDefault( x => x.StudentId.Equals(item.StudentId)) != null ? currentScores.SingleOrDefault(x => x.StudentId.Equals(item.StudentId)).DailyWorkPercentage : 0)); }, Style = "col1Width" });
                columns.Add(new WebGridColumn() { ColumnName = "Belated", Header = "Tardias", Format = (item) => { return new HtmlString(string.Format("<input type='number' id='Belated' min=0  value='{0}' />", currentScores.SingleOrDefault(x => x.StudentId.Equals(item.StudentId)) != null ? currentScores.SingleOrDefault(x => x.StudentId.Equals(item.StudentId)).Belated : 0)); }, Style = "col1Width" });
                columns.Add(new WebGridColumn() { ColumnName = "Absebces", Header = "Ausencias", Format = (item) => { return new HtmlString(string.Format("<input type='number' id='Absebces' min=0 value='{0}' />", currentScores.SingleOrDefault(x => x.StudentId.Equals(item.StudentId)) != null ? currentScores.SingleOrDefault(x => x.StudentId.Equals(item.StudentId)).Absebces : 0)); }, Style = "col1Width" });
                columns.Add(new WebGridColumn() { ColumnName = "AssistancePercentage", Header = "Asistencia", Format = (item) => { return new HtmlString(string.Format("<input type='number' id='AssistancePercentage' readonly value='{1}' />", selectedRegisterProfile.AssistancePercentage, currentScores.SingleOrDefault(x => x.StudentId.Equals(item.StudentId)) != null ? currentScores.SingleOrDefault(x => x.StudentId.Equals(item.StudentId)).AssistancePercentage : 0)); }, Style = "col1Width" });
                columns.Add(new WebGridColumn() { ColumnName = "ConceptPercentage", Header = "Concepto", Format = (item) => { return new HtmlString(string.Format("<input type='number' min=0 max={0} id='ConceptPercentage' value='{1}' />", selectedRegisterProfile.ConceptPercentage, currentScores.SingleOrDefault(x => x.StudentId.Equals(item.StudentId)) != null ? currentScores.SingleOrDefault(x => x.StudentId.Equals(item.StudentId)).ConceptPercentage : 0)); }, Style = "col1Width" });

                ViewBag.Columns = columns;
            }
            catch (Exception) {
                return RedirectToAction("Index", new { error = true });
            }

            return PartialView(scores);
        }
    }
}