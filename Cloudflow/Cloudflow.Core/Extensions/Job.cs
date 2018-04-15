using log4net;

namespace Cloudflow.Core.Extensions
{
    public abstract class Job : ConfigurableExtension
    {
        #region Constructors

        public Job(ExtensionConfiguration jobConfiguration)
        {
            JobConfiguration = jobConfiguration;
            JobLogger = LogManager.GetLogger($"Job.{jobConfiguration.Name}");
        }

        #endregion

        #region Properties

        public ExtensionConfiguration JobConfiguration { get; }

        public ILog JobLogger { get; }

        #endregion

        #region Public Methods

        public abstract void Start();

        public abstract void Stop();

        #endregion
    }
}