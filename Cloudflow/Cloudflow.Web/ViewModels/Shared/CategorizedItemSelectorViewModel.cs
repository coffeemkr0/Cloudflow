using Cloudflow.Core.Extensions.ExtensionAttributes;

namespace Cloudflow.Web.ViewModels.Shared
{
    public class CategorizedItemSelectorViewModel
    {
        #region Properties
        public CategorizedItemCollection CategorizedItemCollection { get; set; }

        public string Caption { get; set; }

        public string CategoriesCaption { get; set; }
        #endregion

        #region Constructors
        public CategorizedItemSelectorViewModel()
        {

        }
        #endregion
    }
}