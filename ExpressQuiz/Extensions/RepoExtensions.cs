using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;
using ExpressQuiz.ViewModels;

namespace ExpressQuiz.Extensions
{
    public static class RepoExtensions
    {
        public static IEnumerable<Quiz> GetTopListByRating(this IRepo<Quiz> repo, IRepo<QuizRating> quizRatingRepo,  int? count)
        {
            var avgRatings = quizRatingRepo.GetAll().GroupBy(x => x.QuizId).Select(
               group => new
               {
                   QuizId = group.Key,
                   Rating = group.Select(x => x.Rating).Average()
               }

               );

            var topList = from x in avgRatings
                join y in repo.GetAll() on x.QuizId equals y.Id
                orderby x.Rating descending
                select y;

            if (!topList.Any())
            {
                return repo.GetAll().OrderByDescending(x=>x.Created);
            }

            if (count.HasValue)
            {
                return topList.Take(count.Value);
            }
            else
            {
                return topList;
            }
        }

        public static IEnumerable<Quiz> GetTopListByLevel(this IRepo<Quiz> repo, IRepo<QuizRating> quizRatingRepo, int? count)
        {
            var avgLevels = quizRatingRepo.GetAll().GroupBy(x => x.QuizId).Select(
               group => new
               {
                   QuizId = group.Key,
                   Level = group.Select(x => x.Level).Average()
               }

               );

            var topList = from x in avgLevels
                          join y in repo.GetAll() on x.QuizId equals y.Id
                          orderby x.Level descending
                          select y;

            if (!topList.Any())
            {
                return repo.GetAll().OrderByDescending(x => x.Created);
            }

            if (count.HasValue)
            {
                return topList.Take(count.Value);
            }
            else
            {
                return topList;
            }
        }

       


        public static void SaveOrder(this IRepo<Answer> repo, Question question, string order)
        {
            var orders = order.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int orderCount = 0;
            foreach (var o in orders)
            {
                var a = question.Answers.First(x => x.Id.ToString() == o);
                a.OrderId = orderCount++;
                repo.Update(a);
            }
            repo.Save();
        }

        public static void SaveOrder(this IRepo<Question> repo, Quiz quiz, string order)
        {
            var orders = order.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int orderCount = 0;
            foreach (var o in orders)
            {
                var a = quiz.Questions.First(x => x.Id.ToString() == o);
                a.OrderId = orderCount++;
                repo.Update(a);
            }
            repo.Save();
        }

        public static QuizCategory Insert(this IRepo<QuizCategory> repo, string name)
        {
            var existing = repo.GetAll().FirstOrDefault(x => x.Name == name);
            if (existing != null)
            {
                return existing;
            }
            else
            {
                var newCat = repo.Insert(new QuizCategory() { Name = name });
                repo.Save();
                return newCat;
            }
        }

        public static IEnumerable<SelectListItem> GetCategoriesAsSelectList(this IRepo<QuizCategory> repo )
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