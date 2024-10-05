using LuxStay.Dao;
using LuxStay.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace Luxstay.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManagerHomestayController : Controller
    {
        private readonly DataProvider _dataProvider;
        private readonly IConfiguration _configuration;

        public ManagerHomestayController(DataProvider dataProvider, IConfiguration configuration)
        {
            _dataProvider = dataProvider;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertHomestay()
        {
            string image_intro = "";
            IFormFile file = Request.Form.Files["image_intro"];

            if (file != null && file.Length > 0) 
                try
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/product", 
                                               Path.GetFileName(file.FileName));
                    using (var stream = new FileStream(path, FileMode.Create)) 
                    {
                        file.CopyTo(stream); 
                    }
                    image_intro = Path.GetFileName(file.FileName);
                }
                catch
                {
                    Console.WriteLine("Lỗi upload image!"); 
                }
            string home_name = Request.Form["home_name"];
            string short_description = Request.Form["short_description"]; 
            int price = 0;
            int room_number = 0;
            try
            {
                price = Int32.Parse(Request.Form["price"]); 
            }
            catch (Exception)
            {
                TempData["Error"] = "Giá Phòng Phải Là Số!"; 
                return RedirectToAction("Index", "Error");
            }
            try
            {
                room_number = Int32.Parse(Request.Form["room_number"]); 
            }
            catch (Exception)
            {
                TempData["Error"] = "Số Phòng Ngủ Phải Là Số!"; 
                return RedirectToAction("Index", "Error");
            }
            string detail_description = Request.Form["detail_description"]; 
            detail_description = detail_description.Replace("break", "<br /><br />");
            string address = Request.Form["address"]; 
            string place_id = Request.Form["place"]; 
            string home_tpye = Request.Form["home_type"]; 
            if (home_tpye.Equals("canho"))
            {
                home_tpye = "Căn hộ dịch vụ";
            }
            else if (home_tpye.Equals("chungcu"))
            {
                home_tpye = "Chung cư";
            }
            else if (home_tpye.Equals("homestay"))
            {
                home_tpye = "Homestay";
            }
            else if (home_tpye.Equals("studio"))
            {
                home_tpye = "Studio";
            }
            else if (home_tpye.Equals("bietthu"))
            {
                home_tpye = "Biệt thự";
            }
            HomeDao homeDao = new HomeDao(_configuration);
            Home home = new Home();
            home.home_name = home_name;
            home.home_type = home_tpye;
            home.image_intro = image_intro;
            Place place = new Place();
            place.place_id = place_id;
            home.place = place;
            home.price = price;
            home.room_number = room_number;
            home.short_description = short_description;
            home.detail_description = detail_description;
            home.address = address;

            homeDao.insert(home);

            string image_1 = Request.Form["image_1"]; 
            string image_2 = Request.Form["image_2"]; 
            string image_3 = Request.Form["image_3"]; 

            int home_id_insert = homeDao.findHomeIdInsert();
            ImagesDetailDao imagesDetailDao = new ImagesDetailDao(_dataProvider);
            imagesDetailDao.insert(home_id_insert, image_1);
            imagesDetailDao.insert(home_id_insert, image_2);
            imagesDetailDao.insert(home_id_insert, image_3);

            PlaceDao placeDao = new PlaceDao(_dataProvider);
            int total_homestay = placeDao.totalHomestayByPlace(place_id);
            placeDao.updateTotalHomestay(total_homestay + 1, place_id);

            return Redirect("/Admin/Homestay");
        }


        [HttpPost]
        public IActionResult UpdateHomestay()
        {
            int home_id = Int32.Parse(Request.Form["home_id"]);
            string image_intro = Request.Form["image_intro"];
            string home_name = Request.Form["home_name"];
            string short_description = Request.Form["short_description"];
            int price = 0;
            int room_number = 0;

            try
            {
                price = Int32.Parse(Request.Form["price"]);
            }
            catch (Exception)
            {
                TempData["Error"] = "Giá Phòng Phải Là Số!";
                return RedirectToAction("Index", "Error");
            }

            try
            {
                room_number = Int32.Parse(Request.Form["room_number"]);
            }
            catch (Exception)
            {
                TempData["Error"] = "Số Phòng Ngủ Phải Là Số!";
                return RedirectToAction("Index", "Error");
            }

            string detail_description = Request.Form["detail_description"];
            detail_description = detail_description.Replace("break", "<br/><br/>");
            string address = Request.Form["address"];
            string place_id = Request.Form["place"];
            string home_tpye = Request.Form["home_type"];

            home_tpye = home_tpye switch
            {
                "canho" => "Căn hộ dịch vụ",
                "chungcu" => "Chung cư",
                "homestay" => "Homestay",
                "studio" => "Studio",
                "bietthu" => "Biệt thự",
                _ => home_tpye
            };

            HomeDao homeDao = new HomeDao(_configuration);
            Home home = new Home();
            home.home_id = home_id;
            home.home_name = home_name;
            home.home_type = home_tpye;
            home.image_intro = image_intro;
            Place place = new Place();
            place.place_id = place_id;
            home.place = place;
            home.price = price;
            home.room_number = room_number;
            home.short_description = short_description;
            home.detail_description = detail_description;
            home.address = address;

            ImagesDetailDao imagesDetailDao = new ImagesDetailDao(_dataProvider);
            imagesDetailDao.clear(home.home_id);

            string image_1 = Request.Form["image_1"];
            string image_2 = Request.Form["image_2"];
            string image_3 = Request.Form["image_3"];
            imagesDetailDao.insert(home.home_id, image_1);
            imagesDetailDao.insert(home.home_id, image_2);
            imagesDetailDao.insert(home.home_id, image_3);

            homeDao.update(home);

            return Redirect("/Admin/Homestay");
        }

        public IActionResult DeleteHomestay()
        {
            int home_id = Int32.Parse(Request.Query["home_id"]);
            HomeDao homeDao = new HomeDao(_configuration);
            homeDao.delete(home_id);
            return Redirect("/Admin/Homestay");
        }
    }
}
