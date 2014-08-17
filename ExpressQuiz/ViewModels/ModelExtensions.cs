using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ExpressQuiz.Models;
using ExpressQuiz.Repos;

namespace ExpressQuiz.ViewModels
{
    public static class ModelExtensions
    {
        public static QuizDetailsViewModel ToViewModel(this Quiz quiz, IRepo<QuizResult> quizResults,
            IRepo<QuizRating> quizRatings )
        {
            var vm = new QuizDetailsViewModel();

            var ratings = quizRatings.GetAll().Where(x => x.QuizId == quiz.Id);
            if (ratings.Any())
            {
                vm.AvgLevel = (int)ratings.Average(x => x.Level) * 20;
                
                vm.AvgRating = (int)ratings.Average(x => x.Rating) * 20;
            }

            var results = quizResults.GetAll().Where(x => x.QuizId == quiz.Id);
            if (results.Any())
            {
                if (quiz.AllowPoints)
                {
                    vm.AvgScore = (int)results.Average(x => x.Score);
                    vm.AvgScorePercent = (vm.AvgScore * 100) / quiz.Questions.Sum(x => x.Points);
                }
                else
                {
                    vm.AvgScore = ((int)results.Average(x => x.Score)*100) / quiz.Questions.Count;
                    vm.AvgScorePercent = vm.AvgScore;
                }
                
                vm.AvgTime = (int)results.Average(x => x.EllapsedTime);
                vm.AvgTimePercent = (vm.AvgTime*100)/quiz.Questions.Sum(x => x.EstimatedTime);
                vm.Sessions = results.Count();
            }

            
          
            vm.Quiz = quiz;

            return vm;
        }

      

        public static QuizzesViewModel ToViewModel(this IQueryable<Quiz> quizzes, 
            IRepo<QuizCategory> categories, 
            IRepo<QuizRating> ratings,
            IRepo<Quiz> quizRepo,  
            int? catId, 
            string searchTerm)
        {
            var vm = new QuizzesViewModel();

            vm.QuizCategories = (from c in categories.GetAll()
                                 orderby c.Name
                                 select new QuizCategoryViewModel()
                                 {
                                     Id = c.Id,
                                     Name = c.Name,
                                     IsSelected = catId.HasValue ?  (c.Id == catId.Value) : false,
                                     QuizCount = quizzes.Count(x => x.Category.Id == c.Id)
                                 }).ToList();

            vm.QuizCategories.Insert(0, new QuizCategoryViewModel()
            {
                Id = -1,
                Name = "All",
                IsSelected = catId.HasValue ? (-1 == catId.Value) : true,
                QuizCount = quizzes.Count()
            });

            if (!String.IsNullOrEmpty(searchTerm))
            {
                quizzes = quizzes.Where(s => s.Name.Contains(searchTerm));
            }

            if (catId.HasValue && catId != -1)
            {
                quizzes = quizzes.Where(x => x.Category.Id == catId);
            }

            vm.Quizzes = quizzes.OrderByDescending(x => x.Created).ToList();

            vm.Filter = QuizFilter.Newest;
            vm.TopQuizzes = quizRepo.GetTopListByRating(ratings, 10).Select(x => new TopListItem()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            vm.SelectedCategory = catId.HasValue ? catId.Value : -1;

            return vm;
        }

        public static EditQuizViewModel ToViewModel(this Quiz quiz, IRepo<QuizCategory> categories)
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

        public static QuestionReviewViewModel ToViewModel(this Question question, int resultId)
        {
            var vm = new QuestionReviewViewModel();
            vm.Question = question;
            vm.QuizResultId = resultId;
            return vm;
        }

        public static QuizReviewViewModel ToViewModel(this QuizResult quizResult, IRepo<Question> questions, IRepo<Answer> answers  )
        {
            var vm = new QuizReviewViewModel();

            var qDetails = new List<QuizReviewItem>();
            foreach (var userAnswer in quizResult.Answers)
            {
                var answer = answers.GetAll().FirstOrDefault(x => x.Id == userAnswer.AnswerId);
                var isAnswerCorrect = answer != null ? answer.IsCorrect : false;
                var questionText = questions.GetAll().First(x => x.Id == userAnswer.QuestionId).Text;

                qDetails.Add(new QuizReviewItem(isAnswerCorrect, questionText, userAnswer.QuestionId));


            }
            vm.Items = qDetails;
            vm.Result = quizResult;
            vm.QuizId = quizResult.QuizId;

            return vm;
        }


        public static CreateQuizViewModel ToViewModel(this Quiz quiz, IRepo<QuizCategory> categories, string userName)
        {
            var vm = new CreateQuizViewModel();
            vm.Categories = categories.GetCategoriesAsSelectList();
            vm.Quiz = new Quiz();
            vm.Quiz.CreatedBy = userName;

            return vm;
        }
    }
}