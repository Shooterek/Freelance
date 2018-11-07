using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Freelance.Utilities
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString CheckBoxListForEnum<TModel, TValue>(this HtmlHelper html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null, bool sortAlphabetically = false) where TModel : class
        {
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, (ViewDataDictionary<TModel>)html.ViewData);
            var value = metadata.Model;

            // Get all enum values
            IEnumerable<TValue> values = Enum.GetValues(typeof(TValue)).Cast<TValue>();

            // Sort them alphabetically by enum name
            if (sortAlphabetically)
                values = values.OrderBy(i => i.ToString());

            // Create checkbox list
            var sb = new StringBuilder();
            foreach (var item in values)
            {
                TagBuilder builder = new TagBuilder("input");
                long targetValue = Convert.ToInt64(item);
                long flagValue = Convert.ToInt64(value);

                if ((targetValue & flagValue) == targetValue)
                    builder.MergeAttribute("checked", "checked");

                builder.MergeAttribute("type", "checkbox");
                builder.MergeAttribute("value", item.ToString());
                builder.MergeAttribute("name", fullBindingName);
                builder.MergeAttribute("id", item.ToString());

                // Add optional html attributes
                if (htmlAttributes != null)
                    builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

                sb.Append(builder.ToString(TagRenderMode.Normal));

                var labelBuilder = new TagBuilder("label");
                labelBuilder.MergeAttribute("for", item.ToString());
                labelBuilder.InnerHtml = item.GetType()?
                                             .GetMember(item.ToString())?
                                             .First()?
                                             .GetCustomAttribute<DisplayAttribute>()?
                                             .Name;

                sb.Append(labelBuilder.ToString(TagRenderMode.Normal));

                // Seperate checkboxes by new line
                sb.Append("<br />");
            }

            return new MvcHtmlString(sb.ToString());
        }
    }
}