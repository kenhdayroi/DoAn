using LuxStay.Dao;
using LuxStay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Luxstay.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomestayController : Controller
    {
        private readonly DataProvider _dataProvider;
        private readonly IConfiguration _configuration;

        public HomestayController(DataProvider dataProvider, IConfiguration configuration)
        {
            _dataProvider = dataProvider;
            _configuration = configuration;
        }

        // GET: Admin/Homestay
        public IActionResult Index()
        {
            HomeDao homeDao = new HomeDao(_configuration);
            int pageIndex = 1;

            if (HttpContext.Request.Query["pageIndex"].Count > 0)
            {
                pageIndex = Int32.Parse(HttpContext.Request.Query["pageIndex"]);
            }

            int pageSize = 10;
            int totalPage = 0;

            int count = homeDao.count();

            if (count % pageSize == 0)
            {
                totalPage = count / pageSize;
            }
            else
            {
                totalPage = count / pageSize + 1;
            }

            ViewData["totalPage"] = totalPage;
            ViewData["pageIndex"] = pageIndex;

            List<Home> homes = homeDao.findAll(pageIndex, pageSize);
            ViewBag.homes = homes;

            return View();
        }

        public IActionResult InsertHome()
        {
            return View();
        }

        public ActionResult UpdateHome()
        {
            int home_id = int.Parse(HttpContext.Request.Query["home_id"]);
            HomeDao homeDao = new HomeDao(_configuration);
            Home home = homeDao.findById(home_id);
            home.detail_description = home.detail_description.Replace("<br/><br/>", "break");
            home.detail_description = home.detail_description.Replace("<br /><br />", "break");
            home.detail_description = home.detail_description.Replace("<br/>", "break");
            home.detail_description = home.detail_description.Replace("<br />", "break");
            ImagesDetailDao imagesDetailDao = new ImagesDetailDao(_dataProvider);

            List<ImagesDetail> imagesDetails = new List<ImagesDetail>();
            imagesDetails = imagesDetailDao.findAllByHomeId(home_id);
            dynamic dy = new ExpandoObject();
            dy.home = home;
            dy.imagesDetails = imagesDetails;

            if (home.home_type.Equals("Phòng Đơn"))
            {
                ViewBag.home_type = "PhongDon";
            }
            else if (home.home_type.Equals("Phòng Đôi"))
            {
                ViewBag.home_type = "Phòng Đôi";
            }
            else if (home.home_type.Equals("Phòng Deluxe"))
            {
                ViewBag.home_type = "Phòng Deluxe";
            }
            else if (home.home_type.Equals("Phòng Studio"))
            {
                ViewBag.home_type = "studio";
            }
            else if (home.home_type.Equals("Phòng standard"))
            {
                ViewBag.home_type = "standard";
            }

            return View(dy);
        }

    }
}
