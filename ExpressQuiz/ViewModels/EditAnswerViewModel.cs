namespace ExpressQuiz.ViewModels
{
    public class EditAnswerViewModel
    {
        public AnswerViewModel Answer { get; set; }
        public int QuestionId { get; set; }
        public int QuizId { get; set; }
    }
}