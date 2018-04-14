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
            ConditionConfiguration = conditionConfiguration;
            ConditionLogger = log4net.LogManager.GetLogger($"Condition.{conditionConfiguration.Name}");
        }
        #endregion

        #region Public Methods
        public abstract bool CheckCondition();
        #endregion
    }
}
