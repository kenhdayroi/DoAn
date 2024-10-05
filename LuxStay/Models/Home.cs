using System;

namespace LuxStay.Models
{
    public class Home
    {
        public int home_id { get; set; }

        public string home_name { get; set; }

        public string home_type { get; set; }

        public int room_number { get; set; }

        public int price { get; set; }

        public Place place { get; set; }

        public string image_intro { get; set; }

        public string address { get; set; }

        public string short_description { get; set; }

        public string detail_description { get; set; }

        public int status { get; set; }
    }
}
