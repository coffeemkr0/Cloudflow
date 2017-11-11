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

        public Dictionary<string,List<PropertyInfo>> GroupedProperties { get; set; }
        #endregion

        #region Constructors
        public PropertyCollection(object model, ResourceManager resourceManager)
        {
            this.HiddenProperties = new List<PropertyInfo>();
            this.UngroupedProperties = new List<PropertyInfo>();
            this.GroupedProperties = new Dictionary<string, List<PropertyInfo>>();

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
                        if (!this.GroupedProperties.ContainsKey(propertyGroupText))
                        {
                            this.GroupedProperties.Add(propertyGroupText, new List<PropertyInfo>());
                        }
                        this.GroupedProperties[propertyGroupText].Add(property);
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
    }
}