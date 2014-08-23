using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;
using ExpressQuiz.Core.Services;
using ExpressQuiz.Migrations;
using ExpressQuiz.Tests.Repos;

namespace ExpressQuiz.Tests
{
    public class MockRepository
    {
        private IEnumerable<Quiz> _quizzes;
        private string _uri;

        public IRepo<Quiz> QuizRepo { get; private set; }
        public IRepo<Question> QuestionRepo { get; private set; }
        public IRepo<Answer> AnswerRepo { get; private set; }
        public IRepo<QuizCategory> QuizCategoryRepo { get; private set; }
        public IRepo<QuizRating> QuizRatingRepo { get; private set; }
        public IRepo<QuizResult> QuizResultRepo { get; private set; }
        public IRepo<ContactInfo> ContactInfoRepo { get; private set; }
        public IQuizService QuizService { get; private set; }
        public IQuestionService QuestionService { get; private set; }
        public IAnswerService AnswerService { get; private set; }
        public IQuizCategoryService QuizCategoryService { get; private set; }
        


        public MockRepository(string uri)
        {
            _uri = uri;
            Reload();
            CreateRepos();
            CreateServices();
        }

        private void CreateRepos( )
        {
            QuizRepo = new InMemRepo<Quiz>(_quizzes.ToList());
            QuestionRepo = new InMemRepo<Question>(_quizzes.SelectMany(x => x.Questions).ToList());
            AnswerRepo =
                new InMemRepo<Answer>(_quizzes.SelectMany(x => x.Questions).SelectMany(x => x.Answers).ToList());
            QuizCategoryRepo = new InMemRepo<QuizCategory>(_quizzes.Select(x => x.Category).ToList());




            var ratings = new List<QuizRating>()
            {
                new QuizRating() {Id = 1, Level = 50, QuizId = 1, Rating = 80},
                new QuizRating() {Id = 2, Level = 30, QuizId = 1, Rating = 30},
                new QuizRating() {Id = 3, Level = 80, QuizId = 2, Rating = 50},
                new QuizRating() {Id = 4, Level = 30, QuizId = 2, Rating = 30}
            };

            var result1 = new QuizResult()
            {
                Answers = new List<UserAnswer>()
                {
                    new UserAnswer() {Id = 1, AnswerId = 1, QuestionId = 1},
                    new UserAnswer() {Id = 2, AnswerId = 2, QuestionId = 2},
                },
                EllapsedTime = 10,
                Score = 50,
                QuizId = 1
            };
            var result2 = new QuizResult()
            {
                Answers = new List<UserAnswer>()
                {
                    new UserAnswer() {Id = 1, AnswerId = 2, QuestionId = 1},
                    new UserAnswer() {Id = 2, AnswerId = 2, QuestionId = 2},
                },
                EllapsedTime = 20,
                Score = 80,
                QuizId = 1
            };

            var results = new List<QuizResult>() {result1, result2};

            QuizRatingRepo = new InMemRepo<QuizRating>(ratings);
            QuizResultRepo = new InMemRepo<QuizResult>(results);
            ContactInfoRepo = new InMemRepo<ContactInfo>(new List<ContactInfo>());
        }

        private void CreateServices()
        {
            QuizService = new QuizService(QuizRepo,QuizResultRepo,QuizRatingRepo);
            QuestionService = new QuestionService(QuestionRepo);
            AnswerService = new AnswerService(AnswerRepo);
            QuizCategoryService = new QuizCategoryService(QuizCategoryRepo);
        }

        public void Reload()
        {
            _quizzes = DataProvider.Import(_uri);

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

           


        }
    }
}
