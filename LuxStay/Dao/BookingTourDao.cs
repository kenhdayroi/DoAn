using System;
using System.Collections.Generic;
using System.Data;
using LuxStay.Models;

namespace LuxStay.Models
{
    public class BookingTourDao
    {
        private readonly DataProvider _dataProvider;

        public BookingTourDao(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public void InsertBookingTour(BookingDTO bookingTour)
        {
            /*  string query = $"INSERT INTO BookingTour (user_id, tour_id, date_booking, total_price, people) " +
                             $"VALUES ({bookingTour.user_id}, {bookingTour.tour_id}, '{bookingTour.date_booking}', " +
                             $"{bookingTour.total_price}, {bookingTour.people})";*/
            string query = "INSERT INTO BookingTour (user_id,tour_id, date_booking, price, total_price, people) " +
                        "VALUES (@userId,@tourId, @dateBooking, @price, @totalPrice, @people)";
            _dataProvider.InsertBooking(bookingTour,query);
        }



        public List<BookingTour> GetBookingsByUserId(int userId)
        {
            List<BookingTour> bookings = new List<BookingTour>();
            string query = $"SELECT * FROM BookingTour WHERE user_id = {userId}";
            DataTable dt = _dataProvider.excuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                bookings.Add(new BookingTour
                {
                    booking_tour_id = Convert.ToInt32(row["booking_tour_id"]),
                    user_id = Convert.ToInt32(row["user_id"]),
                    tour_id = Convert.ToInt32(row["tour_id"]),
                    date_booking = Convert.ToDateTime(row["date_booking"]),
                    price = Convert.ToDecimal(row["price"]),
                    total_price = Convert.ToDecimal(row["total_price"]),
                    people = Convert.ToInt32(row["people"])
                });
            }
            return bookings;
        }
    }
}
