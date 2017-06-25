using System;

namespace Cloudflow.Core.Extensions.ExtensionAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class DisplayNameAttribute : Attribute
    {
        #region Properties
        public string DisplayName { get; set; }
        #endregion

        #region Constructors
        public DisplayNameAttribute(string displayName)
        {
            this.DisplayName = displayName;
        }
        #endregion
    }
}
