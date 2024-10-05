using LuxStay.Dao;
using LuxStay.Models;
using Microsoft.AspNetCore.Mvc;

namespace LuxStay.Controllers
{
    public class VerifyAccountLinkController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly DataProvider _dataProvider;

        public VerifyAccountLinkController(IConfiguration configuration, DataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            UserDao userDao = new UserDao(_dataProvider);
            User user = new User
            {
                email = HttpContext.Session.GetString("emailRegister"),
                password = HttpContext.Session.GetString("passwordRegister"),
                name = HttpContext.Session.GetString("nameRegister"),
                phone = HttpContext.Session.GetString("phoneRegister")
            };

            userDao.insert(user); 

            HttpContext.Session.SetString("register", "register");
            HttpContext.Session.SetString("registerSuccess", "Đăng ký tài khoản thành công! Đăng nhập ngay.");
            HttpContext.Session.Remove("loginFail");

            return RedirectToAction("Index", "Login");
        }
    }
}
