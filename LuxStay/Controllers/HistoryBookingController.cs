using LuxStay.Dao;
using LuxStay.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LuxStay.Controllers
{
    public class HistoryBookingController : Controller
    {
        private readonly DataProvider _dataProvider;

        public HistoryBookingController(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public IActionResult Index()
        {
            User user = HttpContext.Session.GetObjectFromJson<User>("user");
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            int user_id = user.user_id;
            BookingDao bookingDao = new BookingDao(_dataProvider);
            List<Booking> bookings = bookingDao.findAllBookingByUserId(user_id);
            ViewBag.bookings = bookings;
            return View();
        }
    }
}
