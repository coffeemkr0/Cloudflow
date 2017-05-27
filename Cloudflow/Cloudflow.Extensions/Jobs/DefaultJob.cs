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
    [ExportMetadata("JobExtensionId", "3F6F5796-E313-4C53-8064-747C1989DA99")]
    public class DefaultJob : Job
    {
        #region Constructors
        [ImportingConstructor]
        public DefaultJob([Import("JobConfiguration")]JobConfiguration jobConfiguration) : base(jobConfiguration)
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
