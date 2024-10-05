using Microsoft.AspNetCore.Mvc;
using LuxStay.Models;
using System.Data;
using System.Text.Json;
using System.Collections.Generic;

namespace LuxStay.Controllers
{
    public class HistoryBookingTourController : Controller
    {
        private readonly DataProvider _dataProvider;

        public HistoryBookingTourController(DataProvider dataProvider)
        {
            _dataProvider = dataProvider; // Khởi tạo DataProvider
        }

        public IActionResult Index()
        {
            int userId = GetCurrentUserId(); // Lấy userId từ session hoặc phương thức nào đó
            var bookings = GetBookingsByUserId(userId); // Lấy danh sách booking cùng thông tin tour
            return View(bookings); // Trả về view cùng với danh sách booking
        }

        private int GetCurrentUserId()
        {
            User user = JsonSerializer.Deserialize<User>(HttpContext.Session.GetString("user"));
            return user?.user_id ?? 0; // Trả về userId, nếu không có thì trả về 0
        }

        private List<dynamic> GetBookingsByUserId(int userId)
        {
            List<dynamic> bookingsWithTours = new List<dynamic>();

            // Truy vấn để lấy thông tin booking
            string bookingQuery = $"SELECT * FROM BookingTour WHERE user_id = {userId}";
            DataTable bookingDataTable = _dataProvider.excuteQuery(bookingQuery);

            foreach (DataRow bookingRow in bookingDataTable.Rows)
            {
                var booking = new BookingTour
                {
                    booking_tour_id = (int)bookingRow["booking_tour_id"],
                    user_id = (int)bookingRow["user_id"],
                    tour_id = (int)bookingRow["tour_id"],
                    date_booking = (DateTime)bookingRow["date_booking"],
                    total_price = (decimal)bookingRow["total_price"],
                    people = (int)bookingRow["people"]
                };

                // Truy vấn để lấy thông tin tour tương ứng
                string tourQuery = $"SELECT * FROM Tour WHERE tour_id = {booking.tour_id}";
                DataTable tourDataTable = _dataProvider.excuteQuery(tourQuery);

                if (tourDataTable.Rows.Count > 0)
                {
                    var tourRow = tourDataTable.Rows[0];
                    var tourInfo = new
                    {
                        tour_id = (int)tourRow["tour_id"],
                        tour_name = (string)tourRow["tour_name"],
                        // Thêm các thuộc tính khác của tour nếu cần
                    };

                    bookingsWithTours.Add(new { booking, TourInfo = tourInfo });
                }
            }

            return bookingsWithTours; // Trả về danh sách booking cùng thông tin tour
        }
    }
}
