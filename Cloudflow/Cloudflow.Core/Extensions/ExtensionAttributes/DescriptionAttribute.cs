using System;

namespace Cloudflow.Core.Extensions.ExtensionAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class DescriptionAttribute : Attribute
    {
        #region Properties
        public string Description { get; set; }
        #endregion

        #region Constructors
        public DescriptionAttribute(string description)
        {
            this.Description = description;
        }
        #endregion
    }
}
