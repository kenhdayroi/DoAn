using LuxStay.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace LuxStay.Dao
{
    public class ImagesDetailDao
    {
        private readonly DataProvider _dataProvider;

        public ImagesDetailDao(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public List<ImagesDetail> findAllByHomeId(int home_id)
        {
            List<ImagesDetail> imagesDetails = new List<ImagesDetail>();
            string query = "SELECT * FROM Images_Detail WHERE home_id = " + home_id;
            DataTable dataTable = _dataProvider.excuteQuery(query);
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                ImagesDetail imagesDetail = new ImagesDetail
                {
                    image_id = int.Parse(dataTable.Rows[i]["image_id"].ToString()),
                    home_id = int.Parse(dataTable.Rows[i]["home_id"].ToString()),
                    image_url = dataTable.Rows[i]["image_url"].ToString()
                };
                imagesDetails.Add(imagesDetail);
            }
            return imagesDetails;
        }

        public void insert(int home_id, string image_url)
        {
            try
            {
                string query = "INSERT INTO Images_Detail (home_id, image_url) VALUES (" + home_id + ", '" + image_url + "')";
                _dataProvider.ExcuteNonQuery(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi insert user: " + ex.Message);
            }
        }

        public void clear(int home_id)
        {
            try
            {
                string query = "DELETE FROM Images_Detail WHERE home_id = " + home_id;
                _dataProvider.ExcuteNonQuery(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi clear images: " + ex.Message);
            }
        }
    }
}
