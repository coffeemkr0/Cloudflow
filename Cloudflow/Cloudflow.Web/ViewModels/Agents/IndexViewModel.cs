using Cloudflow.Core.Data.Server.Models;
using System.Collections.Generic;

namespace Cloudflow.Web.ViewModels.Agents
{
    public class IndexViewModel
    {
        #region Properties
        public List<AgentConfiguration> AgentConfigurations { get; set; }
        #endregion

        #region Constructors
        public IndexViewModel()
        {
            AgentConfigurations = new List<AgentConfiguration>();
        }
        #endregion
    }
}