using System;
using System.Linq;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;

namespace ExpressQuiz.Core.Services
{
    public class QuizService :  IQuizService
    {
        private readonly IRepo<Quiz> _quizRepo;
        private readonly IRepo<QuizResult> _quizResultRepo;
        private readonly IRepo<QuizRating> _quizRatingRepo;

        public QuizService(
            IRepo<Quiz> quizRepo,
            IRepo<QuizResult> quizResultRepo,
            IRepo<QuizRating> quizRatingRepo )
        {
            _quizRepo = quizRepo;
            _quizResultRepo = quizResultRepo;
            _quizRatingRepo = quizRatingRepo;
        }


        public Quiz Get(int id)
        {
            return _quizRepo.Get(id);
        }

        public IQueryable<Quiz> GetAll()
        {
            return _quizRepo.GetAll();
        }

        public IQueryable<Quiz> GetPublicQuizzes()
        {
            var quizzes = _quizRepo.GetAll();

            quizzes = quizzes.Where(x => !x.Locked);


            quizzes = from q in quizzes
                      where q.Questions.Count > 0 &&
                      (from que in q.Questions where que.Answers.Count > 1 select que).Any()
                      select q;


            return quizzes;
        }

        public Quiz Insert(Quiz o)
        {
            var quiz = _quizRepo.Insert(o);
            _quizRepo.Save();
            return quiz;
        }

        public void Update(Quiz o)
        {
            _quizRepo.Update(o);
            _quizRepo.Save();
        }

        public void Delete(int id)
        {
            _quizRepo.Delete(id);
            _quizRepo.Save();
        }

        public int GetAverageLevel(Quiz quiz)
        {
            var ratings = _quizRatingRepo.GetAll().Where(x => x.QuizId == quiz.Id);
            if (ratings.Any())
            {
                return (int)ratings.Average(x => x.Level);
            }
            return -1;
        }

        public int GetAverageRating(Quiz quiz)
        {
            var ratings = _quizRatingRepo.GetAll().Where(x => x.QuizId == quiz.Id);
            if (ratings.Any())
            {
                return (int)ratings.Average(x => x.Rating);
            }
            return -1;
        }

        public int GetAverageScore(Quiz quiz)
        {
            var results = _quizResultRepo.GetAll().Where(x => x.QuizId == quiz.Id);
            if (results.Any())
            {
                return (int)results.Average(x => x.Score);
            }
            return -1;
        }

        public int GetAverageTime(Quiz quiz)
        {
            var results = _quizResultRepo.GetAll().Where(x => x.QuizId == quiz.Id);
            if (results.Any())
            {
                return (int)results.Average(x => x.EllapsedTime);
            }
            return -1;
        }

        public int GetAverageTimePercent(Quiz quiz)
        {
            var results = _quizResultRepo.GetAll().Where(x => x.QuizId == quiz.Id);
            if (results.Any())
            {
                var avgTime = (int)results.Average(x => x.Score);
                return (avgTime * 100) / quiz.Questions.Sum(x => x.EstimatedTime);
            }
            return -1;
        }

        public IQueryable<Quiz> GetBySearchTerm(string searchTerm,IQueryable<Quiz> quizzes = null )
        {
            var list = quizzes ?? GetPublicQuizzes();
            return list.Where(s => s.Name.ToLower().Contains(searchTerm.ToLower()));
        }

        public IQueryable<Quiz> GetByCategory(int categoryId, IQueryable<Quiz> quizzes = null)
        {
            var list = quizzes ?? GetPublicQuizzes();
            return list.Where(x => x.Category.Id == categoryId);
        }

        public IQueryable<Quiz> GetBy( QuizFilter filter,IQueryable<Quiz> quizzes = null,bool? descending = null,int? count = null)
        {
            var list = quizzes ?? GetPublicQuizzes();
            var descendingValue = descending.HasValue ? descending.Value : true;
            IQueryable<Quiz> topList;
            switch (filter)
            {
                case QuizFilter.Rating:
                    topList = GetByRating(descendingValue, list);
                    break;
                case QuizFilter.Newest:
                    topList = GetByCreationDate(descendingValue, list);
                    break;
                case QuizFilter.Level:
                    topList = GetByLevel(descendingValue, list);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
           
            if (count.HasValue)
            {
                return topList.Take(count.Value);
            }

            return topList;
        }

        public IQueryable<Quiz> GetByRating(bool descending, IQueryable<Quiz> quizzes = null)
        {
            var list = quizzes ?? GetPublicQuizzes();
            var avgRatings = _quizRatingRepo.GetAll().GroupBy(x => x.QuizId).Select(
             group => new
             {
                 QuizId = group.Key,
                 Rating = group.Select(x => x.Rating).Average()
             }

             );

            

            IQueryable<Quiz> topList;
            if (descending)
            {
                topList = from x in avgRatings
                          join y in list on x.QuizId equals y.Id
                          orderby x.Rating descending
                          select y;
            }
            else
            {
                topList = from x in avgRatings
                          join y in list on x.QuizId equals y.Id
                          orderby x.Rating
                          select y;
            }

            return topList;
        }

        public IQueryable<Quiz> GetByLevel(bool descending, IQueryable<Quiz> quizzes=null)
        {
            var list = quizzes ?? GetPublicQuizzes();
            var avgLevels = _quizRatingRepo.GetAll().GroupBy(x => x.QuizId).Select(
             group => new
             {
                 QuizId = group.Key,
                 Level = group.Select(x => x.Level).Average()
             }

             );

            IQueryable<Quiz> topList;
            if (descending)
            {
                topList = from x in avgLevels
                          join y in list on x.QuizId equals y.Id
                          orderby x.Level descending
                          select y;
            }
            else
            {
                topList = from x in avgLevels
                          join y in list on x.QuizId equals y.Id
                          orderby x.Level
                          select y;
            }

            return topList;
        }

        public IQueryable<Quiz> GetByCreationDate(bool descending, IQueryable<Quiz> quizzes= null)
        {
            var list = quizzes ?? GetPublicQuizzes();
            if (descending)
            {
                return list.OrderByDescending(x => x.Created);
            }
            else
            {
                return list.OrderBy(x => x.Created);
            }
        }

        public bool Exists(string name)
        {
            return _quizRepo.GetAll().Any(x => x.Name == name);

        }

    }
}