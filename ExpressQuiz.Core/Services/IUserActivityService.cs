using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ExpressQuiz.Core.Models;

namespace ExpressQuiz.Core.Services
{
    public interface IUserActivityService
    {

        void Add(string userId, ActivityItem item, ActivityAction action,  int itemId, bool overwrite = false);
        void UpdateVote(string userId, ActivityItem item, int itemId, int vote);
        void DeleteAll(string userId);
        void Delete(string userId, ActivityItem item, int itemId);
        void Delete(string userId, ActivityItem item, ActivityAction action, int itemId);
        IQueryable<UserActivity> GetAll(string userId);
        IQueryable<UserActivity> GetAll(string userId, ActivityItem item);
        IQueryable<UserActivity> GetAll(string userId, ActivityItem item, ActivityAction action);

        bool Exists(string userId, ActivityItem item, ActivityAction action, int itemId);

    }
}
