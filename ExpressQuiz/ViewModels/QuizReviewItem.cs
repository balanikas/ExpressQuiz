using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DisplayName("Is correct")]
        public bool IsCorrectAnswer { get; private set; }

        [DisplayName("Question")]
        public string QuestionText { get; private set; }

        public int QuestionId { get; private set; }
    }
}