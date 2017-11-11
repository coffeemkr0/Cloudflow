using Cloudflow.Web.Utility.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            this.PropertyNameParts = new List<string>();
        }
        #endregion
    }
}