using Cloudflow.Core.Data;
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
        CoreDbContext _databaseContext;
        #endregion

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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (_databaseContext != null)
            {
                _databaseContext.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}