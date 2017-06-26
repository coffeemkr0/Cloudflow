using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Extensions.Jobs
{
    [ExportConfigurableExtension("3F6F5796-E313-4C53-8064-747C1989DA99", typeof(DefaultJob), 
        "Default Job", "62A56D5B-07E5-41A3-A637-5E7C53FCF399", "The default Cloudflow job.")]
    public class DefaultJob : Job
    {
        #region Constructors
        [ImportingConstructor]
        public DefaultJob([Import("ExtensionConfiguration")]ExtensionConfiguration jobConfiguration) : base(jobConfiguration)
        {

        }

        public override void Start()
        {
            
        }

        public override void Stop()
        {
            
        }
        #endregion
    }
}
