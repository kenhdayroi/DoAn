using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LuxStay.Models
{
    public class DataProvider
    {
        private readonly SqlConnection _cnn; // Kết nối DB
        private SqlDataAdapter _da; // Xử lý các câu lệnh sql: select
        private SqlCommand _cmd; // Thực thi câu lệnh insert update
        string conectionString = "";
        public DataProvider(IConfiguration configuration)
        {
            string strCnn = configuration.GetConnectionString("DBConnect");
            conectionString = configuration.GetConnectionString("DBConnect");
            _cnn = new SqlConnection(strCnn);
            connect();
        }

        public void connect()
        {
            try
            {
                if (_cnn.State == ConnectionState.Open)
                {
                    _cnn.Close();
                }
                _cnn.Open();
                Console.WriteLine("Connect success !");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi kết nối: " + ex.Message);
            }
        }

        public DataTable excuteQuery(string strSelect)
        {
            DataTable dt = new DataTable(); 
            try
            {
                _da = new SqlDataAdapter(strSelect, _cnn);
                _da.Fill(dt);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi execute query: " + ex.Message);
            }
            return dt;
        }

        public void ExcuteNonQuery(string query)
        {
            try
            {
                _cmd = _cnn.CreateCommand();
                _cmd.CommandType = CommandType.Text;
                _cmd.CommandText = query;
                _cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi execute non query: " + ex.Message);
            }
        }
        public void InsertBooking(BookingDTO data,string query)
        {
            using (SqlConnection conn = new SqlConnection(conectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", data.user_id);
                    cmd.Parameters.AddWithValue("@tourId", data.tour_id);
                    cmd.Parameters.AddWithValue("@dateBooking", data.date_booking);
                    cmd.Parameters.AddWithValue("@price", data.price);
                    cmd.Parameters.AddWithValue("@totalPrice", data.total_price);
                    cmd.Parameters.AddWithValue("@people",data.people);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
