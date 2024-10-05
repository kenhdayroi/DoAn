using LuxStay.Dao;
using LuxStay.Models;
using Microsoft.AspNetCore.Mvc;

namespace LuxStay.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly DataProvider _dataProvider;

        public AccountController(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public IActionResult Index()
        {
            UserDao userDao = new UserDao(_dataProvider);
            ViewBag.users = userDao.findAll();
            return View();
        }

        public IActionResult InsertAccount()
        {
            return View();
        }

        public IActionResult UpdateAccount()
        {
            int userId = int.Parse(HttpContext.Request.Query["user_id"]);
            UserDao userDao = new UserDao(_dataProvider);
            User user = userDao.findById(userId);
            return View(user);
        }
    }
}
