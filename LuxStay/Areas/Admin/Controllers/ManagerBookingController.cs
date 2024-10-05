using LuxStay.Dao;
using LuxStay.Models;
using Microsoft.AspNetCore.Mvc;

namespace LuxStay.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManagerBookingController : Controller
    {
        private readonly DataProvider _dataProvider;
        private readonly IConfiguration _configuration;

        public ManagerBookingController(DataProvider dataProvider, IConfiguration configuration)
        {
            _dataProvider = dataProvider;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            BookingDao bookingDao = new BookingDao(_dataProvider);
            List<Booking> bookings = bookingDao.findAllBooking();
            ViewBag.bookings = bookings;
            return View();
        }

        public IActionResult Cancel()
        {
            int bookingId = int.Parse(HttpContext.Request.Query["booking_id"]);
            BookingDao bookingDao = new BookingDao(_dataProvider);
            bookingDao.deleteById(bookingId);
            return Redirect("/Admin/ManagerBooking");
        }
    }
}
