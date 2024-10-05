using LuxStay.Dao;
using LuxStay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LuxStay.Controllers
{
    public class PlaceController : Controller
    {
        private readonly IConfiguration _configuration;

        public PlaceController(IConfiguration configuration)
        {
            _configuration = configuration; 
        }

        public IActionResult Index()
        {
            string price_search = null;
            if (!string.IsNullOrEmpty(HttpContext.Request.Query["price_search"]))
            {
                price_search = HttpContext.Request.Query["price_search"];
                ViewData["price_search"] = price_search;
            }

            string home_type = null;
            if (!string.IsNullOrEmpty(HttpContext.Request.Query["home_type"]))
            {
                home_type = HttpContext.Request.Query["home_type"];
                ViewData["home_type"] = home_type;
            }

            string order_by = null;
            if (!string.IsNullOrEmpty(HttpContext.Request.Query["order_by"]))
            {
                order_by = HttpContext.Request.Query["order_by"];
                ViewData["order_by"] = order_by;
            }

            HomeDao homeDao = new HomeDao(_configuration);

            string place_id = HttpContext.Request.Query["place_id"];
            int total_home = int.Parse(HttpContext.Request.Query["total_home"]);
            string place_name = HttpContext.Request.Query["place_name"];

            int pageIndex = 1;
            if (!string.IsNullOrEmpty(HttpContext.Request.Query["pageIndex"]))
            {
                pageIndex = int.Parse(HttpContext.Request.Query["pageIndex"]);
            }
            int pageSize = 8;

            int totalPage = 0;

            int count = homeDao.countByPlace(place_id, home_type, price_search);

            totalPage = count / pageSize + (count % pageSize == 0 ? 0 : 1);

            ViewData["place_id"] = place_id;
            ViewData["total_home"] = total_home;
            ViewData["place_name"] = place_name;

            ViewData["totalPage"] = totalPage;
            ViewData["pageIndex"] = pageIndex;

            List<Home> homes = homeDao.findAllByPlaceId(place_id, pageIndex, pageSize, home_type, price_search, order_by);
            return View(homes);
        }
    }
}
