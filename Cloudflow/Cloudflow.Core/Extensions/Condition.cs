using log4net;

namespace Cloudflow.Core.Extensions
{
    public abstract class Condition : ConfigurableExtension
    {
        #region Constructors

        public Condition(ExtensionConfiguration conditionConfiguration)
        {
            ConditionConfiguration = conditionConfiguration;
            ConditionLogger = LogManager.GetLogger($"Condition.{conditionConfiguration.Name}");
        }

        #endregion

        #region Public Methods

        public abstract bool CheckCondition();

        #endregion

        #region Properties

        public ExtensionConfiguration ConditionConfiguration { get; }

        public ILog ConditionLogger { get; }

        #endregion
    }
}