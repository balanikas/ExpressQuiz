using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Policy;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;
using ExpressQuiz.Core.Services;
using ExpressQuiz.ViewModels;


namespace ExpressQuiz.Extensions
{
    public static class ModelExtensions
    {

        public static UserQuizzesViewModel ToUserQuizzesViewModel(this IQueryable<Quiz> quizzes)
        {
            var query = quizzes.Select(x => new QuizViewModel()
            {
                AllowPoints = x.AllowPoints,
                Created = x.Created,
                IsTimeable = x.IsTimeable,
                CreatedBy = x.CreatedBy,
                Locked = x.Locked,
                Name = x.Name,
                QuizId = x.Id,
                Summary = x.Summary,
                Votes = x.Votes,
                Views = x.Views,
                Completed = x.Completed,
                Category = new QuizCategoryViewModel()
                {
                    Id = x.Category.Id,
                    Name = x.Category.Name
                },
                QuestionCount = x.Questions.Count()
            });

            var vm = new UserQuizzesViewModel();
            vm.Quizzes = query.ToList();

            return vm;
        }


        public static IQueryable<QuizViewModel> ToQuizViewModels(this IQueryable<Quiz> quizzes)
        {
            var query = quizzes.Select(x => new QuizViewModel()
            {
                AllowPoints = x.AllowPoints,
                Created = x.Created,
                IsTimeable = x.IsTimeable,
                CreatedBy = x.CreatedBy,
                Locked = x.Locked,
                Name = x.Name,
                QuizId = x.Id,
                Summary = x.Summary,
                Votes = x.Votes,
                Views = x.Views,
                Completed = x.Completed,
                Category = new QuizCategoryViewModel()
                {
                    Id = x.Category.Id,
                    Name = x.Category.Name
                },
                QuestionCount = x.Questions.Count()
            });

            return query;
        }

        public static IQueryable<QuestionViewModel> ToQuestionViewModels(this IQueryable<Question> questions,
            IAnswerService answerService, 
            bool includeAnswers = false,
            bool orderedAnswers = false)
        {
            IQueryable<QuestionViewModel> query;

           

            if (includeAnswers)
            {

                if (orderedAnswers)
                {
                    query = questions.Select(x => new QuestionViewModel()
                    {

                        EstimatedTime = x.EstimatedTime,
                        OrderId = x.OrderId,
                        Points = x.Points,
                        Text = x.Text,
                        QuestionId = x.Id,
                        AnswerCount = x.Answers.Count(),
                        Answers = x.Answers.Select(z =>
                                new AnswerViewModel()
                                {
                                    Text = z.Text,
                                    Explanation = z.Explanation,
                                    IsCorrect = z.IsCorrect,
                                    OrderId = z.OrderId,
                                    AnswerId = z.Id
                                }).OrderBy(z => z.OrderId)
                    });
                }
                else
                {
                    query = questions.Select(x => new QuestionViewModel()
                    {

                        EstimatedTime = x.EstimatedTime,
                        OrderId = x.OrderId,
                        Points = x.Points,
                        Text = x.Text,
                        QuestionId = x.Id,
                        AnswerCount = x.Answers.Count(),
                        Answers = x.Answers.Select(z =>
                                new AnswerViewModel()
                                {
                                    Text = z.Text,
                                    Explanation = z.Explanation,
                                    IsCorrect = z.IsCorrect,
                                    OrderId = z.OrderId,
                                    AnswerId = z.Id
                                })
                    });
                }

            }
            else
            {
                query = questions.Select(x => new QuestionViewModel()
                {

                    EstimatedTime = x.EstimatedTime,
                    OrderId = x.OrderId,
                    Points = x.Points,
                    Text = x.Text,
                    QuestionId = x.Id,
                    AnswerCount = x.Answers.Count(),

                });


                if (orderedAnswers)
                {
                    query = query.OrderBy(x => x.OrderId);
                }
            }

            return query;
        }

        public static IQueryable<AnswerViewModel> ToAnswerViewModels(this IQueryable<Answer> quizzes)
        {
            var query = quizzes.Select(x => new AnswerViewModel()
            {
                Text = x.Text,
                Explanation = x.Explanation,
                IsCorrect = x.IsCorrect,
                OrderId = x.OrderId,
                AnswerId = x.Id
            });

            return query;
        }

        public static QuizViewModel ToQuizViewModel(this Quiz quiz,
            IQuestionService questionService,
             IAnswerService answerService, 
            bool includeQuestions = false, 
            bool includeAnswers = false,
            bool orderedQuestions = false,
            bool orderedAnswers = false)
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
                Votes = quiz.Votes,
                Views = quiz.Views,
                Completed = quiz.Completed,
                Category = new QuizCategoryViewModel()
                {
                    Id = quiz.Category.Id,
                    Name = quiz.Category.Name
                },
                QuestionCount = quiz.Questions.Count()
                
            };

            if (includeQuestions)
            {
                var query = questionService.GetAll()
                    .Where(x => x.QuizId == quiz.Id)
                    .ToQuestionViewModels(answerService, includeAnswers, orderedAnswers);

                if (orderedQuestions)
                {
                    query = query.OrderBy(x => x.OrderId);
                }
              
                vm.Questions = query;
            }

