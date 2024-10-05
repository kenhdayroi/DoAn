using LuxStay.Dao;
using LuxStay.Models;
using Microsoft.AspNetCore.Mvc;

namespace LuxStay.Controllers
{
    public class ForgetPasswordController : Controller
    {
        private readonly DataProvider _dataProvider;
        private readonly IConfiguration _configuration;

        public ForgetPasswordController(DataProvider dataProvider, IConfiguration configuration)
        {
            _dataProvider = dataProvider;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AcceptEmail()
        {
            string email = Request.Form["email"];
            SendMailDao sendMailDao = new SendMailDao(_configuration);
            string password = sendMailDao.RandomCode(4);
            string subject = "Quên Mật Khẩu!";
            string content = "Mật khẩu mới của bạn để đăng nhập trên LuxStay là: " + password;
            sendMailDao.SendMail(email, subject, content);
            UserDao userDao = new UserDao(_dataProvider);
            userDao.updatePasswordByEmail(email, password);
            ViewData["email"] = email;
            return View();
        }
    }
}
