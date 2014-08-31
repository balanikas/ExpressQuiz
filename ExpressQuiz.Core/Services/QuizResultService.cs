using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;

namespace ExpressQuiz.Core.Services
{
    public class QuizResultService : IQuizResultService
    {
        private readonly IRepo<QuizResult> _quizResultRepo;
        private readonly IRepo<QuizRating> _quizRatingRepo;
        private readonly IRepo<Quiz> _quizRepo;

        public QuizResultService(
            IRepo<Quiz> quizRepo,
            IRepo<QuizResult> quizResultRepo,
            IRepo<QuizRating> quizRatingRepo )
        {
            _quizRepo = quizRepo;
            _quizResultRepo = quizResultRepo;
            _quizRatingRepo = quizRatingRepo;
        }

        public int GetAverageScore(int quizId)
        {
            var results = _quizResultRepo.GetAll().Where(x => x.QuizId == quizId);
            if (results.Any())
            {
                return (int)results.Average(x => x.Score);
            }
            return -1;
        }

        public int GetAverageTime(int quizId)
        {
            var results = _quizResultRepo.GetAll().Where(x => x.QuizId == quizId);
            if (results.Any())
            {
                return (int)results.Average(x => x.EllapsedTime);
            }
            return -1;
        }

        public int GetAverageTimePercent(int quizId)
        {
            var results = _quizResultRepo.GetAll().Where(x => x.QuizId == quizId);
            if (results.Any())
            {
                var avgTime = (int)results.Average(x => x.Score);

                var quiz = _quizRepo.Get(quizId);
                return (avgTime * 100) / quiz.Questions.Sum(x => x.EstimatedTime);
            }
            return -1;
        }

        public int GetAverageLevel(int quizId)
        {
            var ratings = _quizRatingRepo.GetAll().Where(x => x.QuizId == quizId);
            if (ratings.Any())
            {
                return (int)ratings.Average(x => x.Level);
            }
            return -1;
        }

        public int GetAverageRating(int quizId)
        {
            var ratings = _quizRatingRepo.GetAll().Where(x => x.QuizId == quizId);
            if (ratings.Any())
            {
                return (int)ratings.Average(x => x.Rating);
            }
            return -1;
        }

        public int GetRelativeScore(int quizId, int compareValue)
        {

            var all = _quizResultRepo.GetAll().Where(x => x.QuizId == quizId).ToList();

            var allCount = all.Count();

            if (allCount == 0)
            {
                return -1;
            }

            var belowCompareValue = all.Count(x => x.Score < compareValue);

            var result = (int)(((double)belowCompareValue / (double)allCount) * 100);

            return result;
        }

        public int GetRelativeTime(int quizId, int compareValue)
        {
            var all = _quizResultRepo.GetAll().Where(x => x.QuizId == quizId).ToList();

            var allCount = all.Count();

            if (allCount == 0)
            {
                return -1;
            }

            var belowCompareValue = all.Count(x => x.EllapsedTime > compareValue);

            var result = (int)(((double)belowCompareValue / (double)allCount) * 100);

            return result;
        }

        public QuizResult Get(int id)
        {
            return _quizResultRepo.Get(id);
        }

        public IQueryable<QuizResult> GetAll()
        {
            return _quizResultRepo.GetAll();
        }

        public QuizResult Insert(QuizResult o)
        {
            var quiz = _quizResultRepo.Insert(o);
            _quizRepo.Save();
            return quiz;
        }

        public void Update(QuizResult o)
        {
            _quizResultRepo.Update(o);
            _quizResultRepo.Save();
        }

        public void Delete(int id)
        {
            _quizResultRepo.Delete(id);
            _quizResultRepo.Save();
        }
    }
}