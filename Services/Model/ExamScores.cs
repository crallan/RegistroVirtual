//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Services.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ExamScores
    {
        public int Id { get; set; }
        public double ExamScore { get; set; }
        public double ExamPercentage { get; set; }
    
        public virtual Scores Scores { get; set; }
        public virtual Exams Exams { get; set; }
    }
}