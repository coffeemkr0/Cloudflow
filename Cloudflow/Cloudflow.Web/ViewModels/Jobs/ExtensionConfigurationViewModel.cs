using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    [DisplayTextPropertyName("Configuration.Name")]
    public class ExtensionConfigurationViewModel
    {
        [Hidden]
        public Guid Id { get; set; }

        [Hidden]
        public bool Deleted { get; set; }

        [Hidden]
        public bool Active { get; set; }

        [Hidden]
        public int Index { get; set; }

        [Hidden]
        public int Position { get; set; }

        [Hidden]
        public Guid ExtensionId { get; set; }

        [Hidden]
        public string ExtensionAssemblyPath { get; set; }

        [Hidden]
        public Guid ConfigurationExtensionId { get; set; }

        [Hidden]
        public string ConfigurationExtensionAssemblyPath { get; set; }

        [PropertyGroupAttribute("GeneralTabText")]
        [DisplayOrder(0)]
        public ExtensionConfiguration Configuration { get; set; }
    }
}