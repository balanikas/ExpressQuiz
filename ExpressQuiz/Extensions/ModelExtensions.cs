using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;
using ExpressQuiz.Core.Services;
using ExpressQuiz.ViewModels;
using WebGrease.Css.Extensions;


namespace ExpressQuiz.Extensions
{
    public static class ModelExtensions
    {

        public static QuizViewModel ToQuizViewModel(this Quiz quiz)
        {
            var vm = new QuizViewModel()
            {
                AllowPoints = quiz.AllowPoints,
                Created = quiz.Created,
                IsTimeable = quiz.IsTimeable,
                CreatedBy = quiz.CreatedBy,
                Locked = quiz.Locked,
                Name = quiz.Name,
                QuizId = quiz.Id,
                Summary = quiz.Summary,
                Category = new QuizCategoryViewModel()
                {
                    Id = quiz.Category.Id,
                    Name = quiz.Category.Name
                },
                Questions = quiz.Questions.ToList().Select(x => x.ToQuestionViewModel()).ToList()
            };

            return vm;
        }

        public static QuestionViewModel ToQuestionViewModel(this Question question)
        {
            var vm = new QuestionViewModel()
            {
                Answers = question.Answers.ToList().Select(x => x.ToAnswerViewModel()).ToList(),
                EstimatedTime = question.EstimatedTime,
                OrderId = question.OrderId,
                Points = question.Points,
                Text = question.Text,
                QuestionId = question.Id
            };

            return vm;
        }

        public static AnswerViewModel ToAnswerViewModel(this Answer answer)
        {
            var vm = new AnswerViewModel
            {
                AnswerId = answer.Id,
                Explanation = answer.Explanation,
                IsCorrect = answer.IsCorrect,
                OrderId = answer.OrderId,
                Text = answer.Text,
            };

            return vm;
        }

        public static QuizResultViewModel ToQuizResultViewModel(this QuizResult result)
        {
            var vm = new QuizResultViewModel()
            {
                EllapsedTime = result.EllapsedTime,
                Score = result.Score,
                UserId = result.UserId,

            };

            return vm;
        }

        public static QuizDetailsViewModel ToQuizDetailsViewModel(this Quiz quiz, IQuizResultService quizResultService, IRepo<QuizRating> quizRatings)
        {
            var vm = new QuizDetailsViewModel();
            
            vm.AvgLevel = quizResultService.GetAverageLevel(quiz.Id);
            vm.AvgRating = quizResultService.GetAverageRating(quiz.Id);

            vm.AvgScore = quizResultService.GetAverageScore(quiz.Id);
            vm.TotalPoints = quiz.Questions.Sum(x => x.Points);
            vm.AvgScorePercent = quizResultService.GetAverageScorePercent(quiz.Id);

            vm.AvgTime = quizResultService.GetAverageTime(quiz.Id);
            vm.AvgTimePercent = quizResultService.GetAverageTimePercent(quiz.Id);
            vm.TotalTime = quiz.Questions.Sum(x => x.EstimatedTime);
           

            vm.Quiz = quiz.ToQuizViewModel();

            return vm;
        }

        public static QuizCategoryViewModel ToQuizCategoryViewModel(this QuizCategory category, IQuizService quizzes)
        {
            var vm = new QuizCategoryViewModel()
            {
                Id = category.Id,
                Name = category.Name,
                QuizCount = quizzes.GetAll().Count(x => x.Category.Id == category.Id)
            };

            return vm;
        }

        public static List<QuizCategoryViewModel> ToQuizCategoriesViewModel(this IQueryable<QuizCategory> categories,IQueryable<Quiz> quizzes, int? catId)
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

