using LuxStay.Models;
using System.Collections.Generic;
using System.Data;

namespace LuxStay.Dao
{
    public class TourDao
    {
        private readonly DataProvider _dataProvider;

        public TourDao(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public List<Tour> GetTours()
        {
            string query = "SELECT * FROM Tour";
            var dataTable = _dataProvider.excuteQuery(query);
            List<Tour> tours = new List<Tour>();

            foreach (DataRow row in dataTable.Rows)
            {
                tours.Add(new Tour
                {
                    tour_id = (int)row["tour_id"],
                    tour_name = (string)row["tour_name"],
                    tour_type = (string)row["tour_type"],
                    tour_number = (int)row["tour_number"],
                    price = (int)row["price"],
                    image = (string)row["image"],
                    address = (string)row["address"],
                    short_description = (string)row["short_description"],
                    detail_description = (string)row["detail_description"]
                });
            }
            return tours;
        }

        public Tour GetTourById(int id)
        {
            string query = $"SELECT * FROM Tour WHERE tour_id = {id}"; 
            var dataTable = _dataProvider.excuteQuery(query);

            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0]; 
                return new Tour
                {
                    tour_id = (int)row["tour_id"],
                    tour_name = (string)row["tour_name"],
                    tour_type = (string)row["tour_type"],
                    tour_number = (int)row["tour_number"],
                    price = (int)row["price"],
                    image = (string)row["image"],
                    address = (string)row["address"],
                    short_description = (string)row["short_description"],
                    detail_description = (string)row["detail_description"]
                };
            }
            return null; 
        }
    }
}
