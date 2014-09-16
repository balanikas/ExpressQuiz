using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;
using ExpressQuiz.Core.Services;
using ExpressQuiz.Extensions;
using ExpressQuiz.ViewModels;

namespace ExpressQuiz
{
    public class ModelConverter
    {
        private readonly IAnswerService _answerService;
        private readonly IQuestionService _questionService;
        private readonly IQuizCategoryService _quizCategorytService;
        private readonly IQuizResultService _quizResultService;
        private readonly IQuizService _quizService;

        public ModelConverter(
            IQuizCategoryService quizCategorytService,
            IQuizResultService quizResultService,
            IAnswerService answerService,
            IQuestionService questionService,
            IQuizService quizService)
        {
            _quizCategorytService = quizCategorytService;
            _quizResultService = quizResultService;
            _answerService = answerService;
            _questionService = questionService;
            _quizService = quizService;
        }

        public UserQuizzesViewModel ToUserQuizzesViewModel(IQueryable<Quiz> quizzes, string userId)
        {
            var vm = new UserQuizzesViewModel();
            vm.Quizzes = ToQuizViewModels(quizzes).AsEnumerable();
            vm.UserId = userId;
            return vm;
        }

        public IQueryable<QuizViewModel> ToQuizViewModels(IQueryable<Quiz> quizzes)
        {
            var query = quizzes.Select(x => new QuizViewModel
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
                Category = new QuizCategoryViewModel
                {
                    Id = x.Category.Id,
                    Name = x.Category.Name
                },
                QuestionCount = x.Questions.Count()
            });

            return query;
        }

