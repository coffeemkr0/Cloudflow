using System.Data.Entity;
using System.Linq;
using Cloudflow.Core.Data.Server.Models;
using Cloudflow.Core.Data.Shared;

namespace Cloudflow.Core.Data.Server
{
    public class ServerDbContext : SharedDbContext
    {
        #region Models

        public DbSet<AgentConfiguration> AgentConfigurations { get; set; }

        #endregion

        #region Public Methods

        protected override void CreateTestData(string extensionsAssemblyPath)
        {
            base.CreateTestData(extensionsAssemblyPath);

            if (AgentConfigurations.ToList().FirstOrDefault() == null)
            {
                AgentConfigurations.Add(AgentConfiguration.CreateTestItem());
                SaveChanges();
            }
        }

        #endregion

        #region Constructors

        public ServerDbContext()
        {
        }

        public ServerDbContext(bool createTestData, string extensionsAssemblyPath) : this()
        {
            if (createTestData) CreateTestData(extensionsAssemblyPath);
        }

        #endregion
    }
}