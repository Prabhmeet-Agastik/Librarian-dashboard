using Microsoft.AspNetCore.Mvc;

namespace LibraryDashboard2.Areas.Student.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
    }
}
