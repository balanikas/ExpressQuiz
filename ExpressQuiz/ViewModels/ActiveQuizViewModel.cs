using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.Expressions;
using ExpressQuiz.Controllers;
using ExpressQuiz.Models;

namespace ExpressQuiz.ViewModels
{
    public class ActiveQuizViewModel
    {
        public Quiz Quiz { get; set; }
        public int EstimatedTime { get; set; }

    }
}