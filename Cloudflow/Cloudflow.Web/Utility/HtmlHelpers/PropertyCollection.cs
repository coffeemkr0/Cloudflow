using Cloudflow.Core.Extensions.ExtensionAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;

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
            HiddenProperties = new List<PropertyInfo>();
            UngroupedProperties = new List<PropertyInfo>();
            GroupedProperties = new List<PropertyGroup>();

            var sortedProperties = GetSortedProperties(model.GetType());

            foreach (var property in sortedProperties)
            {
                if (property.GetCustomAttribute(typeof(HiddenAttribute)) != null)
                {
                    HiddenProperties.Add(property);
                }
                else
                {
                    var propertyGroupText = GetPropertyGroupText(property, resourceManager);

                    if (string.IsNullOrWhiteSpace(propertyGroupText))
                    {
                        UngroupedProperties.Add(property);
                    }
                    else
                    {
                        var propertyGroup = GroupedProperties.FirstOrDefault(i => i.DisplayText == propertyGroupText);

                        if (propertyGroup == null)
                        {
                            propertyGroup = new PropertyGroup
                            {
                                DisplayText = propertyGroupText
                            };
                            GroupedProperties.Add(propertyGroup);
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
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public).
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
                GroupId = Guid.NewGuid();
                Properties = new List<PropertyInfo>();
            }
            #endregion
        }
        #endregion
    }
}