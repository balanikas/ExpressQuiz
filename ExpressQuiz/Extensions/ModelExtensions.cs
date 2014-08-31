using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;
using ExpressQuiz.Core.Services;
using ExpressQuiz.Extensions;
using ExpressQuiz.ViewModels;


namespace ExpressQuiz.Extensions
{
    public static class ModelExtensions
    {
        public static QuizDetailsViewModel ToViewModel(this Quiz quiz, IQuizResultService quizResultService, IRepo<QuizRating> quizRatings)
        {
            var vm = new QuizDetailsViewModel();
            
            vm.AvgLevel = quizResultService.GetAverageLevel(quiz.Id);
            vm.AvgRating = quizResultService.GetAverageRating(quiz.Id);
            vm.AvgScore = quizResultService.GetAverageScore(quiz.Id);
            vm.AvgTime = quizResultService.GetAverageTime(quiz.Id);
            vm.AvgTimePercent = quizResultService.GetAverageTimePercent(quiz.Id);
            
            vm.Quiz = quiz;

            return vm;
        }

        public static List<QuizCategoryViewModel> ToViewModel(this IQueryable<QuizCategory> categories,IQueryable<Quiz> quizzes, int? catId)
        {
            var cats = (from c in categories
                                 orderby c.Name
                                 select new QuizCategoryViewModel()
                                 {
                                     Id = c.Id,
                                     Name = c.Name,
                                     IsSelected = catId.HasValue ? (c.Id == catId.Value) : false,
                                     QuizCount = quizzes.Count(x => x.Category.Id == c.Id)
                                 }).ToList();

            cats.Insert(0, new QuizCategoryViewModel()
            {
                Id = -1,
                Name = "All",
                IsSelected = catId.HasValue ? (-1 == catId.Value) : true,
                QuizCount = quizzes.Count()
            });

            return cats;
        }

        public static QuizzesViewModel ToViewModel(this IQueryable<Quiz> quizzes, 
            IQuizService quizService,
            IQuizCategoryService categories, 
            int? catId)
        {
            var vm = new QuizzesViewModel();
            
            vm.QuizCategories = categories.GetAll().ToViewModel(quizService.GetPublicQuizzes(), catId);

            vm.Filter = QuizFilter.Newest;
            vm.Quizzes = quizzes.ToList();

            vm.TopQuizzes = quizService.GetBy(QuizFilter.Rating, descending: true, count: 10).Select(x => new TopListItem()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            vm.SelectedCategory = catId.HasValue ? catId.Value : -1;

            return vm;
        }

        public static EditQuizViewModel ToViewModel(this Quiz quiz, IQuizCategoryService categories)
        {
            var vm = new EditQuizViewModel();


            vm.Quiz = quiz;

            var sortedQuestions = quiz.Questions.AsQueryable().AsNoTracking().OrderBy(x => x.OrderId).Select(x => x.Id);
            vm.Order = string.Join(",", sortedQuestions);
            vm.Categories = categories.GetCategoriesAsSelectList();
            vm.SelectedCategory = quiz.Category.Id;

            vm.EstimatedTime = (uint)quiz.Questions.Sum(x => x.EstimatedTime);

            return vm;
        }

        public static EditQuestionViewModel ToViewModel(this Question question)
        {
            var vm = new EditQuestionViewModel();

            vm.Question = question;
            vm.Order = string.Join(",", question.Answers.AsQueryable().AsNoTracking().OrderBy(x => x.OrderId).Select(x => x.Id));
       

            return vm;
        }

        public static ActiveQuizViewModel ToViewModel(this Quiz quiz)
        {
            var vm = new ActiveQuizViewModel();
            vm.Quiz = quiz;
            vm.EstimatedTime = quiz.Questions.Sum(x => x.EstimatedTime);

            return vm;
        }

        public static QuestionReviewViewModel ToViewModel(this Question question,int resultId, int userAnswerId )
        {
            var vm = new QuestionReviewViewModel();
            vm.Question = question;
            vm.QuizResultId = resultId;
            vm.UserAnswerId = userAnswerId;
            return vm;
        }

        public static QuizReviewViewModel ToViewModel(this QuizResult quizResult, IQuizService quizzes, IAnswerService answers,
            IQuizResultService quizResultService)
        {
            var vm = new QuizReviewViewModel();
            var quiz = quizzes.Get(quizResult.QuizId);
            var questions = quiz.Questions.Where(x => x.QuizId == quizResult.QuizId);



            var qDetails = new List<QuizReviewItemViewModel>();
            foreach (var userAnswer in quizResult.Answers)
            {
                var answer = answers.GetAll().FirstOrDefault(x => x.Id == userAnswer.AnswerId);
                var isAnswerCorrect = answer != null ? answer.IsCorrect : false;
                var question = questions.First(x => x.Id == userAnswer.QuestionId);
                var questionText = question.Text;
                var pointsEarned = isAnswerCorrect ? question.Points : 0;
                qDetails.Add(new QuizReviewItemViewModel()
                {
                    IsCorrectAnswer = isAnswerCorrect,
                    QuestionId = userAnswer.QuestionId,
                    QuestionText = questionText,
                    PointsEarned = pointsEarned,
                    
                });
            }

           
            var allowPoints = quiz.AllowPoints;
            string scoreText;
            if (allowPoints)
            {
                var totalPoints = questions.Sum(x => x.Points);
                var scoredPoints = quizResult.Score*totalPoints/100;
                scoreText = scoredPoints + " / " + totalPoints + " points";
            }
            else
            {
                scoreText = quizResult.Score + "%";
            }

            vm.Items = qDetails;
            vm.Result = quizResult;
            vm.ScoreText = scoreText;
            vm.EllapsedTimePercent = (int)((double)quizResult.EllapsedTime/(double)questions.Sum(x => x.EstimatedTime)*100);
            vm.QuizId = quizResult.QuizId;

            
            vm.RelativeScore = quizResultService.GetRelativeScore(quiz.Id, quizResult.Score);
            vm.RelativeTime = quizResultService.GetRelativeTime(quiz.Id, quizResult.EllapsedTime);
            return vm;
        }


        public static CreateQuizViewModel ToViewModel(this Quiz quiz, IQuizCategoryService categories, string userName)
        {
            var vm = new CreateQuizViewModel();
            vm.Categories = categories.GetCategoriesAsSelectList();
            vm.Quiz = new Quiz();
            vm.Quiz.CreatedBy = userName;

            return vm;
        }
    }
}