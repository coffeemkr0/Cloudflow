using Cloudflow.Core.Configuration;
using Cloudflow.Core.Extensions;
using Cloudflow.Core.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Extensions.Jobs
{
    [ExportExtension("62A56D5B-07E5-41A3-A637-5E7C53FCF399", typeof(DefaultJobConfiguration))]
    public class DefaultJobConfiguration : ExtensionConfiguration
    {

    }
}
