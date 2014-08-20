using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressQuiz.Core.Repos;
using ExpressQuiz.Core.Services;

namespace ExpressQuiz.Tests
{
    public class ServiceProvider
    {
        public QuizService QuizService { get; private set; }
        public QuestionService QuestionService { get; private set; }
        public AnswerService AnswerService { get; private set; }
        public QuizCategoryService QuizCategoryService { get; private set; }
        public ServiceProvider(RepoProvider repoProvider)
        {
            QuizService = new QuizService(repoProvider.QuizRepo,repoProvider.QuizResultRepo,repoProvider.QuizRatingRepo);
            QuestionService = new QuestionService(repoProvider.QuestionRepo);
            AnswerService = new AnswerService(repoProvider.AnswerRepo);
            QuizCategoryService = new QuizCategoryService(repoProvider.QuizCategoryRepo);
        }
    }
}
