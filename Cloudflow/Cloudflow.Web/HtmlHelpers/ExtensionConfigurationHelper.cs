using Cloudflow.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Cloudflow.Web.HtmlHelpers
{
    public static class ExtensionConfigurationHelper
    {
        private static string GetNameHtml(ExtensionConfiguration configuration)
        {
            StringBuilder htmlStringBuilder = new StringBuilder();

            var tagBuilder = new TagBuilder("label");
            htmlStringBuilder.AppendLine(tagBuilder.ToString(TagRenderMode.StartTag));
            tagBuilder.SetInnerText("Name");
            htmlStringBuilder.AppendLine(tagBuilder.ToString(TagRenderMode.Normal));
            htmlStringBuilder.AppendLine(tagBuilder.ToString(TagRenderMode.EndTag));

            tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttribute("name", "Name");
            tagBuilder.MergeAttribute("type", "text");
            tagBuilder.MergeAttribute("value", configuration.Name);
            htmlStringBuilder.AppendLine(tagBuilder.ToString(TagRenderMode.SelfClosing));

            return htmlStringBuilder.ToString();
        }

        public static MvcHtmlString ExtensionConfiguration(this HtmlHelper htmlHelper, ExtensionConfiguration configuration)
        {
            StringBuilder htmlStringBuilder = new StringBuilder();

            var tagBuilder = new TagBuilder("section");

            htmlStringBuilder.AppendLine(tagBuilder.ToString(TagRenderMode.StartTag));

            htmlStringBuilder.AppendLine(GetNameHtml(configuration));

            htmlStringBuilder.AppendLine(tagBuilder.ToString(TagRenderMode.EndTag));

            return MvcHtmlString.Create(htmlStringBuilder.ToString());
        }
    }
}