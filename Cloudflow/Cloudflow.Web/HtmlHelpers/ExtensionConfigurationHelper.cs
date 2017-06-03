using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ConfigurationAttributes;
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
            Hidden,
            Text,
            Number,
            Complex,
            Unknown
        }

        private enum InputTypes
        {
            Hidden,
            Numeric,
            Text
        }
        #endregion

        #region Private Members
        private static readonly log4net.ILog _log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            if(Attribute.IsDefined(propertyInfo, typeof(HiddenAttribute)))
            {
                return PropertyTypes.Hidden;
            }

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

        private static string Label(PropertyInfo propertyInfo)
        {
            StringBuilder htmlStringBuilder = new StringBuilder();

            var tagBuilder = new TagBuilder("label");
            tagBuilder.MergeAttribute("for", propertyInfo.Name);
            tagBuilder.SetInnerText(propertyInfo.Name);
            htmlStringBuilder.AppendLine(tagBuilder.ToString(TagRenderMode.Normal));

            return htmlStringBuilder.ToString();
        }

        private static string Input(PropertyInfo propertyInfo, object objectInstance, InputTypes inputType)
        {
            StringBuilder htmlStringBuilder = new StringBuilder();

            var tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttribute("name", propertyInfo.Name);

            //TODO:This is not all of the supported input types
            switch (inputType)
            {
                case InputTypes.Hidden:
                    tagBuilder.MergeAttribute("type", "hidden");
                    break;
                case InputTypes.Numeric:
                    tagBuilder.MergeAttribute("type", "number");
                    break;
                case InputTypes.Text:
                    tagBuilder.MergeAttribute("type", "text");
                    break;
            }
            
            tagBuilder.MergeAttribute("value", propertyInfo.GetValue(objectInstance).ToString());
            htmlStringBuilder.AppendLine(tagBuilder.ToString(TagRenderMode.SelfClosing));

            return htmlStringBuilder.ToString();
        }

        private static string HiddenInput(PropertyInfo propertyInfo, object objectInstance)
        {
            StringBuilder htmlStringBuilder = new StringBuilder();

            htmlStringBuilder.AppendLine(Input(propertyInfo, objectInstance, InputTypes.Hidden));

            return htmlStringBuilder.ToString();
        }

        private static string NumericEdit(PropertyInfo propertyInfo, object objectInstance)
        {
            StringBuilder htmlStringBuilder = new StringBuilder();

            htmlStringBuilder.AppendLine(Label(propertyInfo));
            htmlStringBuilder.AppendLine(Input(propertyInfo, objectInstance, InputTypes.Numeric));

            return htmlStringBuilder.ToString();
        }

        private static string TextEdit(PropertyInfo propertyInfo, object objectInstance)
        {
            StringBuilder htmlStringBuilder = new StringBuilder();

            htmlStringBuilder.AppendLine(Label(propertyInfo));
            htmlStringBuilder.AppendLine(Input(propertyInfo, objectInstance, InputTypes.Text));

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
                    case PropertyTypes.Hidden:
                        htmlStringBuilder.AppendLine(HiddenInput(propertyInfo, configuration));
                        break;
                    case PropertyTypes.Text:
                        htmlStringBuilder.AppendLine(TextEdit(propertyInfo, configuration));
                        break;
                    case PropertyTypes.Number:
                        htmlStringBuilder.AppendLine(NumericEdit(propertyInfo, configuration));
                        break;
                    case PropertyTypes.Complex:
                        _log.Info($"A property type was encountered that is not implemented - { propertyInfo.PropertyType }");
                        break;
                    case PropertyTypes.Unknown:
                        _log.Info($"A property type was encountered that is not implemented - { propertyInfo.PropertyType }");
                        break;
                }
            }

            htmlStringBuilder.AppendLine(tagBuilder.ToString(TagRenderMode.EndTag));

            return MvcHtmlString.Create(htmlStringBuilder.ToString());
        }
    }
}