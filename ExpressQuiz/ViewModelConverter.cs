using System;
using System.Linq;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Services;
using ExpressQuiz.ViewModels;

namespace ExpressQuiz
{
    public class ViewModelConverter
    {
        private readonly IAnswerService _answerService;
        private readonly IQuestionService _questionService;
        private readonly IQuizCategoryService _quizCategoryService;
        private readonly IQuizService _quizService;

        public ViewModelConverter(
            IQuizService quizService,
            IAnswerService answerService,
            IQuestionService questionService,
            IQuizCategoryService quizCategoryService)
        {
            _quizService = quizService;
            _answerService = answerService;
            _questionService = questionService;
            _quizCategoryService = quizCategoryService;
        }

        public Quiz ToQuizModel(EditQuizViewModel vm)
        {
            var model = _quizService.Get(vm.Quiz.QuizId);

            if (!String.IsNullOrEmpty(vm.NewCategory))
            {
                model.Category = _quizCategoryService.InsertByName(vm.NewCategory);
                model.Category.Id = model.Category.Id;
            }
            else
            {
                model.Category = _quizCategoryService.Get(vm.SelectedCategory);
                model.Category.Id = vm.SelectedCategory;
            }

            model.Summary = vm.Quiz.Summary;
            model.Name = vm.Quiz.Name;
            model.IsTimeable = vm.Quiz.IsTimeable;
            model.Locked = vm.Quiz.Locked;
            model.AllowPoints = vm.Quiz.AllowPoints;

            return model;
        }


        public Quiz ToQuizModel(CreateQuizViewModel vm)
        {
            var model = new Quiz();

            if (!String.IsNullOrEmpty(vm.NewCategory))
            {
                model.Category = _quizCategoryService.InsertByName(vm.NewCategory);
            }
            else
            {
                model.Category = _quizCategoryService.Get(vm.SelectedCategory);
            }

            model.IsTimeable = vm.Quiz.IsTimeable;
            model.Locked = vm.Quiz.Locked;
            model.Summary = vm.Quiz.Summary;
            model.Name = vm.Quiz.Name;
            model.Created = DateTime.Now;
            model.AllowPoints = vm.Quiz.AllowPoints;


            return model;
        }

        public Question ToQuestionModel(EditQuestionViewModel vm)
        {
            var model = _questionService.Get(vm.Question.QuestionId);
            model.Text = vm.Question.Text;
            model.EstimatedTime = vm.Question.EstimatedTime;
            model.Points = vm.Question.Points;

            return model;
        }

        public Answer ToAnswerModel(EditAnswerViewModel vm)
        {
            var model = _answerService.Get(vm.Answer.AnswerId);

            model.Explanation = vm.Answer.Explanation;
            model.IsCorrect = vm.Answer.IsCorrect;
            model.Text = vm.Answer.Text;

            return model;
        }

        public QuizResult ToQuizResultModel(QuizResultViewModel vm)
        {
            var model = new QuizResult();
            model.EllapsedTime = vm.EllapsedTime;
            model.QuizId = vm.QuizId;
            model.Score = vm.Score;
            model.UserId = vm.UserId;
            model.UserAnswers = vm.UserAnswers.Select(x => new UserAnswer
            {
                AnswerId = x.AnswerId,
                QuestionId = x.QuestionId
            }).ToList();

            return model;
        }
    }
}