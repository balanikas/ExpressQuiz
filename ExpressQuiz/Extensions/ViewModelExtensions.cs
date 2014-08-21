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
            var model = quizService.Get(vm.Quiz.Id);

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

        public static Question ToModel(this EditQuestionViewModel vm, IQuestionService questionService)
        {
            var model = questionService.Get(vm.Question.Id);
            model.Text = vm.Question.Text;
            model.EstimatedTime = vm.Question.EstimatedTime;
            model.Points = vm.Question.Points;

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