using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Services;

namespace ExpressQuiz.Controllers
{
    public class RatingController : Controller
    {
        private readonly IUserActivityService _userActivityService;

        public RatingController(IUserActivityService userActivityService)
        {
            _userActivityService = userActivityService;
        }
       
        [HttpPost]
        public HttpStatusCodeResult RateQuiz(int? id, int vote )
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (vote != -1 && vote != 0 && vote != 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = String.IsNullOrEmpty(User.Identity.Name) ? Session.SessionID : User.Identity.Name;
            _userActivityService.UpdateVote(userId, ActivityItem.Quiz, id.Value, vote);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpStatusCodeResult RateQuestion(int? id, int vote)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (vote != -1 && vote != 0 && vote != 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = String.IsNullOrEmpty(User.Identity.Name) ? Session.SessionID : User.Identity.Name;
           _userActivityService.UpdateVote(userId,ActivityItem.Question, id.Value,vote);
          
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}