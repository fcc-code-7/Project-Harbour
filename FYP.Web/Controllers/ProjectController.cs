using Microsoft.AspNetCore.Mvc;

namespace FYP.Web.Controllers
{
    public class ProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Projects()
        {
            return View();
        }
    }
}
