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
        private readonly IRepo<Question> _questionRepo;
        private readonly IRepo<Answer> _answerRepo;

        public QuizResultService(IRepo<Quiz> quizRepo, IRepo<QuizResult> quizResultRepo, IRepo<QuizRating> quizRatingRepo, IRepo<Question> questionRepo, IRepo<Answer> answerRepo)
        {
            _quizRepo = quizRepo;
            _quizResultRepo = quizResultRepo;
            _quizRatingRepo = quizRatingRepo;
            _questionRepo = questionRepo;
            _answerRepo = answerRepo;
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

        public int GetAverageScorePercent(int quizId)
        {
            var results = _quizResultRepo.GetAll().Where(x => x.QuizId == quizId);
            if (results.Any())
            {
                var avgScore = (int)results.Average(x => x.Score);

                var quiz = _quizRepo.Get(quizId);
                return (avgScore * 100) / quiz.Questions.Sum(x => x.Points);
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
                var avgTime = (int)results.Average(x => x.EllapsedTime);

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

        public QuizStatistics GetStatistics(int quizId)
        {

            var stats = new QuizStatistics();

            var ratings = _quizRatingRepo.GetAll().Where(x => x.QuizId == quizId);
            var results = _quizResultRepo.GetAll().Where(x => x.QuizId == quizId);
            var quiz = _quizRepo.Get(quizId);
            
            if (results.Any())
            {
                stats.AvgScore = (int)results.Average(x => x.Score);
                if (stats.AvgScore > 0)
                {
                    if (quiz.AllowPoints)
                    {
                        stats.AvgScorePercent = (stats.AvgScore * 100) / quiz.Questions.Sum(x => x.Points);
                    }
                    else
                    {
                        stats.AvgScorePercent = (stats.AvgScore * 100) / quiz.Questions.Count;
                    }
                    
                }
                
                stats.AvgTime = (int)results.Average(x => x.EllapsedTime);
                if (stats.AvgTime > 0)
                {
                    stats.AvgTimePercent = (stats.AvgTime * 100) / quiz.Questions.Sum(x => x.EstimatedTime);
                }
                
            }

            if (ratings.Any())
            {
                stats.AvgRating = (int)ratings.Average(x => x.Rating);
                stats.AvgLevel = (int)ratings.Average(x => x.Level);
            }


            if (quiz.AllowPoints)
            {
                stats.TotalPoints = quiz.Questions.Sum(x => x.Points);
            }
            else
            {
                stats.TotalPoints = quiz.Questions.Count;
            }
            
            stats.TotalTime = quiz.Questions.Sum(x => x.EstimatedTime);

            return stats;
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
            o.Score = CalculateScore(o);
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

        private int CalculateScore(QuizResult result)
        {
            var quiz = _quizRepo.Get(result.QuizId);
            var usePoints = quiz.AllowPoints;



            int count = 0;
            var totalPoints = 0;

            if (usePoints)
            {

                foreach (var userAnswer in result.UserAnswers)
                {
                    var points = _questionRepo.Get(userAnswer.QuestionId).Points;
                    totalPoints += points;

                    var correctAnswer = _answerRepo.Get(userAnswer.AnswerId);
                    if (correctAnswer != null && correctAnswer.IsCorrect)
                    {
                        count += points;
                    }

                }
                return count;//(int)(((double)count / (double)totalPoints) * 100);

            }
            else
            {
                totalPoints = quiz.Questions.Count;
                foreach (var userAnswer in result.UserAnswers)
                {
                    var correctAnswer = _answerRepo.Get(userAnswer.AnswerId);
                    if (correctAnswer != null && correctAnswer.IsCorrect)
                    {
                        count++;
                    }

                }
                return count;//(int)(((double)count / (double)totalPoints) * 100);
            }


        }
    }
}