using Cloudflow.Core.Configuration;
using Cloudflow.Core.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Extensions.Jobs
{
    [Export(typeof(Job))]
    [ExportMetadata("JobExtensionId", "62A56D5B-07E5-41A3-A637-5E7C53FCF399")]
    public class DefaultJob : Job
    {
        #region Constructors
        [ImportingConstructor]
        public DefaultJob([Import("JobConfiguration")]JobConfiguration jobConfiguration) : base(jobConfiguration)
        {

        }
        #endregion
    }
}
