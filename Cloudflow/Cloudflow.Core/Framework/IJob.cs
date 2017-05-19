using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Framework
{
    #region Event Handlers
    public delegate void JobTriggerFiredEventHandler(IJob job, Trigger trigger, Dictionary<string, object> triggerData);
    #endregion

    public interface IJob
    {
        #region Events
        event JobTriggerFiredEventHandler JobTriggerFired;
        #endregion

        #region Properties
        Guid Id { get; set; }

        string Name { get; set; }

        List<Step> Steps { get; set; }
        #endregion

        #region Methods
        void Start();

        void Stop();
        #endregion
    }
}
