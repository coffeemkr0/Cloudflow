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
        Models.CloudflowWebDb _db = new Models.CloudflowWebDb();

        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();
            model.AgentConfigurations.AddRange(_db.AgentConfigurations.ToList());

            //model.AgentConfigurations = new List<Core.Web.AgentConfiguration>();
            //model.AgentConfigurations.Add(new Core.Web.AgentConfiguration()
            //{
            //    Id = 1,
            //    MachineName = "mcoffey-vm1",
            //    Enabled = true
            //});

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
            if (_db != null)
            {
                _db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}