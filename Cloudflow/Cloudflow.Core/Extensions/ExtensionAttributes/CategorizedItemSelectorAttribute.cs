using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions.ExtensionAttributes
{
    public class CategorizedItemSelectorAttribute : Attribute
    {
        #region Properties
        public string CaptionResourceName { get; set; }

        public string CategoriesCaptionResourceName { get; set; }
        #endregion

        #region Constructors
        public CategorizedItemSelectorAttribute(string captionResourceName, string categoriesCaptionResourceName)
        {
            this.CaptionResourceName = captionResourceName;
            this.CategoriesCaptionResourceName = categoriesCaptionResourceName;
        }
        #endregion
    }
}
