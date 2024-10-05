using LuxStay.Dao;
using LuxStay.Models;
using Microsoft.AspNetCore.Mvc;

namespace LuxStay.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RestoreController : Controller
    {
        private readonly DataProvider _dataProvider;
        private readonly IConfiguration _configuration;

        public RestoreController(DataProvider dataProvider, IConfiguration configuration)
        {
            _dataProvider = dataProvider;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            HomeDao homeDao = new HomeDao(_configuration);
            List<Home> homes = homeDao.findAllDeleted();
            ViewBag.homes = homes;
            return View();
        }

        public IActionResult RestoreHomestay()
        {
            if (!int.TryParse(HttpContext.Request.Query["home_id"], out int home_id))
            {
                TempData["Error"] = "ID homestay không hợp lệ!";
                return RedirectToAction("Index", "Error");
            }

            HomeDao homeDao = new HomeDao(_configuration);
            homeDao.restore(home_id);
            return Redirect("https://localhost:7017/Admin/Restore/Index");
        }

        public IActionResult ClearHomestay()
        {
            if (!int.TryParse(HttpContext.Request.Query["home_id"], out int home_id))
            {
                TempData["Error"] = "ID homestay không hợp lệ!";
                return RedirectToAction("Index", "Error");
            }

            HomeDao homeDao = new HomeDao(_configuration);
            Home home = homeDao.findById(home_id);

            if (home == null)
            {
                TempData["Error"] = "Homestay không tồn tại!";
                return RedirectToAction("Index", "Error");
            }

            PlaceDao placeDao = new PlaceDao(_dataProvider);
            int total_homestay = placeDao.totalHomestayByPlace(home.place.place_id);

            ImagesDetailDao imagesDetailDao = new ImagesDetailDao(_dataProvider);
            imagesDetailDao.clear(home_id);

          /*  BookingTourDao bookingDao = new BookingTourDao(_dataProvider);*/
            //bookingDao.clear(home_id);

            homeDao.clear(home_id);

            placeDao.updateTotalHomestay(total_homestay - 1, home.place.place_id);

            return Redirect("https://localhost:7017/Admin/Restore/Index");
        }
    }
}
