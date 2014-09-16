namespace ExpressQuiz.ViewModels
{
    public class QuestionReviewViewModel
    {
        public QuestionViewModel Question { get; set; }
        public int QuizResultId { get; set; }
        public int UserAnswerId { get; set; }
    }
}