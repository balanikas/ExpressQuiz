using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ExpressQuiz.Core.Models;

namespace ExpressQuiz.Core.Services
{
    public interface IQuizResultService :  IService<QuizResult>
    {
        int GetAverageScore(int quizId);

        int GetAverageTime(int quizId);

        int GetAverageTimePercent(int quizId);
        
        int GetAverageScorePercent(int quizId);

        int GetAverageLevel(int quizId);

        int GetAverageRating(int quizId);

        int GetRelativeScore(int quizId, int compareValue);

        int GetRelativeTime(int quizId, int compareValue);
       
    }
}
