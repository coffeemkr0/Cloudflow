using System;

namespace Cloudflow.Core.Extensions.ExtensionAttributes
{
    /// <summary>
    /// Specifies the name of a resource entry to load and use as the text for the label of a property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class LabelTextResourceAttribute : Attribute
    {
        #region Properties
        /// <summary>
        /// Gets the name of the resource entry to use.
        /// </summary>
        public string ResourceName { get; }
        #endregion

        #region Constructors
        public LabelTextResourceAttribute(string resourceName)
        {
            ResourceName = resourceName;
        }
        #endregion
    }
}
