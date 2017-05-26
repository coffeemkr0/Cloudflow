using Cloudflow.Core.Data;
using Cloudflow.Core.Data.Server;
using Cloudflow.Web.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cloudflow.Web.Controllers
{
    public class HomeController : Controller
    {
        #region Module Level Declarations
        ServerDbContext _serverDbContexxt;
        #endregion

        public ActionResult Index()
        {
#if DEBUG
            var serverPath = Server.MapPath("~").TrimEnd(Path.DirectorySeparatorChar);
            var extensionsAssemblyPath = Directory.GetParent(serverPath).FullName;
            extensionsAssemblyPath = Path.Combine(extensionsAssemblyPath, @"Cloudflow.Extensions\bin\debug\Cloudflow.Extensions.dll");
            _serverDbContexxt = new ServerDbContext(true, extensionsAssemblyPath);
#else
            _databaseContext = new ServerDbContext();
#endif
            IndexViewModel model = new IndexViewModel();

            model.AgentConfigurations.AddRange(_serverDbContexxt.AgentConfigurations.ToList());

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (_serverDbContexxt != null)
            {
                _serverDbContexxt.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}