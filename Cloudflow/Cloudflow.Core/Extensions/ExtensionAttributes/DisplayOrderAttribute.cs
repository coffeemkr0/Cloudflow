using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions.ExtensionAttributes
{
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
            this.Order = order;
        }
        #endregion
    }
}
