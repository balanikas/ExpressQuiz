
using System.Collections.Generic;
using System.Linq;

using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;
using ExpressQuiz.Migrations;

using ExpressQuiz.Tests.Repos;

namespace ExpressQuiz.Tests
{
    public class RepoProvider
    {
        private IEnumerable<Quiz> _quizzes = new List<Quiz>();

        private IRepo<Quiz> _quizRepo;
        private IRepo<Question> _questionRepo;
        private IRepo<Answer> _answerRepo;
        private IRepo<QuizCategory> _quizCategoryRepo;
        private IRepo<QuizRating> _quizRatingRepo;
        private IRepo<QuizResult> _quizResultRepo;

        public IRepo<Quiz> QuizRepo
        {
            get { return _quizRepo; }
        }

        public IRepo<Question> QuestionRepo
        {
            get { return _questionRepo; }
        }

        public IRepo<Answer> AnswerRepo
        {
            get { return _answerRepo; }
        }

        public IRepo<QuizCategory> QuizCategoryRepo
        {
            get { return _quizCategoryRepo; }
        }

        public IRepo<QuizRating> QuizRatingRepo
        {
            get { return _quizRatingRepo; }
        }

        public IRepo<QuizResult> QuizResultRepo
        {
            get { return _quizResultRepo; }
        }

        public void Load(string uri)
        {
            _quizzes = DataProvider.Import(uri);

            int quizId = 1, questionId = 1, answerId = 1, quizCatId = 1;


            foreach (var quiz in _quizzes)
            {
                quiz.Category.Id = quizCatId;
                quizCatId++;

                quiz.Id = quizId;


                foreach (var q in quiz.Questions)
                {
                    q.Id = questionId;
                    q.QuizId = quizId;
                    q.Quiz = quiz;
                    foreach (var a in q.Answers)
                    {
                        a.Id = answerId++;
                        a.QuestionId = questionId;
                        a.Question = q;

                    }

                    questionId++;
                }

                quizId++;
            }

            _quizRepo = new InMemRepo<Quiz>(_quizzes.ToList());
            _questionRepo = new InMemRepo<Question>(_quizzes.SelectMany(x => x.Questions).ToList());
            _answerRepo =
                new InMemRepo<Answer>(_quizzes.SelectMany(x => x.Questions).SelectMany(x => x.Answers).ToList());
            _quizCategoryRepo = new InMemRepo<QuizCategory>(_quizzes.Select(x => x.Category).ToList());
            _quizRatingRepo =  new InMemRepo<QuizRating>(new List<QuizRating>());
            _quizResultRepo = new InMemRepo<QuizResult>(new List<QuizResult>());


        }

      

    }
}
