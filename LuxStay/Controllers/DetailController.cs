using LuxStay.Dao;
using LuxStay.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace LuxStay.Controllers
{
    public class DetailController : Controller
    {
        private readonly DataProvider _dataProvider;
        private readonly IConfiguration _configuration;

        public DetailController(DataProvider dataProvider, IConfiguration configuration)
        {
            _dataProvider = dataProvider;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            HomeDao homeDao = new HomeDao(_configuration);
            ImagesDetailDao imagesDetailDao = new ImagesDetailDao(_dataProvider);
            int home_id = int.Parse(Request.Query["home_id"]);

            int status = 0;
            BookingDao bookingDao = new BookingDao(_dataProvider);
            Booking booking = bookingDao.findByHomeId(home_id);
            if (booking != null)
             {
                 DateTime date_check_in = booking.date_check_in;
                 DateTime date_check_out = booking.date_check_out;
                 DateTime now = DateTime.Now;
                 if (now <= date_check_out)
                 {
                     status = 1;
                 }
                 else if (now > date_check_out)
                 {
                     status = 0;
                 }
             }

            Home home = homeDao.findById(home_id);
            home.status = status;

            List<ImagesDetail> imagesDetails = imagesDetailDao.findAllByHomeId(home_id);

            dynamic dy = new ExpandoObject();
            dy.home = home;
            dy.imagesDetails = imagesDetails;
            return View(dy);
        }
    }
}
