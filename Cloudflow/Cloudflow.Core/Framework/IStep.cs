using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Framework
{
    #region Event Handlers
    public delegate void StepOutputEventHandler(IStep step, OutputEventLevels level, string message);
    #endregion

    public interface IStep
    {
        #region Events
        event StepOutputEventHandler StepOutput;
        #endregion

        #region Properties
        Guid Id { get; }

        string Name { get; }
        #endregion

        #region Methods
        void Execute();
        #endregion
    }
}
