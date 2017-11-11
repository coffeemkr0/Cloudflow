using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions.ExtensionAttributes
{
    /// <summary>
    /// Specifies which property to use as the display text for an instance of the class when it
    /// is presented as text. For example, spcifying the Name property of a class so that
    /// the Name will be used when rendering a navigation item for the object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DisplayTextPropertyName : Attribute
    {
        #region Properties
        /// <summary>
        /// Gets the name of the property to use for the display text
        /// </summary>
        public string PropertyName { get; }
        #endregion

        #region Constructors
        public DisplayTextPropertyName(string propertyName)
        {
            this.PropertyName = propertyName;
        }
        #endregion
    }
}
