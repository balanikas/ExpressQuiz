using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using ExpressQuiz.Models;

namespace ExpressQuiz.ViewModels
{


    public class QuestionReviewViewModel
    {
        QuizDbContext db = new QuizDbContext();


        public Question Question { get; private set; }
        public int QuizResultId { get; private set; }
        public QuestionReviewViewModel(Question q, int id)
        {
            Question = q;
            QuizResultId = id;
        }



    }
}