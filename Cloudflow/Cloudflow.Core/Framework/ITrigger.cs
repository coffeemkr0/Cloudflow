using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Framework
{
    #region Event Handlers
    public delegate void TriggerFiredEventHandler(ITrigger trigger, Dictionary<string, object> triggerData);
    #endregion
    public interface ITrigger
    {
        #region Events
        event TriggerFiredEventHandler Fired;
        #endregion

        #region Properties
        Guid Id { get; }

        string Name { get; }
        #endregion

        #region Methods
        void Start();

        void Stop();
        #endregion
    }
}
