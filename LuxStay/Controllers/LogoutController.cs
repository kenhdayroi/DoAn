using Microsoft.AspNetCore.Mvc;

namespace LuxStay.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.SetString("logout", "logout");
            return RedirectToAction("Index", "Login");
        }
    }
}
