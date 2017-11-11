using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions.ExtensionAttributes
{
    /// <summary>
    /// Specifies that the property should display its configuration on a tab in a tab control.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CreateTabAttribute : Attribute
    {
        #region Properties
        /// <summary>
        /// Gets the name of the resource string to use for the text on the tab.
        /// </summary>
        public string TabTextResourceName { get; }
        #endregion

        #region Constructors
        public CreateTabAttribute(string tabTextResourceName)
        {
            this.TabTextResourceName = tabTextResourceName;
        }
        #endregion
    }
}
