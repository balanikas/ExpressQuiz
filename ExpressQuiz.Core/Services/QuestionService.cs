using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        public void SaveOrder(List<Question> questions, string order)
        {

            var orders = order.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (orders.Length == 0 || orders.Length != questions.Count())
            {
                throw new ArgumentException("order");
            }

            int orderCount = 0;
            foreach (var o in orders)
            {
                var q = questions.First(x => x.Id.ToString() == o);
                q.OrderId = orderCount++;
                _questionRepo.Update(q);
            }
            _questionRepo.Save();
        }


    }
}
