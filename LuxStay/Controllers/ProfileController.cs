using LuxStay.Dao;
using LuxStay.Models;
using Microsoft.AspNetCore.Mvc;

namespace LuxStay.Controllers
{
    public class ProfileController : Controller
    {
        private readonly DataProvider _dataProvider;

        public ProfileController(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }
        public IActionResult Index()
        {
            User user = HttpContext.Session.GetObjectFromJson<User>("user");
            return View(user);
        }

        public IActionResult Update()
        {
            UserDao userDao = new UserDao(_dataProvider);
            int user_id = int.Parse(HttpContext.Request.Form["user_id"]);
            string email = HttpContext.Request.Form["email"];
            string phone = HttpContext.Request.Form["phone"];
            string name = HttpContext.Request.Form["name"];
            string address = HttpContext.Request.Form["address"];
            string role = "ROLE_USER";
            string password = HttpContext.Request.Form["password"];

            User user = new User
            {
                user_id = user_id,
                email = email,
                phone = phone,
                name = name,
                address = address,
                role = role,
                password = password
            };

            userDao.update(user);

            ViewData["update_profile_success"] = "Cập nhật thông tin tài khoản thành công!";
            HttpContext.Session.SetObjectAsJson("user", user); 
            return View(user);
        }
    }
}