        public static QuizzesViewModel ToQuizzesViewModel(this IQueryable<Quiz> quizzes, 
            IQuizService quizService,
            IQuizCategoryService categories, 
            int? catId)
        {
            var vm = new QuizzesViewModel();
            
            vm.QuizCategories = categories.GetAll().ToQuizCategoriesViewModel(quizService.GetPublicQuizzes(), catId);

            vm.Filter = QuizFilter.Newest;
           

            vm.TopQuizzes = quizService.GetBy(QuizFilter.Rating, descending: true, count: 10).Select(x => new TopListItem()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            vm.SelectedCategoryId = catId.HasValue ?  catId.Value:  -1;

            vm.SearchPlaceHolder = vm.SelectedCategoryId > -1 
                ? "Search in " + categories.Get(vm.SelectedCategoryId).Name 
                : "Search here";


            vm.Quizzes = quizzes.ToList().Select(x => x.ToQuizViewModel()).ToList();

            const int pageSize = 10;

            if (vm.Quizzes.Count() <= pageSize)
            {
                vm.PageCount = 1;
            }
            else
            {
                vm.PageCount = (int)Math.Ceiling((double)vm.Quizzes.Count() / pageSize);
            }

            vm.Quizzes = vm.Quizzes.Take(pageSize).ToList();
           
            return vm;
        }

        public static EditQuizViewModel ToEditQuizViewModel(this Quiz quiz, IQuizCategoryService categories)
        {
            var vm = new EditQuizViewModel();


            vm.Quiz = quiz.ToQuizViewModel();

            var sortedQuestions = quiz.Questions.AsQueryable().AsNoTracking().OrderBy(x => x.OrderId).Select(x => x.Id);
            vm.QuestionOrder = string.Join(",", sortedQuestions);
            vm.Categories = categories.GetCategoriesAsSelectList();
            vm.SelectedCategory = quiz.Category.Id;

            vm.EstimatedTime = (uint)quiz.Questions.Sum(x => x.EstimatedTime);

            return vm;
        }

      

        public static EditQuestionViewModel ToEditQuestionViewModel(this Question question, IQuizService quizzes)
        {
            var vm = new EditQuestionViewModel();

            vm.Question = question.ToQuestionViewModel();
            vm.Order = string.Join(",", question.Answers.AsQueryable().AsNoTracking().OrderBy(x => x.OrderId).Select(x => x.Id));
           
            var quiz = quizzes.Get(question.QuizId);
            vm.QuizId = quiz.Id;
            vm.AllowPoints = quiz.AllowPoints;
            vm.IsTimeable = quiz.IsTimeable;
            
            return vm;
        }

        public static ActiveQuizViewModel ToActiveQuizViewModel(this Quiz quiz)
        {
            var vm = new ActiveQuizViewModel();
            vm.QuizId = quiz.Id;
            vm.EstimatedTime = quiz.Questions.Sum(x => x.EstimatedTime);

            return vm;
        }

        public static QuestionReviewViewModel ToQuestionReviewViewModel(this Question question,int resultId, int userAnswerId )
        {
            var vm = new QuestionReviewViewModel();
            vm.Question = question.ToQuestionViewModel();
            
            vm.QuizResultId = resultId;
            vm.UserAnswerId = userAnswerId;
            return vm;
        }

        public static QuizReviewViewModel ToQuizReviewViewModel(this QuizResult quizResult, IQuizService quizzes, IAnswerService answers,
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


            vm.ResultId = quizResult.Id;
            vm.Score = quizResult.Score;
            vm.EllapsedTime = quizResult.EllapsedTime;
            vm.UserAnswers = new List<UserAnswerViewModel>();
            vm.UserAnswers.AddRange(
                quizResult.Answers.Select(x => new UserAnswerViewModel()
                {
                    AnswerId = x.AnswerId,
                    QuestionId = x.QuestionId
                }));
            
            
            
            
            vm.ScoreText = scoreText;
            vm.EllapsedTimePercent = (int)((double)quizResult.EllapsedTime/(double)questions.Sum(x => x.EstimatedTime)*100);
            vm.QuizId = quizResult.QuizId;

            
            vm.RelativeScore = quizResultService.GetRelativeScore(quiz.Id, quizResult.Score);
            vm.RelativeTime = quizResultService.GetRelativeTime(quiz.Id, quizResult.EllapsedTime);
            return vm;
        }


        public static CreateQuizViewModel ToCreateQuizViewModel(this Quiz quiz, IQuizCategoryService categories, string userName)
        {
            var vm = new CreateQuizViewModel();
            vm.Categories = categories.GetCategoriesAsSelectList();
            vm.Quiz = new QuizViewModel();
            vm.Quiz.CreatedBy = userName;

            return vm;
        }

        public static EditAnswerViewModel ToEditAnswerViewModel(this Answer answer)
        {
            var vm = new EditAnswerViewModel();
           
            vm.QuestionId = answer.QuestionId;
            vm.QuizId = answer.Question.QuizId;

            vm.Answer = answer.ToAnswerViewModel();

            return vm;
        }

    
    }
}