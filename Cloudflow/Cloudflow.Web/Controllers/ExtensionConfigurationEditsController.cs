using Cloudflow.Web.ViewModels.ExtensionConfigurationEdits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cloudflow.Web.Controllers
{
    public class ExtensionConfigurationEditsController : Controller
    {
        #region String Collection Edit
        public ActionResult StringCollectionEditItem(string propertyName, int index)
        {
            var model = new StringCollectionEditItemViewModel
            {
                PropertyName = propertyName,
                ItemIndex = index,
                Value = ""
            };

            return PartialView(model);
        }
        #endregion
    }
}