using Microsoft.AspNetCore.Mvc;
using LuxStay.Dao;


namespace LuxStay.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
