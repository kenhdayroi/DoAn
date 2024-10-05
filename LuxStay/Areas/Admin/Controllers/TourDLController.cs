using Microsoft.AspNetCore.Mvc;
using LuxStay.Models; 
using System.Collections.Generic;
using System.Data;

namespace LuxStay.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TourDLController : Controller
    {
        private readonly DataProvider _dataProvider;

        public TourDLController(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public IActionResult Index()
        {
            List<Tour> tours = new List<Tour>();
            string query = "SELECT * FROM Tour";
            DataTable dt = _dataProvider.excuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                tours.Add(new Tour
                {
                    tour_id = (int)row["tour_id"],
                    tour_name = row["tour_name"].ToString(),
                    tour_type = row["tour_type"].ToString(),
                    tour_number = (int)row["tour_number"],
                    price = (int)row["price"],
                    place_id = row["place_id"].ToString(),
                    image = row["image"].ToString(),
                    address = row["address"].ToString(),
                    short_description = row["short_description"].ToString(),
                    detail_description = row["detail_description"].ToString()
                });
            }

            return View(tours);
        }

        public IActionResult InsertTour()
        {
            ViewBag.Places = GetPlaces();
            return View();
        }

        [HttpPost]
        public IActionResult InsertTour(Tour tour)
        {
            string query = "INSERT INTO Tour (tour_name, tour_type, tour_number, price, place_id, image, address, short_description, detail_description) " +
                           "VALUES (N'" + tour.tour_name + "', N'" + tour.tour_type + "', " + tour.tour_number + ", " + tour.price + ", '" + tour.place_id + "', '" + tour.image + "', N'" + tour.address + "', N'" + tour.short_description + "', N'" + tour.detail_description + "')";
            _dataProvider.ExcuteNonQuery(query);
            return Redirect("/Admin/TourDL");
        }


        public IActionResult UpdateTour(int id)
        {
            Tour tour = null;
            string query = "SELECT * FROM Tour WHERE tour_id = " + id;
            DataTable dt = _dataProvider.excuteQuery(query);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                tour = new Tour
                {
                    tour_id = (int)row["tour_id"],
                    tour_name = row["tour_name"].ToString(),
                    tour_type = row["tour_type"].ToString(),
                    tour_number = (int)row["tour_number"],
                    price = (int)row["price"],
                    place_id = row["place_id"].ToString(),
                    image = row["image"].ToString(),
                    address = row["address"].ToString(),
                    short_description = row["short_description"].ToString(),
                    detail_description = row["detail_description"].ToString()
                };
            }

            ViewBag.Places = GetPlaces();
            return View(tour);
        }

        [HttpPost]
        public IActionResult UpdateTour(Tour tour)
        {
            string query = "UPDATE Tour SET " +
                           "tour_name = N'" + tour.tour_name + "', " +
                           "tour_type = N'" + tour.tour_type + "', " +
                           "tour_number = " + tour.tour_number + ", " +
                           "price = " + tour.price + ", " +
                           "place_id = '" + tour.place_id + "', " +
                           "image = '" + tour.image + "', " +
                           "address = N'" + tour.address + "', " +
                           "short_description = N'" + tour.short_description + "', " +
                           "detail_description = N'" + tour.detail_description + "' " +
                           "WHERE tour_id = " + tour.tour_id;
            _dataProvider.ExcuteNonQuery(query);
            return Redirect("/Admin/TourDL"); 
        }


        public IActionResult DeleteTour(int id)
        {
            string query = "DELETE FROM Tour WHERE tour_id = " + id;
            _dataProvider.ExcuteNonQuery(query);
            return Redirect("/Admin/TourDL"); 
        }

        private List<Place> GetPlaces()
        {
            List<Place> places = new List<Place>();
            string query = "SELECT * FROM Place";
            DataTable dt = _dataProvider.excuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                places.Add(new Place
                {
                    place_id = row["place_id"].ToString(),
                    place_name = row["place_name"].ToString(),
                    image = row["image"].ToString(),
                    total_home = (int)row["total_home"]
                });
            }

            return places;
        }
    }
}
