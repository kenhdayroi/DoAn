using LuxStay.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Newtonsoft.Json;
using LuxStay.Dao;

namespace LuxStay.Controllers
{
    public class LoginController : Controller
    {
        private readonly DataProvider _dataProvider;

        public LoginController(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("logout") != null || HttpContext.Session.GetString("loginFail") != null)
            {
                HttpContext.Session.Remove("logout");
                return View();
            }

            if (HttpContext.Session.GetString("register") != null)
            {
                HttpContext.Session.Remove("register");
                return View();
            }

            string email = "";
            string password = "";
            if (Request.Cookies.TryGetValue("email", out email) && Request.Cookies.TryGetValue("password", out password))
            {
            } 

            UserDao userDao = new UserDao(_dataProvider);
            User user = userDao.findByEmailAndPassword(email, password);
            if (user != null) 
            {
                int length = user.name.Split().Length;
                string lastName = user.name.Split()[length - 1];
                HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user));
                HttpContext.Session.SetString("lastName", lastName);
                if (user.role.Equals("ROLE_ADMIN"))
                {
                    return Redirect("/Admin/HomeAdmin");

                }
                else if (user.role.Equals("ROLE_USER"))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult Authentic(string remember)
        {
            UserDao userDao = new UserDao(_dataProvider);
            string email = Request.Form["email"];
            string password = Request.Form["password"];

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                HttpContext.Session.SetString("loginFail", "Email and password cannot be empty!");
                return RedirectToAction("Index", "Login");
            }

            User user = userDao.findByEmailAndPassword(email, password);

            if (user != null)
            {
                if (remember.Equals("true"))
                {
                    Response.Cookies.Append("email", email, new CookieOptions { Expires = DateTimeOffset.Now.AddDays(1) });
                    Response.Cookies.Append("password", password, new CookieOptions { Expires = DateTimeOffset.Now.AddDays(1) });
                }

                int length = user.name.Split().Length;
                string lastName = user.name.Split()[length - 1];

                HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user));
                HttpContext.Session.SetString("lastName", lastName);

                if (user.role.Equals("ROLE_ADMIN"))
                {
                    return Redirect("/Admin/HomeAdmin");

                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                HttpContext.Session.SetString("loginFail", "Invalid email or password!");
                return RedirectToAction("Index", "Login");
            }
        }


    }
}
