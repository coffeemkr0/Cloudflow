using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using Cloudflow.Web.ViewModels.ExtensionConfigurationEdits;
using Cloudflow.Web.ViewModels.Jobs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
            Collection,
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

        public static PropertyInfo[] GetSortedProperties(this Type type)
        {
            return type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).
                Select(i => new
                {
                    Property = i,
                    Attribute = (DisplayOrderAttribute)Attribute.GetCustomAttribute(i, typeof(DisplayOrderAttribute), true)
                })
                .OrderBy(i => i.Attribute != null ? i.Attribute.Order : 0)
                .Select(i => i.Property)
                .ToArray();
        }

        private static bool IsTextType(this Type type)
        {
            //Check to see if the type is text or if it's nullable text
            return _textTypes.Contains(type) ||
                   _textTypes.Contains(Nullable.GetUnderlyingType(type));
        }

        private static bool IsNumericType(this Type type)
        {
            //Check to see if the type is numeric or if it's a nullable numeric
            return _numericTypes.Contains(type) ||
                   _numericTypes.Contains(Nullable.GetUnderlyingType(type));
        }

        private static bool IsCollection(this Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type) ||
                type.GetInterface(typeof(IEnumerable<>).FullName) != null;
        }

        private static PropertyTypes GetPropertyType(PropertyInfo propertyInfo)
        {
            if(Attribute.IsDefined(propertyInfo, typeof(HiddenAttribute)))
            {
                return PropertyTypes.Hidden;
            }

            if (propertyInfo.PropertyType.IsTextType())
            {
                return PropertyTypes.Text;
            }

            if (propertyInfo.PropertyType.IsNumericType())
            {
                return PropertyTypes.Number;
            }

            if (propertyInfo.PropertyType.IsCollection())
            {
                return PropertyTypes.Collection;
            }

            //If the property type has properties itself, we can consider it a complex type
            if (propertyInfo.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Any())
            {
                return PropertyTypes.Complex;
            }

            //Otherwise we don't know what to do with it at this point
            return PropertyTypes.Unknown;
        }

        private static string GetView(HtmlHelper htmlHelper, string name, object model)
        {
            htmlHelper.ViewContext.Controller.ViewData.Model = model;
            var result = ViewEngines.Engines.FindPartialView(htmlHelper.ViewContext.Controller.ControllerContext, name);
            using (var writer = new StringWriter())
            {
                var viewContext = new ViewContext(htmlHelper.ViewContext.Controller.ControllerContext, result.View,
                    htmlHelper.ViewContext.Controller.ViewData, htmlHelper.ViewContext.Controller.TempData, writer);

                result.View.Render(viewContext, writer);
                var html = writer.GetStringBuilder().ToString();
                return html;
            }
        }

        private static string Label(List<string> prefixes, PropertyInfo propertyInfo, ResourceManager resourceManager)
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

        private static string Input(List<string> prefixes, string propertyName, string value, InputTypes inputType)
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

        private static string HiddenInput(List<string> prefixes, PropertyInfo propertyInfo, object objectInstance)
        {
            var value = propertyInfo.GetValue(objectInstance);
            return Input(prefixes, propertyInfo.Name, value == null ? "" : value.ToString(), InputTypes.Hidden);
        }

        private static string NumericEdit(List<string> prefixes, PropertyInfo propertyInfo, object objectInstance, ResourceManager resourceManager)
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("form-group");

            tagBuilder.InnerHtml = Label(prefixes, propertyInfo, resourceManager);
            var value = propertyInfo.GetValue(objectInstance);
            tagBuilder.InnerHtml += Input(prefixes, propertyInfo.Name, value == null ? "" : value.ToString(), InputTypes.Numeric);

            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        private static string TextEdit(List<string> prefixes, PropertyInfo propertyInfo, object objectInstance, ResourceManager resourceManager)
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("form-group");
            
            tagBuilder.InnerHtml = Label(prefixes, propertyInfo, resourceManager);
            var value = propertyInfo.GetValue(objectInstance);
            tagBuilder.InnerHtml += Input(prefixes, propertyInfo.Name, value == null ? "" : value.ToString(), InputTypes.Text);

            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        private static string CollectionEdit(HtmlHelper htmlHelper, List<string> prefixes, PropertyInfo propertyInfo, object objectInstance, ResourceManager resourceManager)
        {
            StringBuilder htmlStringBuilder = new StringBuilder();

            var model = new StringCollectionEditViewModel
            {
                PropertyName = string.Join(".", prefixes) + "." + propertyInfo.Name,
            };
            var index = 0;
            foreach (var item in (IEnumerable)propertyInfo.GetValue(objectInstance))
            {
                model.Items.Add(new StringCollectionEditItemViewModel
                {
                    PropertyName = model.PropertyName,
                    ItemIndex = index++,
                    Value = item == null ? "" : item.ToString()
                });
            }

            htmlStringBuilder.AppendLine(GetView(htmlHelper, "~/Views/ExtensionConfigurationEdits/StringCollectionEdit.cshtml", model));

            return htmlStringBuilder.ToString();
        }

        private static string EditorNotImplemented(PropertyInfo propertyInfo)
        {
            var tagBuilder = new TagBuilder("h3");

            tagBuilder.InnerHtml = $"No editor available for type {propertyInfo.PropertyType}";

            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        #endregion

        public static MvcHtmlString ExtensionConfiguration(this HtmlHelper htmlHelper, ExtensionConfigurationViewModel configurationViewModel, string propertyNamePrefix)
        {
            return ExtensionConfiguration(htmlHelper, configurationViewModel, new List<string> { propertyNamePrefix });
        }

        public static MvcHtmlString ExtensionConfiguration(this HtmlHelper htmlHelper, ExtensionConfigurationViewModel configurationViewModel, List<string> propertyNamePrefixes)
        {
            StringBuilder htmlStringBuilder = new StringBuilder();

            var resourceManager = LoadResources(configurationViewModel.Configuration.ExtensionType);

            htmlStringBuilder.AppendLine(Input(propertyNamePrefixes,
                "ExtensionId", configurationViewModel.ExtensionId.ToString(), InputTypes.Hidden));
            htmlStringBuilder.AppendLine(Input(propertyNamePrefixes,
                "ExtensionAssemblyPath", configurationViewModel.ExtensionAssemblyPath, InputTypes.Hidden));

            htmlStringBuilder.AppendLine(Input(propertyNamePrefixes, 
                "ConfigurationExtensionId", configurationViewModel.ConfigurationExtensionId.ToString(), InputTypes.Hidden));
            htmlStringBuilder.AppendLine(Input(propertyNamePrefixes, 
                "ConfigurationExtensionAssemblyPath", configurationViewModel.ConfigurationExtensionAssemblyPath, InputTypes.Hidden));

            propertyNamePrefixes.Add("Configuration");

            foreach (var propertyInfo in configurationViewModel.Configuration.ExtensionType.GetSortedProperties())
            {
                switch (GetPropertyType(propertyInfo))
                {
                    case PropertyTypes.Hidden:
                        htmlStringBuilder.AppendLine(HiddenInput(propertyNamePrefixes, propertyInfo, configurationViewModel.Configuration));
                        break;
                    case PropertyTypes.Text:
                        htmlStringBuilder.AppendLine(TextEdit(propertyNamePrefixes, propertyInfo, configurationViewModel.Configuration, resourceManager));
                        break;
                    case PropertyTypes.Number:
                        htmlStringBuilder.AppendLine(NumericEdit(propertyNamePrefixes, propertyInfo, configurationViewModel.Configuration, resourceManager));
                        break;
                    case PropertyTypes.Collection:
                        htmlStringBuilder.AppendLine(CollectionEdit(htmlHelper, propertyNamePrefixes, propertyInfo, configurationViewModel.Configuration, resourceManager));
                        break;
                    case PropertyTypes.Complex:
                        htmlStringBuilder.AppendLine(EditorNotImplemented(propertyInfo));
                        break;
                    case PropertyTypes.Unknown:
                        htmlStringBuilder.AppendLine(EditorNotImplemented(propertyInfo));
                        break;
                }
            }

            return MvcHtmlString.Create(htmlStringBuilder.ToString());
        }
    }
}