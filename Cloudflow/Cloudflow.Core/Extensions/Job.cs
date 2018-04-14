namespace Cloudflow.Core.Extensions
{
    public abstract class Job : ConfigurableExtension
    {
        #region Properties
        public ExtensionConfiguration JobConfiguration { get; }

        public log4net.ILog JobLogger { get; }
        #endregion

        #region Constructors
        public Job(ExtensionConfiguration jobConfiguration) : base()
        {
            JobConfiguration = jobConfiguration;
            JobLogger = log4net.LogManager.GetLogger($"Job.{jobConfiguration.Name}");
        }
        #endregion

        #region Public Methods
        public abstract void Start();

        public abstract void Stop();
        #endregion
    }
}
