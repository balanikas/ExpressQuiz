using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpressQuiz.Models;

namespace ExpressQuiz.ViewModels
{
    public class QuizzesViewModel
    {
        public List<QuizCategory> QuizCategories { get; set; }

        public List<Quiz> TopQuizzes { get; set; } 

        public List<Quiz> Quizzes { get; set; }
 
    }
}