using Microsoft.AspNetCore.Mvc;

namespace FYP.Web.Controllers
{
    public class CordinatorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
