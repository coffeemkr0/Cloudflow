using Cloudflow.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class ExtensionBrowserViewModel
    {
        #region Properties
        public string Id { get; set; }

        public string Caption { get; set; }

        public List<ExtensionLibrary> ExtensionLibraries { get; set; }
        #endregion

        #region Constructors
        public ExtensionBrowserViewModel()
        {
            this.ExtensionLibraries = new List<ExtensionLibrary>();
        }
        #endregion

        #region Public Methods
        public static ExtensionBrowserViewModel GetModel(string id, string extensionLibraryFolder, ConfigurableExtensionTypes extensionType)
        {
            var model = new ExtensionBrowserViewModel
            {
                Id = id,
            };

            switch (extensionType)
            {
                case ConfigurableExtensionTypes.Trigger:
                    model.Caption = "Triggers";
                    break;
                case ConfigurableExtensionTypes.Step:
                    model.Caption = "Steps";
                    break;
                case ConfigurableExtensionTypes.Condition:
                    model.Caption = "Conditions";
                    break;
            }

            foreach (var extensionLibraryFile in Directory.GetFiles(extensionLibraryFolder, "*.dll"))
            {
                var extensionLibrary = new ExtensionLibrary
                {
                    Caption = FileVersionInfo.GetVersionInfo(extensionLibraryFile).ProductName
                };

                model.ExtensionLibraries.Add(extensionLibrary);

                var extensionBrowser = new ConfigurableExtensionBrowser(extensionLibraryFile);
                foreach (var configurableExtension in extensionBrowser.GetConfigurableExtensions(extensionType))
                {
                    var extension = new ExtensionLibrary.Extension
                    {
                        ExtensionId = Guid.Parse(configurableExtension.ExtensionId),
                        ExtensionAssemblyPath = extensionLibraryFile,
                        Name = configurableExtension.ExtensionName,
                        Description = configurableExtension.ExtensionDescription,
                        Icon = configurableExtension.Icon,
                    };

                    extensionLibrary.Extensions.Add(extension);
                }
            }

            return model;
        }
        #endregion

        #region ExtensionLibrary
        public class ExtensionLibrary
        {
            #region Properties
            public string Caption { get; set; }

            public List<Extension> Extensions { get; set; }
            #endregion

            #region Constructors
            public ExtensionLibrary()
            {
                this.Extensions = new List<Extension>();
            }
            #endregion

            #region Extension
            public class Extension
            {
                #region Properties
                public Guid ExtensionId { get; set; }

                public string ExtensionAssemblyPath { get; set; }

                public string Name { get; set; }

                public string Description { get; set; }

                public byte[] Icon { get; set; }
                #endregion
            }
            #endregion
        }
        #endregion
    }
}