        public IQueryable<QuestionViewModel> ToQuestionViewModels(IQueryable<Question> questions,
            bool includeAnswers = false,
            bool orderedAnswers = false)
        {
            IQueryable<QuestionViewModel> query;

            if (includeAnswers)
            {
                if (orderedAnswers)
                {
                    query = questions.Select(x => new QuestionViewModel
                    {
                        EstimatedTime = x.EstimatedTime,
                        OrderId = x.OrderId,
                        Points = x.Points,
                        Text = x.Text,
                        QuestionId = x.Id,
                        Votes = x.Votes,
                        AnswerCount = x.Answers.Count(),
                        Answers = x.Answers.Select(z =>
                            new AnswerViewModel
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
                    query = questions.Select(x => new QuestionViewModel
                    {
                        EstimatedTime = x.EstimatedTime,
                        OrderId = x.OrderId,
                        Points = x.Points,
                        Text = x.Text,
                        QuestionId = x.Id,
                        Votes = x.Votes,
                        AnswerCount = x.Answers.Count(),
                        Answers = x.Answers.Select(z =>
                            new AnswerViewModel
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
                query = questions.Select(x => new QuestionViewModel
                {
                    EstimatedTime = x.EstimatedTime,
                    OrderId = x.OrderId,
                    Points = x.Points,
                    Text = x.Text,
                    QuestionId = x.Id,
                    Votes = x.Votes,
                    AnswerCount = x.Answers.Count(),
                });


                if (orderedAnswers)
                {
                    query = query.OrderBy(x => x.OrderId);
                }
            }

            return query;
        }

        public IQueryable<AnswerViewModel> ToAnswerViewModels(IQueryable<Answer> quizzes)
        {
            var query = quizzes.Select(x => new AnswerViewModel
            {
                Text = x.Text,
                Explanation = x.Explanation,
                IsCorrect = x.IsCorrect,
                OrderId = x.OrderId,
                AnswerId = x.Id
            });

            return query;
        }

        public QuizViewModel ToQuizViewModel(Quiz quiz,
            bool includeQuestions = false,
            bool includeAnswers = false,
            bool orderedQuestions = false,
            bool orderedAnswers = false)
        {
            var vm = new QuizViewModel
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
                Category = new QuizCategoryViewModel
                {
                    Id = quiz.Category.Id,
                    Name = quiz.Category.Name
                },
                QuestionCount = quiz.Questions.Count()
            };

            if (includeQuestions)
            {
                var query = ToQuestionViewModels(_questionService.GetAll()
                    .Where(x => x.QuizId == quiz.Id), includeAnswers, orderedAnswers);

                if (orderedQuestions)
                {
                    query = query.OrderBy(x => x.OrderId);
                }

                vm.Questions = query;
            }

            return vm;
        }

        public QuestionViewModel ToQuestionViewModel(Question question,
            bool includeAnswers = false)
        {
            var vm = new QuestionViewModel
            {
                EstimatedTime = question.EstimatedTime,
                OrderId = question.OrderId,
                Points = question.Points,
                Text = question.Text,
                QuestionId = question.Id,
                AnswerCount = _answerService.GetAll().Count(x => x.QuestionId == question.Id)
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


        public AnswerViewModel ToAnswerViewModel(Answer answer)
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

        public QuizResultViewModel ToQuizResultViewModel(QuizResult result)
        {
            var vm = new QuizResultViewModel
            {
                EllapsedTime = result.EllapsedTime,
                Score = result.Score,
                UserId = result.UserId,
                QuizId = result.QuizId,
            };

            vm.UserAnswers = result.UserAnswers.Select(x => new UserAnswerViewModel
            {
                AnswerId = x.AnswerId,
                QuestionId = x.QuestionId
            }).ToList();

            return vm;
        }

        public QuizDetailsViewModel ToQuizDetailsViewModel(Quiz quiz)
        {
            var vm = new QuizDetailsViewModel();

            var stats = _quizResultService.GetStatistics(quiz.Id);

            vm.AvgLevel = stats.AvgLevel;
            vm.AvgRating = stats.AvgRating;
            vm.AvgScore = stats.AvgScore;
            vm.TotalPoints = stats.TotalPoints;
            vm.AvgScorePercent = stats.AvgScorePercent;
            vm.AvgTime = stats.AvgTime;
            vm.AvgTimePercent = stats.AvgTimePercent;
            vm.TotalTime = stats.TotalTime;


            vm.Quiz = ToQuizViewModel(quiz);


            return vm;
        }

        public QuizCategoryViewModel ToQuizCategoryViewModel(QuizCategory category)
        {
            var vm = new QuizCategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                QuizCount = _quizService.GetAll().Count(x => x.Category.Id == category.Id)
            };

            return vm;
        }

        public IEnumerable<QuizCategoryViewModel> ToQuizCategoriesViewModel(int? catId)
        {
            var categories = _quizCategorytService.GetAll();
            var quizzes = _quizService.GetPublicQuizzes();

            var cats = (from c in categories
                orderby c.Name
                select new QuizCategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsSelected = catId.HasValue ? (c.Id == catId.Value) : false,
                    QuizCount = quizzes.Count(x => x.Category.Id == c.Id)
                }).ToList();

            cats.Insert(0, new QuizCategoryViewModel
            {
                Id = -1,
                Name = "All",
                IsSelected = catId.HasValue ? (-1 == catId.Value) : true,
                QuizCount = cats.Sum(x => x.QuizCount)
            });

            return cats;
        }

        public QuizzesViewModel ToQuizzesViewModel(IQueryable<Quiz> quizzes, int? catId)
        {
            var vm = new QuizzesViewModel();

            vm.QuizCategories = ToQuizCategoriesViewModel(catId);

            vm.Filter = QuizFilter.Newest;


            vm.TopQuizzes =
                _quizService.GetBy(QuizFilter.Votes, descending: true, count: 10).Select(x => new TopListItem
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();

            vm.SelectedCategoryId = catId.HasValue ? catId.Value : -1;

            vm.SearchPlaceHolder = vm.SelectedCategoryId > -1
                ? "Search in " + _quizCategorytService.Get(vm.SelectedCategoryId).Name
                : "Search here";

            const int pageSize = 10;
            var quizCount = quizzes.Count();
            if (quizCount <= pageSize)
            {
                vm.PageCount = 1;
            }
            else
            {
                vm.PageCount = (int) Math.Ceiling((double) quizCount/pageSize);
            }

            vm.Quizzes = ToQuizViewModels(quizzes.Take(pageSize));


            return vm;
        }

        public EditQuizViewModel ToEditQuizViewModel(Quiz quiz)
        {
            var vm = new EditQuizViewModel();


            vm.Quiz = ToQuizViewModel(quiz, true);

            var sortedQuestions = quiz.Questions.AsQueryable().AsNoTracking().OrderBy(x => x.OrderId).Select(x => x.Id);
            vm.QuestionOrder = string.Join(",", sortedQuestions);
            vm.Categories = _quizCategorytService.GetCategoriesAsSelectList();
            vm.SelectedCategory = quiz.Category.Id;

            vm.EstimatedTime = (uint) quiz.Questions.Sum(x => x.EstimatedTime);

            return vm;
        }


        public EditQuestionViewModel ToEditQuestionViewModel(Question question)
        {
            var vm = new EditQuestionViewModel();

            vm.Question = ToQuestionViewModel(question, true);
            vm.Order = string.Join(",",
                question.Answers.AsQueryable().AsNoTracking().OrderBy(x => x.OrderId).Select(x => x.Id));

            var quiz = _quizService.Get(question.QuizId);
            vm.QuizId = quiz.Id;
            vm.AllowPoints = quiz.AllowPoints;
            vm.IsTimeable = quiz.IsTimeable;

            return vm;
        }

        public ActiveQuizViewModel ToActiveQuizViewModel(Quiz quiz)
        {
            var vm = new ActiveQuizViewModel();
            vm.QuizId = quiz.Id;
            vm.EstimatedTime = quiz.Questions.Sum(x => x.EstimatedTime);

            return vm;
        }

        public QuestionReviewViewModel ToQuestionReviewViewModel(Question question, int resultId, int userAnswerId)
        {
            var vm = new QuestionReviewViewModel();
            vm.Question = ToQuestionViewModel(question, true);

            vm.QuizResultId = resultId;
            vm.UserAnswerId = userAnswerId;
            return vm;
        }

        public QuizReviewViewModel ToQuizReviewViewModel(QuizResult quizResult)
        {
            var vm = new QuizReviewViewModel();
            var quiz = _quizService.Get(quizResult.QuizId);
            var questions = quiz.Questions.Where(x => x.QuizId == quizResult.QuizId).ToList();


            var qDetails = new List<QuizReviewItemViewModel>();
            foreach (var userAnswer in quizResult.UserAnswers)
            {
                var answer = _answerService.GetAll().FirstOrDefault(x => x.Id == userAnswer.AnswerId);
                var isAnswerCorrect = answer != null && answer.IsCorrect;
                var question = questions.First(x => x.Id == userAnswer.QuestionId);
                var questionText = question.Text;
                var pointsEarned = isAnswerCorrect ? question.Points : 0;
                qDetails.Add(new QuizReviewItemViewModel
                {
                    IsCorrectAnswer = isAnswerCorrect,
                    QuestionId = userAnswer.QuestionId,
                    QuestionText = questionText,
                    PointsEarned = pointsEarned,
                });
            }

            if (quiz.AllowPoints)
            {
                var totalPoints = questions.Sum(x => x.Points);

                vm.ScoreText = quizResult.Score + " / " + totalPoints + " points";

                vm.ScorePercent = Math.Ceiling(((double) quizResult.Score*100)/totalPoints);
            }
            else
            {
                vm.ScoreText = quizResult.Score + " / " + questions.Sum(x => x.Points) + "points";
                vm.ScorePercent = ((double) quizResult.Score*100)/questions.Sum(x => x.Points);
            }

            vm.Items = qDetails;


            vm.ResultId = quizResult.Id;
            vm.Score = quizResult.Score;
            vm.EllapsedTime = quizResult.EllapsedTime;
            vm.UserAnswers = new List<UserAnswerViewModel>();
            vm.UserAnswers.AddRange(
                quizResult.UserAnswers.Select(x => new UserAnswerViewModel
                {
                    AnswerId = x.AnswerId,
                    QuestionId = x.QuestionId
                }));


            vm.EllapsedTimePercent = (int) (quizResult.EllapsedTime/(double) questions.Sum(x => x.EstimatedTime)*100);
            vm.QuizId = quizResult.QuizId;

            if (_quizResultService.GetAll().Count(x => x.QuizId == quiz.Id) > 1)
            {
                vm.RelativeScore = _quizResultService.GetRelativeScore(quiz.Id, quizResult.Score);
                vm.RelativeTime = _quizResultService.GetRelativeTime(quiz.Id, quizResult.EllapsedTime);
            }
            else
            {
                vm.RelativeScore = -1;
                vm.RelativeTime = -1;
            }

            return vm;
        }


        public CreateQuizViewModel ToCreateQuizViewModel(Quiz quiz, string userName)
        {
            var vm = new CreateQuizViewModel();
            vm.Categories = _quizCategorytService.GetCategoriesAsSelectList();
            vm.Quiz = new QuizViewModel();
            vm.Quiz.CreatedBy = userName;

            return vm;
        }

        public EditAnswerViewModel ToEditAnswerViewModel(Answer answer)
        {
            var vm = new EditAnswerViewModel();

            vm.QuestionId = answer.QuestionId;
            vm.QuizId = answer.Question.QuizId;

            vm.Answer = ToAnswerViewModel(answer);

            return vm;
        }

        public UserActivitiesViewModel ToUserActivitiesViewModel(IQueryable<UserActivity> activities)
        {
            var vm = new UserActivitiesViewModel();


            vm.Activities = activities.Select(x => new UserActivityViewModel
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