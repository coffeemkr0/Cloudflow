using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions.ExtensionAttributes
{
    /// <summary>
    /// When an extension configuration has multiple properties, this attribute specifies in
    /// what order the properties should be displayed
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
