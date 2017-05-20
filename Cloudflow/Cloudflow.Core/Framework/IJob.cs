using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Framework
{
    #region Event Handlers
    public delegate void JobTriggerFiredEventHandler(IJob job, ITrigger trigger, Dictionary<string, object> triggerData);
    #endregion

    public interface IJob
    {
        #region Events
        event JobTriggerFiredEventHandler JobTriggerFired;
        #endregion

        #region Properties
        Guid Id { get; }

        string Name { get; }

        List<IStep> Steps { get; set; }
        #endregion

        #region Methods
        void Start();

        void Stop();
        #endregion
    }
}
