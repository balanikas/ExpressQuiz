using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ExpressQuiz
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ValidateAntiForgeryTokenOnAllPosts : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;


            if (request.HttpMethod == WebRequestMethods.Http.Post)
            {
                try
                {
                    new ValidateAntiForgeryTokenAttribute()
                        .OnAuthorization(filterContext);
                }
                catch (Exception)
                {

                    if (request.IsAjaxRequest())
                    {
                        var antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];

                        var cookieValue = antiForgeryCookie != null
                            ? antiForgeryCookie.Value
                            : null;

                        AntiForgery.Validate(cookieValue, request.Headers["__RequestVerificationToken"]);
                    }
                    
                }
                

               
            }
        }
    }
}