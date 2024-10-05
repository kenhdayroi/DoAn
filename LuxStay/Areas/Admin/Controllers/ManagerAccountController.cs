using LuxStay.Dao;
using LuxStay.Models;
using Microsoft.AspNetCore.Mvc;

namespace LuxStay.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManagerAccountController : Controller
    {
        private readonly DataProvider _dataProvider;
        private readonly IConfiguration _configuration;

        public ManagerAccountController(DataProvider dataProvider, IConfiguration configuration)
        {
            _dataProvider = dataProvider;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InsertAccount()
        {
            UserDao userDao = new UserDao(_dataProvider);
            string email = HttpContext.Request.Form["email"];
            string phone = HttpContext.Request.Form["phone"];
            string name = HttpContext.Request.Form["name"];
            string address = HttpContext.Request.Form["address"];
            string role = HttpContext.Request.Form["role"];
            string password = HttpContext.Request.Form["password"];

            User user = new User
            {
                email = email,
                phone = phone,
                name = name,
                address = address,
                role = role,
                password = password
            };

            userDao.insert(user);

            return Redirect("/Admin/Account");
        }

        [HttpPost]
        public IActionResult UpdateAccount()
        {
            UserDao userDao = new UserDao(_dataProvider);
            int userId = int.Parse(HttpContext.Request.Form["user_id"]);

            string email = HttpContext.Request.Form["email"];
            string phone = HttpContext.Request.Form["phone"];
            string name = HttpContext.Request.Form["name"];
            string address = HttpContext.Request.Form["address"];
            string role = HttpContext.Request.Form["role"];
            string password = HttpContext.Request.Form["password"];

            User user = new User
            {
                user_id = userId,
                email = email,
                phone = phone,
                name = name,
                address = address,
                role = role,
                password = password
            };

            userDao.update(user);

            return Redirect("/Admin/Account");
        }

        public IActionResult DeleteAccount()
        {
            UserDao userDao = new UserDao(_dataProvider);
            int userId = int.Parse(HttpContext.Request.Query["user_id"]);

            userDao.delete(userId);

            return Redirect("/Admin/Account");
        }
    }
}
