using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Services;
using ExpressQuiz.Tests.Repos;
using ExpressQuiz.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressQuiz.Tests.ViewModels
{
    [TestClass]
    public class QuizDetailsTest : ViewModelTest
    {
        [TestMethod]
        public void ToViewModel()
        {
            var quizResults = new List<QuizResult>();
            quizResults.Add(new QuizResult()
            {
                Answers = new Collection<UserAnswer>()
                {
                    new UserAnswer()
                    {
                        AnswerId = 1,
                        QuestionId = 1,
                       
                    },
                    new UserAnswer()
                    {
                        AnswerId = 1,
                        QuestionId = 2,
                       
                    }
                },
                EllapsedTime = 7,
                Score = 17,
                QuizId = 1
            });
            quizResults.Add(new QuizResult()
            {
                Answers = new Collection<UserAnswer>()
                {
                    new UserAnswer()
                    {
                        AnswerId = 2,
                        QuestionId = 1,
                       
                    },
                    new UserAnswer()
                    {
                        AnswerId = 2,
                        QuestionId = 2,
                       
                    }
                },
                EllapsedTime = 18,
                Score = 15,
                QuizId = 1
            });


            var quizRatings = new List<QuizRating>();
            quizRatings.Add(new QuizRating()
            {
                Level = 90,
                Rating = 60,
                QuizId = 1
            });
            quizRatings.Add(new QuizRating()
            {
                Level = 80,
                Rating = 50,
                QuizId = 1
            });
            var quiz = _repoProvider.QuizRepo.Get(1);

            var quizService = new QuizService(_repoProvider.QuizRepo, _repoProvider.QuizResultRepo,
                _repoProvider.QuizRatingRepo);
            var vm = quiz.ToViewModel(quizService,new InMemRepo<QuizResult>(quizResults), new InMemRepo<QuizRating>(quizRatings));

        }
    }
}
