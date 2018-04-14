using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cloudflow.Web.ViewModels.ExtensionConfigurationEdits
{
    public class TabPaneViewModel
    {
        #region Properties
        public Guid Id { get; set; }

        public bool Active { get; set; }

        public object Model { get; set; }

        public List<string> PropertyNameParts { get; set; }

        public List<PropertyInfo> Properties { get; set; }
        #endregion

        #region Constructors
        public TabPaneViewModel()
        {
            PropertyNameParts = new List<string>();
            Properties = new List<PropertyInfo>();
        }
        #endregion
    }
}