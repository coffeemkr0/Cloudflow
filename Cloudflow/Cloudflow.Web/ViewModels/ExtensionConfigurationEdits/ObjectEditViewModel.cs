using Cloudflow.Web.Utility.HtmlHelpers;
using System.Collections.Generic;

namespace Cloudflow.Web.ViewModels.ExtensionConfigurationEdits
{
    public class ObjectEditViewModel
    {
        #region Properties
        public object Model { get; set; }

        public PropertyCollection PropertyCollection { get; set; }

        public List<string> PropertyNameParts { get; set; }
        #endregion

        #region Constructors
        public ObjectEditViewModel()
        {
            PropertyNameParts = new List<string>();
        }
        #endregion
    }
}