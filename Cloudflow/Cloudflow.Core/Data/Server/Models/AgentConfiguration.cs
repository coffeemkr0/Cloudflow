using System;

namespace Cloudflow.Core.Data.Server.Models
{
    public class AgentConfiguration
    {
        #region Properties
        public int Id { get; set; }

        public bool Enabled { get; set; }

        public string MachineName { get; set; }

        public int Port { get; set; }
        #endregion

        #region Public Methods
        public static AgentConfiguration CreateTestItem()
        {
            return new AgentConfiguration()
            {
                Enabled = true,
                MachineName = Environment.MachineName,
                Port = Communication.TcpIp.GetLocalAvailablePort()
            };
        }
        #endregion
    }
}