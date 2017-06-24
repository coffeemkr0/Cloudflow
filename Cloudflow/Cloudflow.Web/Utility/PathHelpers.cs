using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cloudflow.Web.Utility
{
    public static class PathHelpers
    {
        #region Public Methods

        public static string GetExtensionLibrariesPath(this Controller controller)
        {
            return controller.Server.MapPath(@"~\ExtensionLibraries");
        }

        public static List<string> GetExtensionLibraries(this Controller controller)
        {
            return Directory.GetFiles(GetExtensionLibrariesPath(controller), "*.dll").ToList();
        }
        #endregion
    }
}