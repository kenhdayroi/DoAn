using Microsoft.AspNetCore.Mvc;
using LuxStay.Dao;
using LuxStay.Models;
using System.Text.Json;

namespace LuxStay.Controllers
{
    public class DetailTourController : Controller
    {
        private readonly TourDao _tourDao;
        private readonly BookingTourDao _bookingTourDao;
        private readonly SendMailDao _sendMailDao; 

        public DetailTourController(IConfiguration configuration, BookingTourDao bookingTourDao)
        {
            DataProvider dataProvider = new DataProvider(configuration);
            _tourDao = new TourDao(dataProvider);
            _bookingTourDao = bookingTourDao;
            _sendMailDao = new SendMailDao(configuration); 
        }

        public IActionResult Index(int id)
        {
            var tour = _tourDao.GetTourById(id);
            if (tour == null)
            {
                return NotFound();
            }
            return View(tour);
        }

        [HttpPost]
        public IActionResult InsertBookingTour(int tour_id, int people, decimal price, decimal total_price)
        {
            int userId = GetCurrentUserId();
            BookingDTO bookingTour = new BookingDTO
            {
                user_id = userId,
                tour_id = tour_id,
                date_booking = DateTime.Now,
                price = price,
                total_price = total_price,
                people = people
            };

            _bookingTourDao.InsertBookingTour(bookingTour);

            User user = JsonSerializer.Deserialize<User>(HttpContext.Session.GetString("user"));
            string emailToSend = user.email;
            string subject = "Đặt tour thành công!";
            string content = $"<h3>Cảm ơn bạn đã đặt tour!</h3><p>Thông tin tour:</p><p>ID Tour: {tour_id}</p><p>Số người: {people}</p><p>Tổng giá: {total_price:C}</p>";

            _sendMailDao.SendMail(emailToSend, subject, content);

            return RedirectToAction("Index", "HistoryBookingTour");
        }

        private int GetCurrentUserId()
        {
            User user = JsonSerializer.Deserialize<User>(HttpContext.Session.GetString("user"));
            return user.user_id;
        }
    }
}
