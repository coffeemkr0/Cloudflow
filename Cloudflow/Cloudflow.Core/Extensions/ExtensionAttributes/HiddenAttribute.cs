using System;

namespace Cloudflow.Core.Extensions.ExtensionAttributes
{
    /// <summary>
    /// Specifies that an extension property should be hidden in a dynamically created user interface
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class HiddenAttribute : Attribute
    {

    }
}
