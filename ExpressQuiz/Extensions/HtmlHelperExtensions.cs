﻿using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace ExpressQuiz.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString Slider<TModel, TValue>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression,
            object htmlAttributes = null)
        {
            var data = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            string propertyName = data.PropertyName;
            var input = new TagBuilder("input");
            input.Attributes.Add("name", propertyName);


            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                input.MergeAttributes(attributes);
            }

            return new MvcHtmlString(input.ToString());
        }
    }
}