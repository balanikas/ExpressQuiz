using ExpressQuiz.Core.Models;

namespace ExpressQuiz.Extensions
{
    public static class ModelExtensions
    {
        public static void SetDefaultValues(this Question question)
        {
            question.Text = "enter question here";
            question.EstimatedTime = 10;
        }

        public static void SetDefaultValues(this Answer answer)
        {
            answer.Text = "enter answer here";
        }
    }
}