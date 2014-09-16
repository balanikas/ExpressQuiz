namespace ExpressQuiz.ViewModels
{
    public class EditQuestionViewModel
    {
        public QuestionViewModel Question { get; set; }
        public string Order { get; set; }
        public int QuizId { get; set; }
        public bool AllowPoints { get; set; }
        public bool IsTimeable { get; set; }
    }
}