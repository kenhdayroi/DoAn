using LuxStay.Dao;
using LuxStay.Models;
using Microsoft.AspNetCore.Mvc;

namespace LuxStay.Controllers
{
    public class CancelBookingController : Controller
    {
        private readonly DataProvider _dataProvider;

        public CancelBookingController(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }
        public IActionResult Index()
        {
            int booking_id = int.Parse(Request.Query["booking_id"]);
            BookingDao bookingDao = new BookingDao(_dataProvider);
            bookingDao.deleteById(booking_id);
            return RedirectToAction("Index", "HistoryBooking");
        }
    }
}
