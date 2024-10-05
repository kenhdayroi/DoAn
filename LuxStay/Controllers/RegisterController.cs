using LuxStay.Dao;
using LuxStay.Models;
using Microsoft.AspNetCore.Mvc;

public class RegisterController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly DataProvider _dataProvider;

    public RegisterController(IConfiguration configuration, DataProvider dataProvider)
    {
        _dataProvider = dataProvider;
        _configuration = configuration;
    }

    // GET: Register
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Verify()
    {
        UserDao userDao = new UserDao(_dataProvider);
        User user = new User
        {
            email = HttpContext.Request.Form["email"],
            phone = HttpContext.Request.Form["phone"],
            name = HttpContext.Request.Form["name"],
            password = HttpContext.Request.Form["password"]
        };

        string repassword = HttpContext.Request.Form["repassword"];

        // Lưu thông tin vào session
        HttpContext.Session.SetString("emailRegister", user.email);
        HttpContext.Session.SetString("passwordRegister", user.password);
        HttpContext.Session.SetString("nameRegister", user.name);
        HttpContext.Session.SetString("phoneRegister", user.phone);

        // Kiểm tra email đã tồn tại chưa
        if (userDao.findByEmail(user.email) != null)
        {
            HttpContext.Session.SetString("emailExist", "Địa chỉ email đã tồn tại!");
            HttpContext.Session.Remove("passwordNotMatch");
            return RedirectToAction("Index");
        }

        // Kiểm tra mật khẩu
        if (user.password.Equals(repassword))
        {
            userDao.insert(user);
            // Gửi mail xác thực
            SendMailDao sendMailDao = new SendMailDao(_configuration);
            sendMailDao.SendVerificationLinkEmail(user.email);
            string code_verify = sendMailDao.RandomCode(4);
            HttpContext.Session.SetString("code_verify", code_verify);

            // Gửi mail chứa mã xác thực
            string subject = "Xác thực địa chỉ email!";
            string content = "Cảm ơn bạn đã đăng ký sử dụng dịch vụ của Luxstay! Mã xác thực của bạn là: " + code_verify;
            string link = "Cảm ơn bạn đã đăng ký sử dụng dịch vụ của Luxstay! Mã xác thực của bạn là: " + code_verify;
            sendMailDao.SendMail(user.email, subject, link);

            return RedirectToAction("Index", "Login");
        }
        else
        {
            HttpContext.Session.SetString("passwordNotMatch", "Mật khẩu không trùng khớp!");
            HttpContext.Session.Remove("emailExist");
            return RedirectToAction("Index");
        }
    }
}

