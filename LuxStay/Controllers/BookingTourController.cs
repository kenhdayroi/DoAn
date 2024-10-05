using Microsoft.AspNetCore.Mvc;
using LuxStay.Models;

namespace LuxStay.Controllers
{
    public class BookingTourController : Controller
    {
        private readonly BookingTourDao _bookingTourDao;

       /* public BookingTourController(BookingTourDao bookingTourDao)
        {
            _bookingTourDao = bookingTourDao;
        }*/

       /* [HttpPost]
        public IActionResult InsertBookingTour(int tour_id, int people, decimal total_price)
        {
            // Lấy user_id từ session hoặc một cách khác
            int userId = GetCurrentUserId(); // Phương thức lấy user_id của người dùng hiện tại
            BookingTour bookingTour = new BookingTour
            {
                user_id = userId,
                tour_id = tour_id,
                date_booking = DateTime.Now,
                total_price = total_price,
                people = people
            };

            // Gọi phương thức để lưu thông tin đặt tour vào cơ sở dữ liệu
            _bookingTourDao.InsertBookingTour(bookingTour);

            // Chuyển hướng đến trang lịch sử đặt tour sau khi đặt thành công
            return RedirectToAction("History", "BookingTour");
        }*/

      /*  public IActionResult History()
        {
            int userId = GetCurrentUserId(); 
            var bookings = _bookingTourDao.GetBookingsByUserId(userId);
            return View(bookings); 
        }
*/
        private int GetCurrentUserId()
        {
            return Convert.ToInt32(HttpContext.Session.GetString("UserId"));
        }
    }
}
