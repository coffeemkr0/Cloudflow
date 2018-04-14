using Cloudflow.Core.Data.Server;
using Cloudflow.Web.ViewModels.Runs;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Cloudflow.Web.Controllers
{
    public class RunsController : Controller
    {
        #region Module Level Declarations
        ServerDbContext _serverDbContext;
        #endregion

        #region Actions
        // GET: Runs
        public ActionResult Index()
        {
#if DEBUG
            var serverPath = Server.MapPath("~").TrimEnd(Path.DirectorySeparatorChar);
            var extensionsAssemblyPath = Directory.GetParent(serverPath).FullName;
            extensionsAssemblyPath = Path.Combine(extensionsAssemblyPath, @"Cloudflow.Extensions\bin\debug\Cloudflow.Extensions.dll");
            _serverDbContext = new ServerDbContext(true, extensionsAssemblyPath);
#else
            _databaseContext = new ServerDbContext();
#endif

            var model = new IndexViewModel();

            model.AgentConfigurations.AddRange(_serverDbContext.AgentConfigurations.ToList());

            return View(model);
        }
        #endregion
    }
}