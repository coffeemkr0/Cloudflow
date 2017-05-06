using Cloudflow.Core.Data;
using Cloudflow.Web.ViewModels.Agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cloudflow.Web.Controllers
{
    public class AgentsController : Controller
    {
        #region Module Level Declarations
        CoreDbContext _databaseContext;
        #endregion

        #region Actions
        // GET: Agents
        public ActionResult Index()
        {
#if DEBUG
            _databaseContext = new CoreDbContext(true);
#else
            _databaseContext = new CoreDbContext();
#endif

            IndexViewModel model = new IndexViewModel();

            model.AgentConfigurations.AddRange(_databaseContext.AgentConfigurations.ToList());

            return View(model);
        }
        #endregion
    }
}