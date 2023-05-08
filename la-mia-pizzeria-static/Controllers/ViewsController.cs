using Microsoft.AspNetCore.Mvc;

namespace la_mia_pizzeria_static.Controllers
{
    public class ViewsController : Controller
    {
        private ICustomLogger _logger;

        public ViewsController(ICustomLogger logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.WriteLog("test 2004");
            return View();
        }
    }
}
