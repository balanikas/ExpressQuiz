namespace ExpressQuiz.Models
{
    public class UserAnswer
    {
        public int ID { get; set; }
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public int QuizResultId { get; set; }
        public virtual QuizResult QuizResult { get; set; }
    }
}