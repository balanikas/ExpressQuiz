using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressQuiz.ViewModels;

namespace ExpressQuiz.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index(int statusCode, Exception exception, bool isAjaxRequet)
        {
            Response.StatusCode = statusCode;

            if (!isAjaxRequet)
            {
                var model = new ErrorViewModel { HttpStatusCode = statusCode, Exception = exception };

                return View(model);
            }
            else
            {
                var errorObjet = new { message = exception.Message };
                return Json(errorObjet, JsonRequestBehavior.AllowGet);
            }
        }
    }
}