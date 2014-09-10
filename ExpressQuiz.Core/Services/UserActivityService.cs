using System;
using System.Linq;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;

namespace ExpressQuiz.Core.Services
{
    public class UserActivityService : IUserActivityService
    {

        private readonly IRepo<UserActivity> _userActivityRepo;
        private readonly IRepo<Quiz> _quizRepo;
        private readonly IRepo<Question> _questionRepo;

        public UserActivityService(IRepo<UserActivity> userActivityRepo, IRepo<Quiz> quizRepo, IRepo<Question> questionRepo)
        {
            _quizRepo = quizRepo;
            _questionRepo = questionRepo;
            _userActivityRepo = userActivityRepo;
        }


        public void Add(string userId, ActivityItem item, ActivityAction action, int itemId, bool overwrite = false)
        {
            if (overwrite)
            {
                if (Exists(userId, item, action, itemId))
                {
                    Delete(userId, item, action, itemId);
                }
                
            }
           

            var ua = new UserActivity()
            {
                UserId = userId,
                Item = item,
                Action = action,
                ItemId = itemId,
                Date = DateTime.Now
            };

            _userActivityRepo.Insert(ua);
            _userActivityRepo.Save();



            switch (item)
            {
                case ActivityItem.Quiz:
                    switch (action)
                    {
                        case ActivityAction.UpVote:
                            UpdateQuizVotes(itemId, 1);
                            break;
                        case ActivityAction.DownVote:
                            UpdateQuizVotes(itemId, -1);
                            break;
                        case ActivityAction.View:
                            UpdateQuizViews(itemId);
                            break;
                        case ActivityAction.BeginQuiz:
                            break;
                        case ActivityAction.EndQuiz:
                            UpdateQuizCompletions(itemId);
                            break;
                    }
                    break;
                case ActivityItem.Question:
                    switch (action)
                    {
                        case ActivityAction.UpVote:
                            UpdateQuestionVotes(itemId, 1);
                            break;
                        case ActivityAction.DownVote:
                            UpdateQuestionVotes(itemId, -1);
                            break;
                    }
                    break;
            }

           
        }

        private void UpdateQuizCompletions(int id)
        {
            var quiz = _quizRepo.Get(id);
            quiz.Completed++;
            _quizRepo.Update(quiz);
            _quizRepo.Save();
        }

        private void UpdateQuizVotes(int id ,int vote)
        {
            var quiz = _quizRepo.Get(id);
            quiz.Votes += vote;
            _quizRepo.Update(quiz);
            _quizRepo.Save();
        }

        private void UpdateQuizViews(int id)
        {
            var quiz = _quizRepo.Get(id);
            quiz.Views++;
            _quizRepo.Update(quiz);
            _quizRepo.Save();
        }

        private void UpdateQuestionVotes(int id, int vote)
        {
            var question = _questionRepo.Get(id);
            question.Votes += vote;
            _questionRepo.Update(question);
            _questionRepo.Save();
        }

        public void UpdateVote(string userId, ActivityItem item, int itemId, int vote)
        {
            Delete(userId, item, ActivityAction.DownVote, itemId);
            Delete(userId, item, ActivityAction.UpVote, itemId);

            if (vote == 1)
            {
                Add(userId, item, ActivityAction.UpVote, itemId);
            }
            else if (vote == -1)
            {
                Add(userId, item, ActivityAction.DownVote, itemId);
            }

        }

        public void Delete(string userId, ActivityItem item, int itemId)
        {
            var uas = _userActivityRepo.GetAll().Where(
                x => x.UserId == userId &&
                x.Item == item ).ToList();

            if (!uas.Any())
            {
                return;
            }

            foreach (var ua in uas)
            {
                _userActivityRepo.Delete(ua.Id);
                _userActivityRepo.Save();
            }
            

        }

        public void Delete(string userId, ActivityItem item, ActivityAction action, int itemId)
        {
            var uas = _userActivityRepo.GetAll().Where(
                 x => x.UserId == userId &&
                 x.Item == item && 
                 x.Action == action &&
                 x.ItemId == itemId).ToList();

            if (!uas.Any())
            {
                return;
            }

            foreach (var ua in uas)
            {
                _userActivityRepo.Delete(ua.Id);
                _userActivityRepo.Save();
            }
            
        }


        public void DeleteAll(string userId)
        {
            var uas = _userActivityRepo.GetAll().Where(
               x => x.UserId == userId).ToList();

            if (!uas.Any())
            {
                return;
            }

            foreach (var ua in uas)
            {
                _userActivityRepo.Delete(ua.Id);
                _userActivityRepo.Save();
            }
            

        }

        public IQueryable<UserActivity> GetAll(string userId)
        {
            return _userActivityRepo.GetAll().Where(
              x => x.UserId == userId );
        }

        public IQueryable<UserActivity> GetAll(string userId, ActivityItem item)
        {
            return _userActivityRepo.GetAll().Where(
              x => x.UserId == userId &&
              x.Item == item);
        }

        public IQueryable<UserActivity> GetAll(string userId, ActivityItem item, ActivityAction action)
        {
            return _userActivityRepo.GetAll().Where(
                x => x.UserId == userId &&
                x.Item == item &&
                x.Action == action);
        }

        public bool Exists(string userId, ActivityItem item, ActivityAction action, int itemId)
        {
            var result = _userActivityRepo.GetAll().FirstOrDefault(
                x => x.UserId == userId &&
                     x.Item == item &&
                     x.Action == action &&
                     x.ItemId == itemId);

            return result != null;
        }
    }
}