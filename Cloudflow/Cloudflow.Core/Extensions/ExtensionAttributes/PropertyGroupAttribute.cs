using System;

namespace Cloudflow.Core.Extensions.ExtensionAttributes
{
    /// <summary>
    /// Specifies the group that a property belongs to.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyGroupAttribute : Attribute
    {
        #region Properties
        /// <summary>
        /// Gets the name of the resource string to use as the display text for the group.
        /// This is also used to identify the group that the property belongs to.
        /// </summary>
        public string GroupTextResourceName { get; }
        #endregion

        #region Constructors
        public PropertyGroupAttribute(string groupTextResourceName)
        {
            GroupTextResourceName = groupTextResourceName;
        }
        #endregion
    }
}
