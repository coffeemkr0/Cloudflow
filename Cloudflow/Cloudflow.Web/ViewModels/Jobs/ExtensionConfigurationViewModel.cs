using Cloudflow.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class ExtensionConfigurationViewModel
    {
        public Guid ExtensionId { get; set; }

        public string ExtensionAssemblyPath { get; set; }

        public ExtensionConfiguration Configuration { get; set; }
    }
}