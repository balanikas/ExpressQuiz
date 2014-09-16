using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ExpressQuiz.Core.Services;

namespace ExpressQuiz.Extensions
{
    public static class ServiceExtensions
    {
        public static IEnumerable<SelectListItem> GetCategoriesAsSelectList(this IQuizCategoryService repo)
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