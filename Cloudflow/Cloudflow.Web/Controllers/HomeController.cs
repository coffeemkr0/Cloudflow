using Cloudflow.Core.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cloudflow.Web.Utility;
using Cloudflow.Web.ViewModels.Shared;
using Cloudflow.Web.ViewModels.Home;

namespace Cloudflow.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Triggers()
        {
            var model = new ExtensionBrowserViewModel(this.GetExtensionLibrariesPath(), ConfigurableExtensionTypes.Trigger);
            return PartialView("ExtensionBrowser", model);
        }
    }
}