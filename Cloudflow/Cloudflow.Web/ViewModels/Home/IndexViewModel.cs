using Cloudflow.Core.Data.Models;
using Cloudflow.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Home
{
    public class IndexViewModel
    {
        #region Properties
        public List<AgentConfiguration> AgentConfigurations { get; set; }
        #endregion

        #region Constructors
        public IndexViewModel()
        {
            this.AgentConfigurations = new List<AgentConfiguration>();
        }
        #endregion
    }
}