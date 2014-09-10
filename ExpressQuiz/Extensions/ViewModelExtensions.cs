using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Services;
using ExpressQuiz.ViewModels;

namespace ExpressQuiz.Extensions
{
    public static class ViewModelExtensions
    {
        public static Quiz ToModel(this EditQuizViewModel vm, IQuizService quizService, IQuizCategoryService quizCategoryService)
        {
            var model = quizService.Get(vm.Quiz.QuizId);

            if (!String.IsNullOrEmpty(vm.NewCategory))
            {
                model.Category = quizCategoryService.InsertByName(vm.NewCategory);
                model.Category.Id = model.Category.Id;
            }
            else
            {
                model.Category = quizCategoryService.Get(vm.SelectedCategory);
                model.Category.Id = vm.SelectedCategory;
            }

            model.Summary = vm.Quiz.Summary;
            model.Name = vm.Quiz.Name;
            model.IsTimeable = vm.Quiz.IsTimeable;
            model.Locked = vm.Quiz.Locked;
            model.AllowPoints = vm.Quiz.AllowPoints;

            return model;
        }


        public static Quiz ToModel(this CreateQuizViewModel vm, IQuizCategoryService categories)
        {
            var model = new Quiz();

            if (!String.IsNullOrEmpty(vm.NewCategory))
            {
                model.Category = categories.InsertByName(vm.NewCategory);
            }
            else
            {
                model.Category = categories.Get(vm.SelectedCategory);
            }

            model.IsTimeable = vm.Quiz.IsTimeable;
            model.Locked = vm.Quiz.Locked;
            model.Summary = vm.Quiz.Summary;
            model.Name = vm.Quiz.Name;
            model.Created = DateTime.Now;
            model.AllowPoints = vm.Quiz.AllowPoints;


            return model;
        }
     
        public static Question ToModel(this EditQuestionViewModel vm, IQuestionService questionService)
        {
            var model = questionService.Get(vm.Question.QuestionId);
            model.Text = vm.Question.Text;
            model.EstimatedTime = vm.Question.EstimatedTime;
            model.Points = vm.Question.Points;

            return model;
        }

        public static QuizResult ToModel(this QuizResultViewModel vm)
        {
            var model = new QuizResult();
            model.EllapsedTime = vm.EllapsedTime;
            model.QuizId = vm.QuizId;
            model.Score = vm.Score;
            model.UserId = vm.UserId;
            model.UserAnswers = vm.UserAnswers.Select(x => new UserAnswer()
            {
                AnswerId = x.AnswerId,
                QuestionId = x.QuestionId
            }).ToList();

            return model;
        }

        public static void SetDefaultValues(this Question question)
        {
            question.Text = "enter question here";
            question.EstimatedTime = 10;
        }

        public static void SetDefaultValues(this Answer answer)
        {
            answer.Text = "enter answer here";
        }
    }
}