using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;

namespace ExpressQuiz.Core.Services
{
    
    public class QuizCategoryService : IQuizCategoryService
    {
        private readonly IRepo<QuizCategory> _quizCategoryRepo;

        public QuizCategoryService(
            IRepo<QuizCategory> quizCategoryRepo)
        {

            _quizCategoryRepo = quizCategoryRepo;
        }
        
        public QuizCategory Get(int id)
        {
            return _quizCategoryRepo.Get(id);
        }

        public IQueryable<QuizCategory> GetAll()
        {
            return _quizCategoryRepo.GetAll();
        }

        public QuizCategory Insert(QuizCategory o)
        {
            var quiz = _quizCategoryRepo.Insert(o);
            _quizCategoryRepo.Save();
            return quiz;
        }

        public void Update(QuizCategory o)
        {
            _quizCategoryRepo.Update(o);
            _quizCategoryRepo.Save();
        }

        public void Delete(int id)
        {
            _quizCategoryRepo.Delete(id);
            _quizCategoryRepo.Save();
        }

        public QuizCategory InsertByName(string name)
        {
            var existing = _quizCategoryRepo.GetAll().FirstOrDefault(x => x.Name == name);
            if (existing != null)
            {
                return existing;
            }
            else
            {
                var newCat = _quizCategoryRepo.Insert(new QuizCategory() { Name = name });
                _quizCategoryRepo.Save();
                return newCat;
            }
        }


    }
}
