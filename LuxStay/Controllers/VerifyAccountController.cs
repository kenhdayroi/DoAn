using LuxStay.Dao;
using LuxStay.Models;
using Microsoft.AspNetCore.Mvc;

namespace Luxstay.Controllers
{
    public class VerifyAccountController : Controller
    {
        private readonly UserDao _userDao;
        private readonly SendMailDao _sendMailDao;

        public VerifyAccountController(UserDao userDao, SendMailDao sendMailDao)
        {
            _userDao = userDao;
            _sendMailDao = sendMailDao;
        }

        // POST: VerifyAccount
        [HttpPost]
        public IActionResult Index(string code)
        {
            User user = new User
            {
                email = HttpContext.Session.GetString("emailRegister"),
                password = HttpContext.Session.GetString("passwordRegister"),
                name = HttpContext.Session.GetString("nameRegister"),
                phone = HttpContext.Session.GetString("phoneRegister")
            };

            string code_verify = HttpContext.Session.GetString("code_verify");

            if (code.Equals(code_verify))
            {
                // Condition to create account is valid
                _userDao.insert(user); // Insert account to database
                // Set registration status
                HttpContext.Session.SetString("register", "register");
                // Display notification "Register successfully!" for client
                HttpContext.Session.SetString("registerSuccess", "Đăng ký tài khoản thành công! Đăng nhập ngay.");
                HttpContext.Session.Remove("loginFail");
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.verify_fail = "Mã xác nhận không chính xác!";
                return View();
            }
        }

        public IActionResult ResendMail()
        {
            // Send mail again to verify
            string code_verify = _sendMailDao.RandomCode(4);
            HttpContext.Session.SetString("code_verify", code_verify);
            string subject = "Xác thực địa chỉ email!";
            string content = "Cảm ơn bạn đã đăng ký sử dụng dịch vụ của Luxstay! Mã xác thực của bạn là: " + code_verify;
            string email = HttpContext.Session.GetString("emailRegister");
            _sendMailDao.SendMail(email, subject, content);
            ViewBag.resend_mail = "Mã xác nhận đã được gửi lại!";
            return View();
        }
    }
}
