using System;

namespace LuxStay.Models
{
    public class Booking
    {
        public int booking_id { get; set; }

        public User user { get; set; }

        public Home home { get; set; }

        public DateTime date_check_in { get; set; }

        public DateTime date_check_out { get; set; }

        public int total_price { get; set; }
    }
}
