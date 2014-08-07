using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpressQuiz.Models;
using ExpressQuiz.ViewModels;

namespace ExpressQuiz.Repos
{
    public static class RepoExtensions
    {
        public static IEnumerable<Quiz> GetTopList(this IRepo<Quiz> repo, IRepo<QuizRating> quizRatingRepo,  int? count)
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

        public static IEnumerable<Quiz> AsOrdered(this IRepo<Quiz> repo, IRepo<QuizRating> quizRatingRepo, QuizFilter filter)
        {



            switch (filter)
            {
                case QuizFilter.Rating:
                    return repo.GetTopList(quizRatingRepo, null);
                    break;
                case QuizFilter.Newest:
                    return repo.GetAll().OrderByDescending(x => x.Created);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
    }
}