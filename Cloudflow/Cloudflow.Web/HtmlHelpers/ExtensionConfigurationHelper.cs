using Cloudflow.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Cloudflow.Web.HtmlHelpers
{
    public static class ExtensionConfigurationHelper
    {
        #region Enums
        private enum PropertyTypes
        {
            Text,
            Number,
            Complex,
            Unknown
        }
        #endregion

        #region Private Members
        private static HashSet<Type> _textTypes = new HashSet<Type>
        {
            typeof(string),
            typeof(Guid)
        };

        private static HashSet<Type> _numericTypes = new HashSet<Type>
        {
            typeof(byte),
            typeof(sbyte),
            typeof(UInt16),
            typeof(UInt32),
            typeof(UInt64),
            typeof(Int16),
            typeof(Int32),
            typeof(Int64),
            typeof(decimal),
            typeof(double),
            typeof(Single),
        };
        #endregion

        #region Private Methods
        private static bool IsTextType(Type type)
        {
            //Check to see if the type is text or if it's nullable text
            return _textTypes.Contains(type) ||
                   _textTypes.Contains(Nullable.GetUnderlyingType(type));
        }

        private static bool IsNumericType(Type type)
        {
            //Check to see if the type is numeric or if it's a nullable numeric
            return _numericTypes.Contains(type) ||
                   _numericTypes.Contains(Nullable.GetUnderlyingType(type));
        }

        private static PropertyTypes GetPropertyType(PropertyInfo propertyInfo)
        {
            if (IsTextType(propertyInfo.PropertyType))
            {
                return PropertyTypes.Text;
            }

            if (IsNumericType(propertyInfo.PropertyType))
            {
                return PropertyTypes.Number;
            }

            //If the property type has properties itself, we can consider it a complex type
            if(propertyInfo.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Any())
            {
                return PropertyTypes.Complex;
            }

            //Otherwise we don't know what to do with it
            return PropertyTypes.Unknown;
        }

        private static string TextEdit(PropertyInfo propertyInfo, object objectInstance)
        {
            StringBuilder htmlStringBuilder = new StringBuilder();

            var tagBuilder = new TagBuilder("label");
            htmlStringBuilder.AppendLine(tagBuilder.ToString(TagRenderMode.StartTag));
            tagBuilder.SetInnerText(propertyInfo.Name);
            htmlStringBuilder.AppendLine(tagBuilder.ToString(TagRenderMode.Normal));
            htmlStringBuilder.AppendLine(tagBuilder.ToString(TagRenderMode.EndTag));

            tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttribute("name", "Name");
            tagBuilder.MergeAttribute("type", "text");
            tagBuilder.MergeAttribute("value", propertyInfo.GetValue(objectInstance).ToString());
            htmlStringBuilder.AppendLine(tagBuilder.ToString(TagRenderMode.SelfClosing));

            return htmlStringBuilder.ToString();
        }
        #endregion


        public static MvcHtmlString ExtensionConfiguration(this HtmlHelper htmlHelper, ExtensionConfiguration configuration)
        {
            StringBuilder htmlStringBuilder = new StringBuilder();

            var tagBuilder = new TagBuilder("section");

            htmlStringBuilder.AppendLine(tagBuilder.ToString(TagRenderMode.StartTag));

            foreach (var propertyInfo in configuration.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
            {
                switch (GetPropertyType(propertyInfo))
                {
                    case PropertyTypes.Text:
                        htmlStringBuilder.AppendLine(TextEdit(propertyInfo, configuration));
                        break;
                    case PropertyTypes.Number:
                        break;
                    case PropertyTypes.Complex:
                        break;
                    case PropertyTypes.Unknown:

                        break;
                }
            }

            htmlStringBuilder.AppendLine(tagBuilder.ToString(TagRenderMode.EndTag));

            return MvcHtmlString.Create(htmlStringBuilder.ToString());
        }
    }
}