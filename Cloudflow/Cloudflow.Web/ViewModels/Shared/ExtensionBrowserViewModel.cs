using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.Controllers;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Shared
{
    public class ExtensionBrowserViewModel
    {
        #region Properties
        public List<ExtensionLibraryViewModel> ExtensionLibraries { get; }

        public ConfigurableExtensionTypes ExtensionType { get; }

        public string ExtensionTypeDisplayText
        {
            get
            {
                switch (this.ExtensionType)
                {
                    case ConfigurableExtensionTypes.Job:
                        return "Jobs";
                    case ConfigurableExtensionTypes.Trigger:
                        return "Triggers";
                    case ConfigurableExtensionTypes.Step:
                        return "Steps";
                    default:
                        throw new ArgumentException($"The extension type {this.ExtensionType} is not supported.", "ExtensionType");
                }
            }
        }
        #endregion

        #region Constructors
        public ExtensionBrowserViewModel(string extensionLibraryFolder, ConfigurableExtensionTypes extensionType)
        {
            this.ExtensionType = extensionType;
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
                    Name = FileVersionInfo.GetVersionInfo(extensionLibraryFile).ProductName
                };

                if (index == 0) extensionLibraryViewModel.Active = true;

                var extensionBrowser = new ConfigurableExtensionBrowser(extensionLibraryFile);
                foreach (var extension in extensionBrowser.GetConfigurableExtensions(extensionType))
                {
                    var displayNameAttribute = (DisplayNameAttribute)extension.GetType().GetCustomAttributes(typeof(DisplayNameAttribute), true).SingleOrDefault();
                    var descriptionAttribute = (DescriptionAttribute)extension.GetType().GetCustomAttributes(typeof(DescriptionAttribute), true).SingleOrDefault();

                    var extensionViewModel = new ExtensionViewModel
                    {
                        ExtensionId = extension.ExtensionId,
                        ExtensionLibrary = extensionLibraryFile,
                        Name = displayNameAttribute != null ? displayNameAttribute.DisplayName : extension.ExtensionType.ToString(),
                        Description = descriptionAttribute != null ? descriptionAttribute.Description : ""
                    };
                    extensionLibraryViewModel.Extensions.Add(extensionViewModel);
                }

                libraries.Add(extensionLibraryViewModel);

#if DEBUG
                //Add it twice just for testing having multiple extensions
                libraries.Add(extensionLibraryViewModel);
#endif
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

            public string Description { get; set; }
            #endregion
        }
    }
}