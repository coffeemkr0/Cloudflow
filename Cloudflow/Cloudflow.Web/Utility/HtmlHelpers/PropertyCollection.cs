using Cloudflow.Core.Extensions.ExtensionAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Web;

namespace Cloudflow.Web.Utility.HtmlHelpers
{
    public class PropertyCollection
    {
        #region Properties
        public List<PropertyInfo> HiddenProperties { get; set; }

        public List<PropertyInfo> UngroupedProperties { get; set; }

        public List<PropertyGroup> GroupedProperties { get; set; }
        #endregion

        #region Constructors
        public PropertyCollection(object model, ResourceManager resourceManager)
        {
            this.HiddenProperties = new List<PropertyInfo>();
            this.UngroupedProperties = new List<PropertyInfo>();
            this.GroupedProperties = new List<PropertyGroup>();

            var sortedProperties = GetSortedProperties(model.GetType());

            foreach (var property in sortedProperties)
            {
                if (property.GetCustomAttribute(typeof(HiddenAttribute)) != null)
                {
                    this.HiddenProperties.Add(property);
                }
                else
                {
                    var propertyGroupText = GetPropertyGroupText(property, resourceManager);

                    if (string.IsNullOrWhiteSpace(propertyGroupText))
                    {
                        this.UngroupedProperties.Add(property);
                    }
                    else
                    {
                        var propertyGroup = this.GroupedProperties.FirstOrDefault(i => i.DisplayText == propertyGroupText);

                        if (propertyGroup == null)
                        {
                            propertyGroup = new PropertyGroup
                            {
                                DisplayText = propertyGroupText
                            };
                            this.GroupedProperties.Add(propertyGroup);
                        }

                        propertyGroup.Properties.Add(property);
                    }
                }
            }
        }
        #endregion

        #region Private Methods
        private List<PropertyInfo> GetSortedProperties(Type type)
        {
            return type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).
                Select(i => new
                {
                    Property = i,
                    Attribute = (DisplayOrderAttribute)Attribute.GetCustomAttribute(i, typeof(DisplayOrderAttribute), true)
                })
                .OrderBy(i => i.Attribute != null ? i.Attribute.Order : 0)
                .Select(i => i.Property)
                .ToList();
        }

        private string GetPropertyGroupText(PropertyInfo propertyInfo, ResourceManager resourceManager)
        {
            var attribute = (PropertyGroupAttribute)propertyInfo.GetCustomAttribute(typeof(PropertyGroupAttribute));
            if (attribute != null && resourceManager != null)
            {
                return resourceManager.GetString(attribute.GroupTextResourceName);
            }

            return "";
        }
        #endregion

        #region PropertyGroup
        public class PropertyGroup
        {
            #region Properties
            public Guid GroupId { get; }

            public string DisplayText { get; set; }

            public List<PropertyInfo> Properties { get; set; }
            #endregion

            #region Constructors
            public PropertyGroup()
            {
                this.GroupId = Guid.NewGuid();
                this.Properties = new List<PropertyInfo>();
            }
            #endregion
        }
        #endregion
    }
}