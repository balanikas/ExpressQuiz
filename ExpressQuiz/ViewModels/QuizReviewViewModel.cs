using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using ExpressQuiz.Models;
using ExpressQuiz.Repos;

namespace ExpressQuiz.ViewModels
{
    

    public class QuizReviewViewModel
    {
        QuizDbContext db = new QuizDbContext();
        

        public QuizResult Result { get; private set; }
        public QuizReviewViewModel(QuizResult result)
        {
            Result = result;
            
        }


        public int GetScore()
        {
            return CalculateScore();
        }

        public List<QuestionDetails> GetQuestionDetails()
        {
            

            var qDetails = new List<QuestionDetails>();
            foreach (var userAnswer in Result.Answers)
            {
                var answer = db.Answers.FirstOrDefault(x => x.Id == userAnswer.AnswerId);
                var isAnswerCorrect = answer != null ? answer.IsCorrect : false;
                var questionText = db.Questions.First(x => x.Id == userAnswer.QuestionId).Text;

                qDetails.Add(new QuestionDetails(isAnswerCorrect, questionText, userAnswer.QuestionId));
              
                
            }

            return qDetails;
        }

        private int CalculateScore()
        {

            int count = 0;
            foreach (var userAnswer in Result.Answers)
            {
                var correctAnswer = db.Answers.FirstOrDefault(x => x.Id == userAnswer.AnswerId);
                if (correctAnswer != null && correctAnswer.IsCorrect)
                {
                    count++;
                }
                
            }
            return (int)((double)count / Result.Answers.Count) * 100;
        }

        public class QuestionDetails
        {
            public QuestionDetails(bool isCorrectAnswer, string questionText, int questionId)
            {
                IsCorrectAnswer = isCorrectAnswer;
                QuestionText = questionText;
                QuestionId = questionId;
            }

            public bool IsCorrectAnswer { get; private set; }
            public string QuestionText { get; private set; }

            public int QuestionId { get; private set; }
        }
    }
}