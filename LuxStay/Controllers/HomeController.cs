using Microsoft.AspNetCore.Mvc;
using LuxStay.Models;
using System;
using LuxStay.Dao;
using System.Dynamic;
using Microsoft.Extensions.Configuration;

namespace LuxStay.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataProvider _dataProvider;
        private readonly IConfiguration _configuration;

        public HomeController(DataProvider dataProvider, IConfiguration configuration)
        {
            _dataProvider = dataProvider;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            SendMailDao sendMailDao = new SendMailDao(_configuration);
            HomeDao homeDao = new HomeDao(_configuration);
            PlaceDao placeDao = new PlaceDao(_dataProvider);
            TourDao tourDao = new TourDao(_dataProvider); 

            int pageIndex = 1;
            if (Request.Query.ContainsKey("pageIndex"))
            {
                pageIndex = Int32.Parse(Request.Query["pageIndex"]);
            }
            int pageSize = 8;

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

            dynamic dy = new ExpandoObject();
            dy.homes = homeDao.findAll(pageIndex, pageSize);
            dy.places = placeDao.findAll();
            dy.tours = tourDao.GetTours(); 

            return View(dy);
        }
    }
}
