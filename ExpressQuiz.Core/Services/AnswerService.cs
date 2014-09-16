using System;
using System.Linq;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;

namespace ExpressQuiz.Core.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IRepo<Answer> _answerRepo;

        public AnswerService(
            IRepo<Answer> answerRepo)
        {
            _answerRepo = answerRepo;
        }

        public Answer Get(int id)
        {
            return _answerRepo.Get(id);
        }

        public IQueryable<Answer> GetAll()
        {
            return _answerRepo.GetAll();
        }

        public Answer Insert(Answer o)
        {
            var quiz = _answerRepo.Insert(o);
            _answerRepo.Save();
            return quiz;
        }

        public void Update(Answer o)
        {
            _answerRepo.Update(o);
            _answerRepo.Save();
        }

        public void Delete(int id)
        {
            _answerRepo.Delete(id);
            _answerRepo.Save();
        }


        public void SaveOrder(Question question, string order)
        {
            if (question.Answers.Count() <= 1)
            {
                return;
            }

            var ordersStr = order.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);

            if (ordersStr.Length == 0)
            {
                throw new ArgumentException("order");
            }

            var orders = ordersStr.Select(int.Parse).ToList();

            var answersToUpdate = _answerRepo.GetAll().Where(x => x.QuestionId == question.Id).ToList();
            int orderCount = 0;
            foreach (var o in orders)
            {
                var q = answersToUpdate.First(x => x.Id == o);
                q.OrderId = orderCount++;
            }
            _answerRepo.Save();
        }
    }
}