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
    public static class JobEditorHelper
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
            if (Attribute.IsDefined(propertyInfo, typeof(HiddenAttribute)))
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

        private static PropertyTypes GetCollectionItemType(PropertyInfo propertyInfo)
        {
            var listType = propertyInfo.PropertyType.GetGenericArguments().Single();

            if (listType.IsTextType())
            {
                return PropertyTypes.Text;
            }

            if (listType.IsNumericType())
            {
                return PropertyTypes.Number;
            }

            if (listType.IsCollection())
            {
                return PropertyTypes.Collection;
            }

            //If the property type has properties itself, we can consider it a complex type
            if (listType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Any())
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

        private static string GetLabelText(PropertyInfo propertyInfo, ResourceManager resourceManager)
        {
            var labelTextAttribute = (LabelTextResourceAttribute)propertyInfo.GetCustomAttribute(typeof(LabelTextResourceAttribute));
            if (labelTextAttribute != null && resourceManager != null)
            {
                return resourceManager.GetString(labelTextAttribute.ResourceName);
            }
            else
            {
                return propertyInfo.Name;
            }
        }

        private static string GetDisplayText(object model)
        {
            if (model == null) return "Null";

            var attribute = (DisplayTextPropertyName)model.GetType().GetCustomAttribute(typeof(DisplayTextPropertyName));
            if (attribute != null)
            {
                var propertyNameParts = attribute.PropertyName.Split('.').ToList();
                object value = model;

                while(propertyNameParts.Count > 0)
                {
                    var propertyInfo = value.GetType().GetProperty(propertyNameParts.First());
                    value = propertyInfo.GetValue(value);
                    propertyNameParts.RemoveAt(0);
                }

                return value?.ToString() ?? model.GetType().Name;
            }
            else
            {
                return model.GetType().Name;
            }
        }

        private static string ObjectEdit(HtmlHelper htmlHelper, object model, List<string> propertyNameParts)
        {
            StringBuilder htmlStringBuilder = new StringBuilder();

            var resourceManager = LoadResources(model.GetType());

            var propertyCollection = new PropertyCollection(model, resourceManager);

            //Render property groups as a tab header
            if (propertyCollection.GroupedProperties.Count > 0)
            {
                htmlStringBuilder.AppendLine(TabHeader(htmlHelper, propertyCollection.GroupedProperties));
            }

            //Render hidden properties as hidden inputs
            foreach (var propertyInfo in propertyCollection.HiddenProperties)
            {
                var thisPropertyNameParts = new List<string>();
                thisPropertyNameParts.AddRange(propertyNameParts);
                thisPropertyNameParts.Add(propertyInfo.Name);

                htmlStringBuilder.AppendLine(Input(thisPropertyNameParts, propertyInfo.GetValue(model)?.ToString() ?? "", InputTypes.Hidden));
            }

            //Render ungrouped properties first
            foreach (var propertyInfo in propertyCollection.UngroupedProperties)
            {
                var thisPropertyNameParts = new List<string>();
                thisPropertyNameParts.AddRange(propertyNameParts);
                thisPropertyNameParts.Add(propertyInfo.Name);

                htmlStringBuilder.AppendLine(PropertyEdit(htmlHelper, model, resourceManager, propertyInfo, thisPropertyNameParts));
            }

            //Render grouped properties as tab panes
            foreach (var propertyGroup in propertyCollection.GroupedProperties)
            {
                htmlStringBuilder.AppendLine(TabPane(htmlHelper, propertyGroup, model, propertyNameParts, propertyCollection.GroupedProperties.IndexOf(propertyGroup) == 0));
            }

            return htmlStringBuilder.ToString();
        }

        private static string PropertyEdit(HtmlHelper htmlHelper, object model, ResourceManager resourceManager, PropertyInfo propertyInfo, List<string> propertyNameParts)
        {
            StringBuilder htmlStringBuilder = new StringBuilder();

            switch (GetPropertyType(propertyInfo))
            {
                case PropertyTypes.Text:
                    htmlStringBuilder.AppendLine(TextEdit(propertyNameParts, propertyInfo, model, resourceManager));
                    break;
                case PropertyTypes.Number:
                    htmlStringBuilder.AppendLine(NumericEdit(propertyNameParts, propertyInfo, model, resourceManager));
                    break;
                case PropertyTypes.Collection:
                    htmlStringBuilder.AppendLine(CollectionEdit(htmlHelper, propertyInfo, model, propertyNameParts));
                    break;
                case PropertyTypes.Complex:
                    htmlStringBuilder.AppendLine(ObjectEdit(htmlHelper, propertyInfo.GetValue(model), propertyNameParts));
                    break;
                case PropertyTypes.Unknown:
                    htmlStringBuilder.AppendLine(EditorNotImplemented(PropertyTypes.Unknown, propertyInfo));
                    break;
            }

            return htmlStringBuilder.ToString();
        }

        private static string TabHeader(HtmlHelper htmlHelper, List<PropertyCollection.PropertyGroup> propertyGroups)
        {
            StringBuilder htmlStringBuilder = new StringBuilder();

            var tabHeaderModel = new TabHeaderViewModel();
            foreach (var propertyGroup in propertyGroups)
            {
                tabHeaderModel.Items.Add(new TabHeaderViewModel.TabHeaderItem
                {
                    Id = propertyGroup.GroupId,
                    Active = propertyGroups.IndexOf(propertyGroup) == 0,
                    DisplayText = propertyGroup.DisplayText
                });
            }

            htmlStringBuilder.AppendLine(GetView(htmlHelper, "~/Views/ExtensionConfigurationEdits/TabHeader.cshtml", tabHeaderModel));

            return htmlStringBuilder.ToString();
        }

        private static string TabPane(HtmlHelper htmlHelper, PropertyCollection.PropertyGroup propertyGroup, object model, List<string> propertyNameParts, bool active)
        {
            StringBuilder htmlStringBuilder = new StringBuilder();

            var tabPaneModel = new TabPaneViewModel
            {
                Id = propertyGroup.GroupId,
                Active = active,
                Model = model,
                PropertyNameParts = propertyNameParts,
                Properties = propertyGroup.Properties
            };

            htmlStringBuilder.AppendLine(GetView(htmlHelper, "~/Views/ExtensionConfigurationEdits/TabPane.cshtml", tabPaneModel));

            return htmlStringBuilder.ToString();
        }

        private static string Label(List<string> propertyNameParts, PropertyInfo propertyInfo, ResourceManager resourceManager)
        {
            StringBuilder htmlStringBuilder = new StringBuilder();

            var tagBuilder = new TagBuilder("label");
            var name = string.Join(".", propertyNameParts);
            tagBuilder.MergeAttribute("for", name);

            tagBuilder.SetInnerText(GetLabelText(propertyInfo, resourceManager));

            htmlStringBuilder.AppendLine(tagBuilder.ToString(TagRenderMode.Normal));

            return htmlStringBuilder.ToString();
        }

        private static string Input(List<string> propertyNameParts, string value, InputTypes inputType)
        {
            var tagBuilder = new TagBuilder("input");

            var id = string.Join("_", propertyNameParts);
            tagBuilder.MergeAttribute("id", id);

            var name = string.Join(".", propertyNameParts);
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

        private static string NumericEdit(List<string> propertyNameParts, PropertyInfo propertyInfo, object objectInstance, ResourceManager resourceManager)
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("form-group");

            tagBuilder.InnerHtml = Label(propertyNameParts, propertyInfo, resourceManager);
            var value = propertyInfo.GetValue(objectInstance);
            tagBuilder.InnerHtml += Input(propertyNameParts, value == null ? "" : value.ToString(), InputTypes.Numeric);

            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        private static string TextEdit(List<string> propertyNameParts, PropertyInfo propertyInfo, object objectInstance, ResourceManager resourceManager)
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("form-group");

            tagBuilder.InnerHtml = Label(propertyNameParts, propertyInfo, resourceManager);
            var value = propertyInfo.GetValue(objectInstance);
            tagBuilder.InnerHtml += Input(propertyNameParts, value == null ? "" : value.ToString(), InputTypes.Text);

            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        private static string CollectionEdit(HtmlHelper htmlHelper, PropertyInfo propertyInfo, object objectInstance, List<string> propertyNameParts)
        {
            var listType = propertyInfo.PropertyType.GetGenericArguments().Single();
            var resourceManager = LoadResources(listType);

            switch (GetCollectionItemType(propertyInfo))
            {
                case PropertyTypes.Text:
                    return StringCollectionEdit(htmlHelper, propertyInfo, objectInstance, propertyNameParts, resourceManager);
                case PropertyTypes.Number:
                    return EditorNotImplemented(PropertyTypes.Collection, propertyInfo);
                case PropertyTypes.Collection:
                    return EditorNotImplemented(PropertyTypes.Collection, propertyInfo);
                case PropertyTypes.Complex:
                    return ComplexCollectionEdit(htmlHelper, propertyInfo, objectInstance, propertyNameParts, resourceManager);
                default:
                    return EditorNotImplemented(PropertyTypes.Collection, propertyInfo);
            }
        }

        private static string ComplexCollectionEdit(HtmlHelper htmlHelper, PropertyInfo propertyInfo, object objectInstance, List<string> propertyNameParts, ResourceManager resourceManager)
        {
            StringBuilder htmlStringBuilder = new StringBuilder();

            var model = new ComplexCollectionEditViewModel
            {
                LabelText = GetLabelText(propertyInfo, resourceManager),
                PropertyNameParts = propertyNameParts
            };

            var index = 0;
            foreach (var item in (IEnumerable)propertyInfo.GetValue(objectInstance))
            {
                var itemPropertyNameParts = new List<string>();
                itemPropertyNameParts.AddRange(propertyNameParts);
                var itemPropertyName = itemPropertyNameParts.LastOrDefault();
                if (itemPropertyName != null) itemPropertyName += $"[{index}]";

                model.Items.Add(new ComplexCollectionEditItemViewModel
                {
                    DisplayText = GetDisplayText(item),
                    Active = index == 0,
                    PropertyNameParts = itemPropertyNameParts,
                    Value = item
                });

                index += 1;
            }

            htmlStringBuilder.AppendLine(GetView(htmlHelper, "~/Views/ExtensionConfigurationEdits/ComplexCollectionEdit.cshtml", model));

            return htmlStringBuilder.ToString();
        }

        private static string StringCollectionEdit(HtmlHelper htmlHelper, PropertyInfo propertyInfo, object objectInstance, List<string> propertyNameParts, ResourceManager resourceManager)
        {
            StringBuilder htmlStringBuilder = new StringBuilder();

            var model = new StringCollectionEditViewModel
            {
                LabelText = GetLabelText(propertyInfo, resourceManager),
                PropertyName = string.Join(".", propertyNameParts)
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

        private static string EditorNotImplemented(PropertyTypes propertyType, PropertyInfo propertyInfo)
        {
            var tagBuilder = new TagBuilder("h3");

            tagBuilder.InnerHtml = $"There is no editor implemented for the {propertyType} property type - {propertyInfo.PropertyType}";

            return tagBuilder.ToString(TagRenderMode.Normal);
        }
        #endregion

        #region Public Methods
        public static MvcHtmlString CreateModelEdit(this HtmlHelper htmlHelper, object model)
        {
            return CreateModelEdit(htmlHelper, model, new List<string>());
        }

        public static MvcHtmlString CreatePropertyEdit(this HtmlHelper htmlHelper, object model, PropertyInfo propertyInfo, List<string> propertyNameParts)
        {
            var resourceManager = LoadResources(model.GetType());
            return MvcHtmlString.Create(PropertyEdit(htmlHelper, model, resourceManager, propertyInfo, propertyNameParts));
        }

        public static MvcHtmlString CreateModelEdit(this HtmlHelper htmlHelper, object model, List<string> propertyNameParts)
        {
            return MvcHtmlString.Create(ObjectEdit(htmlHelper, model, propertyNameParts));
        }
        #endregion
    }
}