using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Configuration
{
    public static class AgentLocalConfiguration
    {
        #region Public Methods
        public static System.Configuration.Configuration GetConfiguration()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                            "Cloudflow", "Agent", "Agent.config");

            System.Configuration.Configuration agentConfiguration = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(
                new System.Configuration.ExeConfigurationFileMap { ExeConfigFilename = path },
                System.Configuration.ConfigurationUserLevel.None);

            return agentConfiguration;
        }
        #endregion
    }
}
