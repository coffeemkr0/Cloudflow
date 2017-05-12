using Cloudflow.Core.Data;
using Cloudflow.Core.Data.Server;
using Cloudflow.Web.ViewModels.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            _serverDbContext = new ServerDbContext(true);
#else
            _databaseContext = new ServerDbContext();
#endif

            IndexViewModel model = new IndexViewModel();

            model.AgentConfigurations.AddRange(_serverDbContext.AgentConfigurations.ToList());

            return View(model);
        }
        #endregion
    }
}