using System;
using System.Linq;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;

namespace ExpressQuiz.Core.Services
{
    
    public class QuestionService : IQuestionService
    {
        private readonly IRepo<Question> _questionRepo;


        public QuestionService(
            IRepo<Question> questionRepo)
        {
            _questionRepo = questionRepo;
        }

        public Question Get(int id)
        {
            return _questionRepo.Get(id);
        }

        public IQueryable<Question> GetAll()
        {
            return _questionRepo.GetAll();
        }

        public Question Insert(Question o)
        {
            var quiz = _questionRepo.Insert(o);
            _questionRepo.Save();
            return quiz;
        }

        public void Update(Question o)
        {
            _questionRepo.Update(o);
            _questionRepo.Save();
        }

        public void Delete(int id)
        {
            _questionRepo.Delete(id);
            _questionRepo.Save();
        }


        public void SaveOrder(int quizId, string order)
        {
            var ordersStr = order.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (ordersStr.Length == 0 )
            {
                throw new ArgumentException("order");
            }

            var orders = ordersStr.Select(int.Parse).ToList();

            var questionsToUpdate = _questionRepo.GetAll().Where(x => x.QuizId == quizId).ToList();
            int orderCount = 0;
            foreach (var o in orders)
            {
                var q = questionsToUpdate.First(x => x.Id == o);
                q.OrderId = orderCount++;
            }

            _questionRepo.Save();
        }

    }

    
}
