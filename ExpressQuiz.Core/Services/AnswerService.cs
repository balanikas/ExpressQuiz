using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        public void SaveOrder(List<Answer> answers, string answersOrder)
        {
            var orders = answersOrder.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int orderCount = 0;
            foreach (var o in orders)
            {
                var q = answers.First(x => x.Id.ToString() == o);
                q.OrderId = orderCount++;
                _answerRepo.Update(q);
            }
            _answerRepo.Save();
        }


    }
}
