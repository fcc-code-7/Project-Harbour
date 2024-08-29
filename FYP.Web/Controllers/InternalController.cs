using Microsoft.AspNetCore.Mvc;

namespace FYP.Web.Controllers
{
    public class InternalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
