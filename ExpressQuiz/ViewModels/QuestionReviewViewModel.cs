﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using ExpressQuiz.Models;

namespace ExpressQuiz.ViewModels
{


    public class QuestionReviewViewModel
    {
       
        public Question Question { get;  set; }
        public int QuizResultId { get;  set; }
       
    }
}