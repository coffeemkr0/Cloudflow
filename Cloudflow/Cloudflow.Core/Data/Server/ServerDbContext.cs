using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Cloudflow.Core.Data.Server.Models;
using Cloudflow.Core.Data.Shared.Models;
using Cloudflow.Core.Data.Shared;

namespace Cloudflow.Core.Data.Server
{
    public class ServerDbContext : SharedDbContext
    {
        #region Models
        public DbSet<AgentConfiguration> AgentConfigurations { get; set; }
        #endregion

        #region Constructors
        public ServerDbContext()
        {

        }

        public ServerDbContext(bool createTestData, string extensionsAssemblyPath) : this()
        {
            if (createTestData)
            {
                CreateTestData(extensionsAssemblyPath);
            }
        }
        #endregion

        #region Public Methods
        protected override void CreateTestData(string extensionsAssemblyPath)
        {
            base.CreateTestData(extensionsAssemblyPath);

            if (this.AgentConfigurations.ToList().FirstOrDefault() == null)
            {
                this.AgentConfigurations.Add(AgentConfiguration.CreateTestItem());
                this.SaveChanges();
            }
        }
        #endregion
    }
}
