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

                List<ScoreModel> currentScores = new Score().GetScores(Convert.ToInt32(selectedClass), Convert.ToInt32(selectedYear), Convert.ToInt32(selectedTrimester)).ToList();

                foreach (StudentModel student in students)
                {
                    ScoreModel scoreEntry = currentScores.Where(x => x.StudentId.Equals(student.Id)).Count() > 0 ? currentScores.Where(x => x.StudentId.Equals(student.Id)).First() : new ScoreModel();
                    scoreEntry.StudentId = student.Id;
                    scoreEntry.StudentName = string.Format("{0} {1}", student.FirstName, student.LastName);

                    scores.Add(scoreEntry);
                }

                List<WebGridColumn> columns = new List<WebGridColumn>();
                columns.Add(new WebGridColumn() { ColumnName = "StudentName", Header = "Estudiante", Format = (item) => { return new HtmlString(string.Format("<label id='StudentName'>{0}</label> <input type=hidden id='StudentId' value={1} /> <input type=hidden id='RegisterProfileId' value={2} /> <input type=hidden id='ScoreId' value={3} />", item.StudentName, item.StudentId, selectedRegisterProfile.Id, item.Id)); }, Style = "col3Width" });

                selectedRegisterProfile.Exams = new Exams().GetExamsByRegisterProfile(selectedRegisterProfile.Id).ToList();
                selectedRegisterProfile.ExtraclassWorks = new ExtraclassWork().GetExtraclassWorksByRegisterProfile(selectedRegisterProfile.Id).ToList();

                int examIndex = 0;

                foreach (ExamModel exam in selectedRegisterProfile.Exams)
                {
                    string examPointsFieldId = string.Format("exam-points-{0}", examIndex);
                    string examPercentageFieldId = string.Format("exam-percentage-{0}", examIndex);
                    string examScoreFieldId = string.Format("exam-score-{0}", examIndex);

                    columns.Add(new WebGridColumn() {
                        Header = string.Format("{0} Nota Obt.", exam.Name),
                        Format = (item) => { return new HtmlString(string.Format("<input type='number' class='exam-score' min=0 max=100 id={0} value={1} data-exam-id={2} />", examScoreFieldId, 
                            currentScores.Where(x => x.StudentId.Equals(item.StudentId) && x.ExamResults.Where(t => t.ExamId.Equals(exam.Id)).Count() > 0).Count() > 0 ? currentScores.Where(x => x.StudentId.Equals(item.StudentId)).First().ExamResults.Where(t => t.ExamId.Equals(exam.Id)).FirstOrDefault().ExamScore : 0, exam.Id)); },
                        Style = "col1Width" });

                    columns.Add(new WebGridColumn()
                    {
                        Header = string.Format("{0} - {1}pts", exam.Name, exam.Percentage),
                        Format = (item) => { return new HtmlString(string.Format("<input type='number' min=0 class='exam-points {0}' value={1} max={2} data-exam-id={3} />", examPointsFieldId,
                            currentScores.Where(x => x.StudentId.Equals(item.StudentId) && x.ExamResults.Where(t => t.ExamId.Equals(exam.Id)).Count() > 0).Count() > 0 ? currentScores.Where(x => x.StudentId.Equals(item.StudentId)).First().ExamResults.Where(t => t.ExamId.Equals(exam.Id)).FirstOrDefault().ExamPoints : 0, 
                            exam.Score, exam.Id)); },
                        Style = "col1Width"
                    });

                    columns.Add(new WebGridColumn() {
                        Header = string.Format("{0} - {1}%", exam.Name, exam.Percentage),
                        Format = (item) => { return new HtmlString(string.Format("<input type='number' min=0 class='exam-percentage {0}' value={1} max={2} data-exam-id={3} readonly />", examPercentageFieldId,
                            currentScores.Where(x => x.StudentId.Equals(item.StudentId) && x.ExamResults.Where(t => t.ExamId.Equals(exam.Id)).Count() > 0).Count() > 0 ? currentScores.Where(x => x.StudentId.Equals(item.StudentId)).First().ExamResults.Where(t => t.ExamId.Equals(exam.Id)).FirstOrDefault().ExamPercentage : 0,
                            exam.Percentage, exam.Id )); },
                        Style = "col1Width" });

                    examIndex++;
                }

                int extraclassIndex = 0;

                foreach (ExtraclassWorkModel extraclass in selectedRegisterProfile.ExtraclassWorks)
                {
                    string extraclassPercentageFieldId = string.Format("extraclass-percentage-{0}", extraclassIndex);
                   
                    columns.Add(new WebGridColumn() {
                        Header = string.Format("{0} - {1}%", extraclass.Name, extraclass.Percentage),
                        Format = (item) => { return new HtmlString(string.Format("<input type='number' class='extraclass-score' min=0 max={0} id={1} value={2} data-extraclass-id={3} />", extraclass.Percentage, extraclassPercentageFieldId,
                            currentScores.Where(x => x.StudentId.Equals(item.StudentId) && x.ExtraclasWorkResults.Where(t => t.ExtraclassWorkId.Equals(extraclass.Id)).Count() > 0).Count() > 0 ? currentScores.Where(x => x.StudentId.Equals(item.StudentId)).First().ExtraclasWorkResults.Where(t => t.ExtraclassWorkId.Equals(extraclass.Id)).FirstOrDefault().ExtraclassWorkPercentage : 0, extraclass.Id)); },
                        Style = "col1Width" });

                    extraclassIndex++;
                }

                columns.Add(new WebGridColumn() { ColumnName = "DailyWorkPercentage", Header = "Trabajo Cotidiano", Format = (item) => { return new HtmlString(string.Format("<input type='number' min=0 max={0} id='DailyWorkPercentage' value='{1}' />", selectedRegisterProfile.DailyWorkPercentage, currentScores.Where( x => x.StudentId.Equals(item.StudentId)).Count() > 0 ? currentScores.FirstOrDefault(x => x.StudentId.Equals(item.StudentId)).DailyWorkPercentage : 0)); }, Style = "col1Width" });
                columns.Add(new WebGridColumn() { ColumnName = "Belated", Header = "Tardias", Format = (item) => { return new HtmlString(string.Format("<input class='assistance-related-field' type='number' id='Belated' min=0  value='{0}' />", currentScores.Where(x => x.StudentId.Equals(item.StudentId)).Count() > 0 ? currentScores.FirstOrDefault(x => x.StudentId.Equals(item.StudentId)).Belated : 0)); }, Style = "col1Width" });
                columns.Add(new WebGridColumn() { ColumnName = "Absences", Header = "Ausencias", Format = (item) => { return new HtmlString(string.Format("<input class='assistance-related-field' type='number' id='Absences' min=0 value='{0}' />", currentScores.Where(x => x.StudentId.Equals(item.StudentId)).Count() > 0 ? currentScores.FirstOrDefault(x => x.StudentId.Equals(item.StudentId)).Absences : 0)); }, Style = "col1Width" });
                columns.Add(new WebGridColumn() { ColumnName = "AssistancePercentage", Header = "Asistencia", Format = (item) => { return new HtmlString(string.Format("<input type=hidden id='NumberOfLessons' value={0} /> <input type='number' id='AssistancePercentage' readonly max={1} value='{2}' />", selectedRegisterProfile.NumberOfLessons, selectedRegisterProfile.AssistancePercentage, currentScores.Where(x => x.StudentId.Equals(item.StudentId)).Count() > 0 ? currentScores.FirstOrDefault(x => x.StudentId.Equals(item.StudentId)).AssistancePercentage : selectedRegisterProfile.AssistancePercentage)); }, Style = "col1Width" });
                columns.Add(new WebGridColumn() { ColumnName = "ConceptPercentage", Header = "Concepto", Format = (item) => { return new HtmlString(string.Format("<input type='number' min=0 max={0} id='ConceptPercentage' value='{1}' />", selectedRegisterProfile.ConceptPercentage, currentScores.Where(x => x.StudentId.Equals(item.StudentId)).Count() > 0 ? currentScores.FirstOrDefault(x => x.StudentId.Equals(item.StudentId)).ConceptPercentage : 0)); }, Style = "col1Width" });

                ViewBag.Columns = columns;
            }
            catch (Exception ex) {
                return Redirect("/Score?error=true");
            }

            return PartialView(scores);
        }

        public ActionResult Save(List<ScoreModel> scoreList)
        {
            Score scoreDomain = new Score();

            if (scoreDomain.Save(scoreList))
            {
                return RedirectToAction("Index", new { success = true });
            }
            else
            {
                return RedirectToAction("Index", new { error = true });
            }
        }
    }
}