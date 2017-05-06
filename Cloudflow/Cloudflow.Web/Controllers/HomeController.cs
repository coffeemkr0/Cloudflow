using Cloudflow.Core.Data;
using Cloudflow.Core.Data.Server;
using Cloudflow.Web.ViewModels.Home;
using System;
using System.Collections.Generic;
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
            _serverDbContexxt = new ServerDbContext(true);
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