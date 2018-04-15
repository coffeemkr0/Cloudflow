using System.Configuration;
using Cloudflow.Core.Agents;

namespace Cloudflow.Core.Configuration
{
    public class AgentLocalConfigurationSettings : IAgentConfigurationSettings
    {
        private readonly System.Configuration.Configuration _agentConfiguration;

        public AgentLocalConfigurationSettings(string configFilePath)
        {
            _agentConfiguration = ConfigurationManager.OpenMappedExeConfiguration(
                new ExeConfigurationFileMap {ExeConfigFilename = configFilePath},
                ConfigurationUserLevel.None);

            Port = int.TryParse(_agentConfiguration.AppSettings.Settings["Port"].Value, out var port) ? port : 0;
        }

        public int Port { get; set; }

        public void Save()
        {
            var portSetting = _agentConfiguration.AppSettings.Settings["Port"];
            if (portSetting == null)
                _agentConfiguration.AppSettings.Settings.Add("Port", Port.ToString());
            else
                portSetting.Value = Port.ToString();

            _agentConfiguration.Save();
        }
    }
}