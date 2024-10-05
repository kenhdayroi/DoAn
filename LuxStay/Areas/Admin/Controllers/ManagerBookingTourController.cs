using Microsoft.AspNetCore.Mvc;
using LuxStay.Models;
using System.Collections.Generic;
using System.Data;

namespace LuxStay.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManagerBookingTourController : Controller
    {
        private readonly DataProvider _dataProvider;

        public ManagerBookingTourController(IConfiguration configuration)
        {
            _dataProvider = new DataProvider(configuration);
        }

        public IActionResult Index()
        {
            List<BookingTour> bookings = GetAllBookings();
            return View(bookings);
        }

        private List<BookingTour> GetAllBookings()
        {
            List<BookingTour> bookings = new List<BookingTour>();
            string query = "SELECT booking_tour_id, user_id, tour_id, date_booking, total_price, people FROM BookingTour";
            DataTable dt = _dataProvider.excuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                BookingTour booking = new BookingTour
                {
                    booking_tour_id = Convert.ToInt32(row["booking_tour_id"]),
                    user_id = Convert.ToInt32(row["user_id"]),
                    tour_id = Convert.ToInt32(row["tour_id"]),
                    date_booking = Convert.ToDateTime(row["date_booking"]),
                    total_price = Convert.ToDecimal(row["total_price"]),
                    people = Convert.ToInt32(row["people"])
                };
                bookings.Add(booking);
            }
            return bookings;
        }
    }
}
