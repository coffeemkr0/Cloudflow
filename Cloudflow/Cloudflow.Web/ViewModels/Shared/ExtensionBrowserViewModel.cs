using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Shared
{
    public class ExtensionBrowserViewModel
    {
        #region Properties
        public List<ExtensionLibraryViewModel> ExtensionLibraries { get; set; }
        #endregion

        #region Constructors
        public ExtensionBrowserViewModel(string extensionLibraryFolder, ConfigurableExtensionTypes extensionType)
        {
            this.ExtensionLibraries = GetConfigurableExtensions(extensionLibraryFolder, extensionType);
        }
        #endregion

        #region Private Methods
        private List<ExtensionLibraryViewModel> GetConfigurableExtensions(string extensionLibraryFolder, ConfigurableExtensionTypes extensionType)
        {
            var libraries = new List<ExtensionLibraryViewModel>();

            int index = 0;
            foreach (var extensionLibraryFile in Directory.GetFiles(extensionLibraryFolder, "*.dll"))
            {
                var extensionLibraryViewModel = new ExtensionLibraryViewModel
                {
                    Name = Path.GetFileNameWithoutExtension(extensionLibraryFile)
                };

                if (index == 0) extensionLibraryViewModel.Active = true;

                var extensionBrowser = new ConfigurableExtensionBrowser(extensionLibraryFile);
                foreach (var trigger in extensionBrowser.GetConfigurableExtensions(extensionType))
                {
                    var extensionViewModel = new ExtensionViewModel
                    {
                        ExtensionId = trigger.Id,
                        ExtensionLibrary = extensionLibraryFile,
                        Name = trigger.Type.ToString()
                    };
                    extensionLibraryViewModel.Extensions.Add(extensionViewModel);
                }

                libraries.Add(extensionLibraryViewModel);
            }

            return libraries;
        }
        #endregion

        public class ExtensionLibraryViewModel
        {
            #region Properties
            public string Name { get; set; }

            public bool Active { get; set; }

            public List<ExtensionViewModel> Extensions { get; set; }
            #endregion

            #region Constructors
            public ExtensionLibraryViewModel()
            {
                this.Extensions = new List<ExtensionViewModel>();
            }
            #endregion
        }

        public class ExtensionViewModel
        {
            #region Properties
            public string ExtensionId { get; set; }

            public string ExtensionLibrary { get; set; }

            public string Name { get; set; }
            #endregion
        }
    }
}