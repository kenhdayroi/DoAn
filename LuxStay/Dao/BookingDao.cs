using LuxStay.Models;
using System.Data;

namespace LuxStay.Dao
{
    public class BookingDao
    {
        private readonly DataProvider _dataProvider;

        public BookingDao(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public void Insert(int user_id, int home_id, DateTime date_check_in, DateTime date_check_out, int total_price)
        {
            string query = "INSERT INTO Booking (user_id, home_id, date_check_in, date_check_out, total_price) " +
                           "VALUES (" + user_id + ", " + home_id + ", '" + date_check_in.ToString("yyyy-MM-dd") + "', '" + date_check_out.ToString("yyyy-MM-dd") + "', " + total_price + ")";

            _dataProvider.ExcuteNonQuery(query);
        }



        public Booking findByHomeId(int home_id)
        {
            try
            {
                Booking booking = new Booking();

                String query = "select top(1) * from Booking "
                            + "where home_id = " + home_id
                            + " order by booking_id desc";
                DataTable dataTable = _dataProvider.excuteQuery(query);
                booking.booking_id = Int32.Parse(dataTable.Rows[0]["booking_id"].ToString());
                User user = new User();
                user.user_id = Int32.Parse(dataTable.Rows[0]["user_id"].ToString());
                booking.user = user;
                Home home = new Home();
                home.home_id = Int32.Parse(dataTable.Rows[0]["home_id"].ToString());
                booking.home = home;
                booking.date_check_in = DateTime.Parse(dataTable.Rows[0]["date_check_in"].ToString());
                booking.date_check_out = DateTime.Parse(dataTable.Rows[0]["date_check_out"].ToString());
                booking.total_price = Int32.Parse(dataTable.Rows[0]["total_price"].ToString());
                return booking;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Booking> findAllBookingByUserId(int user_id)
        {
            try
            {
                List<Booking> bookings = new List<Booking>();

                String query = "select * from Home h "
                            + "join Booking b "
                            + "on h.home_id = b.home_id "
                            + "join[User] u on u.[user_id] = b.[user_id] "
                            + "where u.[user_id] = " + user_id;
                DataTable dataTable = _dataProvider.excuteQuery(query);
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Booking booking = new Booking();
                    booking.booking_id = Int32.Parse(dataTable.Rows[i]["booking_id"].ToString());

                    DateTime date_check_in = DateTime.Parse(dataTable.Rows[i]["date_check_in"].ToString());
                    DateTime date_check_out = DateTime.Parse(dataTable.Rows[i]["date_check_out"].ToString());
                    booking.date_check_in = date_check_in;
                    booking.date_check_out = date_check_out;
                    booking.total_price = Int32.Parse(dataTable.Rows[i]["total_price"].ToString());

                    int status = 0;
                    BookingDao bookingDao = new BookingDao(_dataProvider);

                    DateTime now = DateTime.Now;
                    if (now <= date_check_in)
                    {
                        status = 2; 
                    } 
                    else if (now <= date_check_out)
                    {
                        status = 1; 
                    } 
                    else if (now > date_check_out) 
                    {
                        status = 0; 
                    }

                    User user = new User();
                    user.user_id = Int32.Parse(dataTable.Rows[i]["user_id"].ToString());
                    user.email = dataTable.Rows[i]["email"].ToString();
                    user.password = dataTable.Rows[i]["password"].ToString();
                    user.name = dataTable.Rows[i]["name"].ToString();
                    user.role = dataTable.Rows[i]["role"].ToString();
                    user.phone = dataTable.Rows[i]["phone"].ToString();
                    user.address = dataTable.Rows[i]["address"].ToString();
                    user.gender = bool.Parse(dataTable.Rows[i]["gender"].ToString());
                    user.verify = bool.Parse(dataTable.Rows[i]["verify"].ToString());
                    booking.user = user;

                    Home home = new Home();
                    home.home_id = Int32.Parse(dataTable.Rows[i]["home_id"].ToString());
                    home.home_name = dataTable.Rows[i]["home_name"].ToString();
                    home.home_type = dataTable.Rows[i]["home_type"].ToString();
                    home.room_number = Int32.Parse(dataTable.Rows[i]["room_number"].ToString());
                    home.price = Int32.Parse(dataTable.Rows[i]["price"].ToString());
                    home.image_intro = dataTable.Rows[i]["image_intro"].ToString();
                    home.address = dataTable.Rows[i]["address"].ToString();
                    home.short_description = dataTable.Rows[i]["short_description"].ToString();
                    home.detail_description = dataTable.Rows[i]["detail_description"].ToString();
                    home.status = status;
                    booking.home = home;

                    bookings.Add(booking);
                }
                return bookings;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void clear(int home_id)
        {
            try
            {
                string query = "Delete from Booking Where home_id = " + home_id;
                _dataProvider.ExcuteNonQuery(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi insert user: " + ex.Message);
            }
        }

        public void deleteById(int booking_id)
        {
            try
            {
                string query = "Delete from Booking Where booking_id = " + booking_id;
                _dataProvider.ExcuteNonQuery(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi insert user: " + ex.Message);
            }
        }

        public List<Booking> findAllBooking()
        {
            try
            {
                List<Booking> bookings = new List<Booking>();

                String query = "select * from Home h "
                            + "join Booking b "
                            + "on h.home_id = b.home_id "
                            + "join[User] u on u.[user_id] = b.[user_id] ";
                DataTable dataTable = _dataProvider.excuteQuery(query);
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Booking booking = new Booking();
                    booking.booking_id = Int32.Parse(dataTable.Rows[i]["booking_id"].ToString());

                    DateTime date_check_in = DateTime.Parse(dataTable.Rows[i]["date_check_in"].ToString());
                    DateTime date_check_out = DateTime.Parse(dataTable.Rows[i]["date_check_out"].ToString());
                    booking.date_check_in = date_check_in;
                    booking.date_check_out = date_check_out;
                    booking.total_price = Int32.Parse(dataTable.Rows[i]["total_price"].ToString());

                    int status = 0;
                    BookingDao bookingDao = new BookingDao(_dataProvider);

                    DateTime now = DateTime.Now;
                    if (now <= date_check_in)
                    {
                        status = 2;
                    } 
                    else if (now <= date_check_out)
                    {
                        status = 1; 
                    } 
                    else if (now > date_check_out) 
                    {
                        status = 0; 
                    }

                    User user = new User();
                    user.user_id = Int32.Parse(dataTable.Rows[i]["user_id"].ToString());
                    user.email = dataTable.Rows[i]["email"].ToString();
                    user.password = dataTable.Rows[i]["password"].ToString();
                    user.name = dataTable.Rows[i]["name"].ToString();
                    user.role = dataTable.Rows[i]["role"].ToString();
                    user.phone = dataTable.Rows[i]["phone"].ToString();
                    user.address = dataTable.Rows[i]["address"].ToString();
                    user.gender = bool.Parse(dataTable.Rows[i]["gender"].ToString());
                    user.verify = bool.Parse(dataTable.Rows[i]["verify"].ToString());
                    booking.user = user;

                    Home home = new Home();
                    home.home_id = Int32.Parse(dataTable.Rows[i]["home_id"].ToString());
                    home.home_name = dataTable.Rows[i]["home_name"].ToString();
                    home.home_type = dataTable.Rows[i]["home_type"].ToString();
                    home.room_number = Int32.Parse(dataTable.Rows[i]["room_number"].ToString());
                    home.price = Int32.Parse(dataTable.Rows[i]["price"].ToString());
                    home.image_intro = dataTable.Rows[i]["image_intro"].ToString();
                    home.address = dataTable.Rows[i]["address"].ToString();
                    home.short_description = dataTable.Rows[i]["short_description"].ToString();
                    home.detail_description = dataTable.Rows[i]["detail_description"].ToString();
                    home.status = status;
                    booking.home = home;

                    bookings.Add(booking);
                }
                return bookings;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    } 
}
