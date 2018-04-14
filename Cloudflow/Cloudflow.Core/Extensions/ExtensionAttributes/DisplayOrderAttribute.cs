using System;

namespace Cloudflow.Core.Extensions.ExtensionAttributes
{
    /// <inheritdoc />
    /// <summary>
    /// Specifies the display order for a property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DisplayOrderAttribute : Attribute
    {
        #region Properties
        /// <summary>
        /// Gets the sort order for the property
        /// </summary>
        public int Order { get; }
        #endregion

        #region Constructors
        public DisplayOrderAttribute(int order)
        {
            Order = order;
        }
        #endregion
    }
}
