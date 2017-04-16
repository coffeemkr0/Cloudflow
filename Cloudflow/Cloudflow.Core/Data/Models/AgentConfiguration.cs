using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cloudflow.Core.Data.Models
{
    public class AgentConfiguration
    {
        #region Properties
        public int Id { get; set; }

        public bool Enabled { get; set; }

        public string MachineName { get; set; }
        #endregion

        #region Public Methods
        public static AgentConfiguration CreateTestItem()
        {
            return new AgentConfiguration()
            {
                Enabled = true,
                MachineName = Environment.MachineName
            };
        }
        #endregion
    }
}