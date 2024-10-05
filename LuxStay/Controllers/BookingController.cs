using LuxStay.Dao;
using LuxStay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace LuxStay.Controllers
{
    public class BookingController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly DataProvider _dataProvider;
        private readonly SendMailDao _sendMailDao;

        public BookingController(IConfiguration configuration, DataProvider dataProvider, SendMailDao sendMailDao)
        {
            _configuration = configuration;
            _dataProvider = dataProvider;
            _sendMailDao = sendMailDao;
        }

        [HttpPost]
        public IActionResult Index()
        {
            HomeDao homeDao = new HomeDao(_configuration);
            int home_id = Int32.Parse(Request.Form["home_id"]);
            Home home = homeDao.findById(home_id);
            User user = HttpContext.Session.GetObjectFromJson<User>("user");

            BookingDao bookingDao = new BookingDao(_dataProvider);

            try
            {
                string str_date_check_in = Request.Form["dateCheckIn"];
                string str_date_check_out = Request.Form["dateCheckOut"];

                if (!DateTime.TryParse(str_date_check_in, out DateTime date_check_in))
                {
                    HttpContext.Session.SetString("Error", "Ngày nhận phòng không hợp lệ!");
                    return RedirectToAction("Index", "Error");
                }

                if (!DateTime.TryParse(str_date_check_out, out DateTime date_check_out))
                {
                    HttpContext.Session.SetString("Error", "Ngày trả phòng không hợp lệ!");
                    return RedirectToAction("Index", "Error");
                }

                DateTime now = DateTime.Now;

                if (date_check_in < now)
                {
                    HttpContext.Session.SetString("Error", "Ngày Đặt Phòng Phải Lớn Hơn Ngày Hiện Tại!");
                    return RedirectToAction("Index", "Error");
                }
                else
                {
                    TimeSpan TimeCheckInt = date_check_in - now;
                    if (TimeCheckInt.Days > 7)
                    {
                        HttpContext.Session.SetString("Error", "Thời Gian Đặt Phòng Trước Không Quá 7 Ngày Tính Từ Ngày Hôm Nay!");
                        return RedirectToAction("Index", "Error");
                    }
                    else
                    {
                        if (date_check_out <= date_check_in)
                        {
                            HttpContext.Session.SetString("Error", "Thời Gian Đặt Phòng Phải Ít Nhất Là 1 Ngày!");
                            return RedirectToAction("Index", "Error");
                        }
                        else
                        {
                            TimeSpan TimeCheckOut = date_check_out - date_check_in;
                            int total_day_number = TimeCheckOut.Days;
                            int total_price = total_day_number * home.price;

                            bookingDao.Insert(user.user_id, home.home_id, date_check_in, date_check_out, total_price);

                            string subject = "Xác nhận đặt phòng LuxStay";
                            string content = $"Xin chào {user.name},<br><br>" +
                                             $"Cảm ơn bạn đã đặt phòng tại LuxStay.<br>" +
                                             $"Thông tin đặt phòng:<br>" +
                                             $"- Phòng: {home.home_name}<br>" +
                                             $"- Ngày nhận phòng: {date_check_in:dd/MM/yyyy}<br>" +
                                             $"- Ngày trả phòng: {date_check_out:dd/MM/yyyy}<br>" +
                                             $"- Tổng giá: {total_price} VND<br><br>" +
                                             "Chúc bạn có kỳ nghỉ vui vẻ!<br>LuxStay";

                            _sendMailDao.SendMail(user.email, subject, content);

                            return RedirectToAction("Index", "HistoryBooking");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("Error", "Hãy Đảm Bảo Rằng Bạn Đã Nhập Đúng Ngày/Tháng/Năm khi đặt phòng!");
                return RedirectToAction("Index", "Error");
            }
        }

    }
}
