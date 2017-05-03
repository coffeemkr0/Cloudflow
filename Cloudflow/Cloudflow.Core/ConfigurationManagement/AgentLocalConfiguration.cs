using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.ConfigurationManagement
{
    public static class AgentLocalConfiguration
    {
        #region Public Methods
        public static Configuration GetConfiguration()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                            "Cloudflow", "Agent", "Agent.config");

            Configuration agentConfiguration = ConfigurationManager.OpenMappedExeConfiguration(
                new ExeConfigurationFileMap { ExeConfigFilename = path }, ConfigurationUserLevel.None);

            return agentConfiguration;
        }
        #endregion
    }
}
