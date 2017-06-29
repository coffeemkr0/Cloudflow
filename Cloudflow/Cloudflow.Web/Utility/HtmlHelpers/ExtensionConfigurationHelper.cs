using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using Cloudflow.Web.ViewModels.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Cloudflow.Web.Utility.HtmlHelpers
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
        private static ResourceManager LoadResources(Type type)
        {
            var defaultResources = type.Assembly.GetManifestResourceNames().FirstOrDefault(i => i.Contains("Properties.Resources"));

            if (defaultResources != null)
            {
                var resourceBaseName = defaultResources.Remove(defaultResources.LastIndexOf("."));
                return new ResourceManager(resourceBaseName, type.Assembly);
            }
            return null;
        }

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

            //Otherwise we don't know what to do with it at this point
            return PropertyTypes.Unknown;
        }

        private static string Label(string[] prefixes, PropertyInfo propertyInfo, ResourceManager resourceManager)
        {
            StringBuilder htmlStringBuilder = new StringBuilder();

            var tagBuilder = new TagBuilder("label");
            var name = string.Join(".", prefixes) + "." + propertyInfo.Name;
            tagBuilder.MergeAttribute("for", name);

            var labelTextAttribute = (LabelTextResourceAttribute)propertyInfo.GetCustomAttribute(typeof(LabelTextResourceAttribute));
            if(labelTextAttribute != null && resourceManager != null)
            {
                tagBuilder.SetInnerText(resourceManager.GetString(labelTextAttribute.ResourceName));
            }
            else
            {
                tagBuilder.SetInnerText(propertyInfo.Name);
            }
            
            htmlStringBuilder.AppendLine(tagBuilder.ToString(TagRenderMode.Normal));

            return htmlStringBuilder.ToString();
        }

        private static string Input(string[] prefixes, string propertyName, string value, InputTypes inputType)
        {
            var tagBuilder = new TagBuilder("input");

            var id = string.Join("_", prefixes) + "_" + propertyName;
            tagBuilder.MergeAttribute("id", id);

            var name = string.Join(".", prefixes) + "." + propertyName;
            tagBuilder.MergeAttribute("name", name);

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
            
            tagBuilder.MergeAttribute("value", value);

            tagBuilder.AddCssClass("form-control");

            return tagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        private static string HiddenInput(string[] prefixes, PropertyInfo propertyInfo, object objectInstance)
        {
            var value = propertyInfo.GetValue(objectInstance);
            return Input(prefixes, propertyInfo.Name, value == null ? "" : value.ToString(), InputTypes.Hidden);
        }

        private static string NumericEdit(string[] prefixes, PropertyInfo propertyInfo, object objectInstance, ResourceManager resourceManager)
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("form-group");

            tagBuilder.InnerHtml = Label(prefixes, propertyInfo, resourceManager);
            var value = propertyInfo.GetValue(objectInstance);
            tagBuilder.InnerHtml += Input(prefixes, propertyInfo.Name, value == null ? "" : value.ToString(), InputTypes.Numeric);

            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        private static string TextEdit(string[] prefixes, PropertyInfo propertyInfo, object objectInstance, ResourceManager resourceManager)
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("form-group");
            
            tagBuilder.InnerHtml = Label(prefixes, propertyInfo, resourceManager);
            var value = propertyInfo.GetValue(objectInstance);
            tagBuilder.InnerHtml += Input(prefixes, propertyInfo.Name, value == null ? "" : value.ToString(), InputTypes.Text);

            return tagBuilder.ToString(TagRenderMode.Normal);
        }
        #endregion

        public static MvcHtmlString ExtensionConfiguration(this HtmlHelper htmlHelper, ExtensionConfigurationViewModel configurationViewModel, string viewModelPropertyName)
        {
            StringBuilder htmlStringBuilder = new StringBuilder();

            var resourceManager = LoadResources(configurationViewModel.Configuration.ExtensionType);

            htmlStringBuilder.AppendLine(Input(new string[] { viewModelPropertyName },
                "ExtensionId", configurationViewModel.ExtensionId.ToString(), InputTypes.Hidden));
            htmlStringBuilder.AppendLine(Input(new string[] { viewModelPropertyName },
                "ExtensionAssemblyPath", configurationViewModel.ExtensionAssemblyPath, InputTypes.Hidden));

            htmlStringBuilder.AppendLine(Input(new string[] { viewModelPropertyName }, 
                "ConfigurationExtensionId", configurationViewModel.ConfigurationExtensionId.ToString(), InputTypes.Hidden));
            htmlStringBuilder.AppendLine(Input(new string[] { viewModelPropertyName }, 
                "ConfigurationExtensionAssemblyPath", configurationViewModel.ConfigurationExtensionAssemblyPath, InputTypes.Hidden));

            foreach (var propertyInfo in configurationViewModel.Configuration.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
            {
                switch (GetPropertyType(propertyInfo))
                {
                    case PropertyTypes.Hidden:
                        htmlStringBuilder.AppendLine(HiddenInput(new string[] { viewModelPropertyName, "Configuration" }, propertyInfo, configurationViewModel.Configuration));
                        break;
                    case PropertyTypes.Text:
                        htmlStringBuilder.AppendLine(TextEdit(new string[] { viewModelPropertyName, "Configuration" }, propertyInfo, configurationViewModel.Configuration, resourceManager));
                        break;
                    case PropertyTypes.Number:
                        htmlStringBuilder.AppendLine(NumericEdit(new string[] { viewModelPropertyName, "Configuration" }, propertyInfo, configurationViewModel.Configuration, resourceManager));
                        break;
                    case PropertyTypes.Complex:
                        _log.Info($"A property type was encountered that is not implemented - { propertyInfo.PropertyType }");
                        break;
                    case PropertyTypes.Unknown:
                        _log.Info($"A property type was encountered that is not implemented - { propertyInfo.PropertyType }");
                        break;
                }
            }

            return MvcHtmlString.Create(htmlStringBuilder.ToString());
        }
    }
}