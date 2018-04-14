using System;
using System.IO;

namespace Cloudflow.Core.Configuration
{
    public static class AgentLocalConfiguration
    {
        #region Public Methods
        public static System.Configuration.Configuration GetConfiguration()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                            "Cloudflow", "Agent", "Agent.config");

            var agentConfiguration = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(
                new System.Configuration.ExeConfigurationFileMap { ExeConfigFilename = path },
                System.Configuration.ConfigurationUserLevel.None);

            return agentConfiguration;
        }
        #endregion
    }
}