            return vm;
        }

        public static QuestionViewModel ToQuestionViewModel(this Question question, IAnswerService answers, bool includeAnswers = false)
        {
            var vm = new QuestionViewModel()
            {
                
                EstimatedTime = question.EstimatedTime,
                OrderId = question.OrderId,
                Points = question.Points,
                Text = question.Text,
                QuestionId = question.Id,
                AnswerCount = answers.GetAll().Count(x=> x.QuestionId == question.Id)
            };

            if (includeAnswers)
            {
                vm.Answers = question.Answers.ToList().Select(x => new AnswerViewModel
                {
                    AnswerId = x.Id,
                    Explanation = x.Explanation,
                    IsCorrect = x.IsCorrect,
                    OrderId = x.OrderId,
                    Text = x.Text,
                });
            }

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
                QuizId = result.QuizId,
                
            };

            vm.UserAnswers = result.UserAnswers.Select(x => new UserAnswerViewModel()
            {
                AnswerId = x.AnswerId,
                QuestionId = x.QuestionId
            }).ToList();

            return vm;
        }

        public static QuizDetailsViewModel ToQuizDetailsViewModel(this Quiz quiz, 
            IQuestionService questions,
            IAnswerService answers, 
            IQuizResultService quizResultService, 
            IRepo<QuizRating> quizRatings)
        {
            var vm = new QuizDetailsViewModel();

            var stats = quizResultService.GetStatistics(quiz.Id);

            vm.AvgLevel = stats.AvgLevel;
            vm.AvgRating = stats.AvgRating;
            vm.AvgScore = stats.AvgScore;
            vm.TotalPoints = stats.TotalPoints;
            vm.AvgScorePercent = stats.AvgScorePercent;
            vm.AvgTime = stats.AvgTime;
            vm.AvgTimePercent = stats.AvgTimePercent;
            vm.TotalTime = stats.TotalTime;
            
            
            vm.Quiz = quiz.ToQuizViewModel(questions,answers);
            

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

        public static IEnumerable<QuizCategoryViewModel> ToQuizCategoriesViewModel(this IQueryable<QuizCategory> categories,IQueryable<Quiz> quizzes, int? catId)
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
                QuizCount = cats.Sum(x=>x.QuizCount)
            });

            return cats;
        }

        public static QuizzesViewModel ToQuizzesViewModel(this IQueryable<Quiz> quizzes,
            IQuestionService questions,
            IQuizService quizService,
            IQuizCategoryService categories, 
            int? catId)
        {
            var vm = new QuizzesViewModel();
            
            vm.QuizCategories = categories.GetAll().ToQuizCategoriesViewModel(quizService.GetPublicQuizzes(), catId);

            vm.Filter = QuizFilter.Newest;
           

            vm.TopQuizzes = quizService.GetBy(QuizFilter.Votes, descending: true, count: 10).Select(x => new TopListItem()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            vm.SelectedCategoryId = catId.HasValue ?  catId.Value:  -1;

            vm.SearchPlaceHolder = vm.SelectedCategoryId > -1 
                ? "Search in " + categories.Get(vm.SelectedCategoryId).Name 
                : "Search here";

            const int pageSize = 10;
            var quizCount = quizzes.Count();
            if (quizCount <= pageSize)
            {
                vm.PageCount = 1;
            }
            else
            {
                vm.PageCount = (int)Math.Ceiling((double)quizCount / pageSize);
            }

            vm.Quizzes = quizzes.Take(pageSize).ToQuizViewModels();


           

 
            return vm;
        }

        public static EditQuizViewModel ToEditQuizViewModel(this Quiz quiz, 
            IQuestionService questions,
            IAnswerService answers, 
            IQuizCategoryService categories)
        {
            var vm = new EditQuizViewModel();


            vm.Quiz = quiz.ToQuizViewModel(questions, answers,true);
            


            var sortedQuestions = quiz.Questions.AsQueryable().AsNoTracking().OrderBy(x => x.OrderId).Select(x => x.Id);
            vm.QuestionOrder = string.Join(",", sortedQuestions);
            vm.Categories = categories.GetCategoriesAsSelectList();
            vm.SelectedCategory = quiz.Category.Id;

            vm.EstimatedTime = (uint)quiz.Questions.Sum(x => x.EstimatedTime);

            return vm;
        }



        public static EditQuestionViewModel ToEditQuestionViewModel(this Question question, IAnswerService answers, IQuizService quizzes)
        {
            var vm = new EditQuestionViewModel();

            vm.Question = question.ToQuestionViewModel(answers,true);
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

        public static QuestionReviewViewModel ToQuestionReviewViewModel(this Question question, IAnswerService answers,  int resultId, int userAnswerId)
        {
            var vm = new QuestionReviewViewModel();
            vm.Question = question.ToQuestionViewModel(answers,true);
            
            vm.QuizResultId = resultId;
            vm.UserAnswerId = userAnswerId;
            return vm;
        }

        public static QuizReviewViewModel ToQuizReviewViewModel(this QuizResult quizResult, 
            IQuizService quizzes, 
            IAnswerService answers,
            IQuizResultService quizResultService)
        {
            var vm = new QuizReviewViewModel();
            var quiz = quizzes.Get(quizResult.QuizId);
            var questions = quiz.Questions.Where(x => x.QuizId == quizResult.QuizId);

           

            var qDetails = new List<QuizReviewItemViewModel>();
            foreach (var userAnswer in quizResult.UserAnswers)
            {
                var answer = answers.GetAll().FirstOrDefault(x => x.Id == userAnswer.AnswerId);
                var isAnswerCorrect = answer != null && answer.IsCorrect;
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
                quizResult.UserAnswers.Select(x => new UserAnswerViewModel()
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

        public static UserActivitiesViewModel ToUserActivitiesViewModel(this IQueryable<UserActivity> activities)
        {
            var vm = new UserActivitiesViewModel();

            
            vm.Activities = activities.Select(x => new UserActivityViewModel()
            {
                UserId = x.UserId,
                Action = x.Action.ToString(),
                Date = x.Date,
                Item = x.Item.ToString(),
                ItemId = x.ItemId
            }).ToList();

            return vm;
        }

    
    }
}