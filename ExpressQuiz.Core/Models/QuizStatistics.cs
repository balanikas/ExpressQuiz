using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressQuiz.Core.Models
{
    public class QuizStatistics
    {
        public int AvgScore { get; set; }
        public int AvgLevel { get; set; }
        public int AvgRating { get; set; }
        public int AvgTime { get; set; }
        public int AvgTimePercent { get; set; }
        public int AvgScorePercent { get; set; }
        public int TotalPoints { get; set; }
        public int TotalTime { get; set; }

        public QuizStatistics()
        {
            AvgLevel = -1;
            AvgRating = -1;
            AvgScore = -1;
            AvgScorePercent = -1;
            AvgTime = -1;
            AvgTimePercent = -1;
            TotalPoints = -1;
            TotalTime = -1;
        }
    }
}
