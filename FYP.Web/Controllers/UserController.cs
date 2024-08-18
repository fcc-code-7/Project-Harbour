using Microsoft.AspNetCore.Mvc;

namespace FYP.Web.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }
        public IActionResult Message()
        {
            return View();
        }
    }
}
