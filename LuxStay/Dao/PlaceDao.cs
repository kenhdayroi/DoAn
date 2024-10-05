using LuxStay.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace LuxStay.Dao
{
    public class PlaceDao
    {
        private readonly DataProvider _dataProvider;

        public PlaceDao(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public List<Place> findAll()
        {
            List<Place> places = new List<Place>();
            string query = "SELECT * FROM Place";
            DataTable dataTable = _dataProvider.excuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                Place place = new Place
                {
                    place_id = row["place_id"].ToString(),
                    place_name = row["place_name"].ToString(),
                    image = row["image"].ToString(),
                    total_home = Convert.ToInt32(row["total_home"])
                };
                places.Add(place);
            }
            return places;
        }

        public int totalHomestayByPlace(string place_id)
        {
            int total_homestay = 0;
            string query = "SELECT p.total_home FROM Place p WHERE place_id = @place_id";
            var parameters = new[] { new SqlParameter("@place_id", place_id) };
            DataTable dataTable = _dataProvider.excuteQuery(query);

            if (dataTable.Rows.Count > 0)
            {
                total_homestay = Convert.ToInt32(dataTable.Rows[0]["total_home"]);
            }
            return total_homestay;
        }

        public void updateTotalHomestay(int totalHomestay, string place_id)
        {
            try
            {
                string query = "Update Place set total_home = " + totalHomestay
                            + " where place_id = '" + place_id + "'";
                _dataProvider.ExcuteNonQuery(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi insert user: " + ex.Message);
            }
        }
    }
}
