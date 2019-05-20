using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UpdateResxFile.Models;

namespace UpdateResxFile.Controllers
{
    public class HomeController : Controller
    {
        private Managers.ResourceManager _resourceManager;

        public HomeController()
        {
            _resourceManager = new Managers.ResourceManager();
        }

        public IActionResult Index()
        {
            List<ResourceModel> sourceResourceModels = _resourceManager.GetResources();
            return View(sourceResourceModels);
        }

        [HttpPost]
        public IActionResult Update(List<ResourceModel> resourceModels)
        {
            //GenericHelper _genericHelper = new GenericHelper();
            _resourceManager.UpdateDestinationResources(resourceModels);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
