using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressQuiz.ViewModels
{
    public class QuizReviewItem
    {
        public QuizReviewItem(bool isCorrectAnswer, string questionText, int questionId)
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