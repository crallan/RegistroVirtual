using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models
{
    public class RegisterProfileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float DailyWorkPercentage { get; set; }
        public float ConceptPercentage { get; set; }
        public float AssistancePercentage { get; set; }
        public int TrimesterId { get; set; }
        public int SchoolYearId { get; set; }
        public int SubjectId { get; set; }
        public int UserId { get; set; }
        public int NumberOfLessons { get; set; }
        public List<SelectListItem> Trimesters { get; set; }
        public List<SelectListItem> SchoolYears { get; set; }
        public List<SelectListItem> Subjects { get; set; }

        public List<ExamModel> Exams { get; set; }
        public List<ExtraclassWorkModel> ExtraclassWorks { get; set; }

        public RegisterProfileModel()
        {
            Trimesters = new List<SelectListItem>();
            SchoolYears = new List<SelectListItem>();

            Exams = new List<ExamModel>();
            ExtraclassWorks = new List<ExtraclassWorkModel>();
        }

        public RegisterProfileModel(string Name, float DailyWorkPercentage, float ConceptPercentage, float AssistancePercentage, int TrimesterId, int SchoolYearId, int NumberOfLessons)
        {
            Trimesters = new List<SelectListItem>();
            SchoolYears = new List<SelectListItem>();

            Exams = new List<ExamModel>();
            ExtraclassWorks = new List<ExtraclassWorkModel>();

            this.Name = Name;
            this.DailyWorkPercentage = DailyWorkPercentage;
            this.ConceptPercentage = ConceptPercentage;
            this.AssistancePercentage = AssistancePercentage;
            this.TrimesterId = TrimesterId;
            this.SchoolYearId = SchoolYearId;
            this.NumberOfLessons = NumberOfLessons;
        }

    }
}
