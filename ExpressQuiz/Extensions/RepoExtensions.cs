using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;
using ExpressQuiz.Core.Services;
using ExpressQuiz.ViewModels;

namespace ExpressQuiz.Extensions
{
    public static class RepoExtensions
    {
       
       

        public static IEnumerable<SelectListItem> GetCategoriesAsSelectList(this IQuizCategoryService repo )
        {
            var cats = repo.GetAll()
                .Select(x =>
                    new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    });


            return new SelectList(cats, "Value", "Text");
        }
    }
}