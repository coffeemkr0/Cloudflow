using Cloudflow.Web.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Home
{
    public class IndexViewModel
    {
        #region Properties
        public ExtensionBrowserViewModel ExtensionBrowserViewModel { get; set; }
        #endregion

        #region Constructors
        public IndexViewModel()
        {
            
        }
        #endregion
    }
}