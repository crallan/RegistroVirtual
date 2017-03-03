﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models
{
    public class ScoreViewModel
    {
        public int ClassId { get; set; }
        public int TrimesterId { get; set; }
        public List<SelectListItem> Classes { get; set; }
        public List<SelectListItem> Trimesters { get; set; }
        public SelectList Years { get; set; }

        public ScoreViewModel()
        {
            Years = new SelectList(Enumerable.Range(2017, (DateTime.Now.Year - 2017) + 1));
            Classes = new List<SelectListItem>();
            Trimesters = new List<SelectListItem>();
        }
    }
}