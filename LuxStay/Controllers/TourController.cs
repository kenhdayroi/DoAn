using Microsoft.AspNetCore.Mvc;
using LuxStay.Dao; // Thêm namespace cho TourDao
using LuxStay.Models;
using System.Collections.Generic;

namespace LuxStay.Controllers
{
    public class TourController : Controller
    {
        private readonly TourDao _tourDao;

        public TourController(IConfiguration configuration)
        {
            DataProvider dataProvider = new DataProvider(configuration);
            _tourDao = new TourDao(dataProvider);
        }

        public IActionResult Detail(int id) 
        {
            var tour = _tourDao.GetTourById(id);
            if (tour == null)
            {
                return NotFound();
            }
            return View(tour);
        }

        [HttpPost]
        public IActionResult BookTour(DateTime tourDate)
        {
            return RedirectToAction("Index", "HistoryBookingTour");
        }

    }

}
