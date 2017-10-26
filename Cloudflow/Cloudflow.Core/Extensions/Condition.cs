using Cloudflow.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions
{
    public abstract class Condition : ConfigurableExtension
    {
        #region Properties
        public ExtensionConfiguration ConditionConfiguration { get; }

        public log4net.ILog ConditionLogger { get; }
        #endregion

        #region Constructors
        public Condition(ExtensionConfiguration conditionConfiguration) : base()
        {
            this.ConditionConfiguration = conditionConfiguration;
            this.ConditionLogger = log4net.LogManager.GetLogger($"Condition.{conditionConfiguration.Name}");
        }
        #endregion

        #region Public Methods
        public abstract bool CheckCondition();
        #endregion
    }
}
