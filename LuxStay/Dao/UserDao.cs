using LuxStay.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LuxStay.Dao
{
    public class UserDao
    {
        private readonly DataProvider _dataProvider;

        public UserDao(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public List<User> findAll()
        {
            string query = "SELECT * FROM [User]";
            List<User> users = new List<User>();
            try
            {
                DataTable dataTable = _dataProvider.excuteQuery(query);
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    User user = new User
                    {
                        user_id = int.Parse(dataTable.Rows[i]["user_id"].ToString()),
                        email = dataTable.Rows[i]["email"].ToString(),
                        password = dataTable.Rows[i]["password"].ToString(),
                        name = dataTable.Rows[i]["name"].ToString(),
                        role = dataTable.Rows[i]["role"].ToString(),
                        phone = dataTable.Rows[i]["phone"].ToString(),
                        address = dataTable.Rows[i]["address"].ToString(),
                        gender = bool.Parse(dataTable.Rows[i]["gender"].ToString()),
                        verify = bool.Parse(dataTable.Rows[i]["verify"].ToString())
                    };
                    users.Add(user);
                }
                return users;
            }
            catch
            {
                return null;
            }
        }

        public User findByEmailAndPassword(string email, string password)
        {
            string query = $"SELECT * FROM [User] WHERE email = '{email}' and password = '{password}'";
            try
            {
                DataTable dataTable = _dataProvider.excuteQuery(query);
                User user = new User
                {
                    user_id = int.Parse(dataTable.Rows[0]["user_id"].ToString()),
                    email = dataTable.Rows[0]["email"].ToString(),
                    password = dataTable.Rows[0]["password"].ToString(),
                    name = dataTable.Rows[0]["name"].ToString(),
                    role = dataTable.Rows[0]["role"].ToString(),
                    phone = dataTable.Rows[0]["phone"].ToString(),
                    address = dataTable.Rows[0]["address"].ToString(),
                    gender = bool.Parse(dataTable.Rows[0]["gender"].ToString()),
                    verify = bool.Parse(dataTable.Rows[0]["verify"].ToString())
                };
                return user;
            }
            catch
            {
                return null;
            }
        }

        public User findByEmail(string email)
        {
            string query = $"SELECT * FROM [User] WHERE email = '{email}'";
            try
            {
                DataTable dataTable = _dataProvider.excuteQuery(query);
                User user = new User
                {
                    user_id = int.Parse(dataTable.Rows[0]["user_id"].ToString()),
                    email = dataTable.Rows[0]["email"].ToString(),
                    password = dataTable.Rows[0]["password"].ToString(),
                    name = dataTable.Rows[0]["name"].ToString(),
                    role = dataTable.Rows[0]["role"].ToString(),
                    phone = dataTable.Rows[0]["phone"].ToString(),
                    address = dataTable.Rows[0]["address"].ToString(),
                    gender = bool.Parse(dataTable.Rows[0]["gender"].ToString()),
                    verify = bool.Parse(dataTable.Rows[0]["verify"].ToString())
                };
                return user;
            }
            catch
            {
                return null;
            }
        }

        public void insert(User user)
        {
            try
            {
                string query = $"INSERT INTO [User] VALUES('{user.email}', '{user.phone}', N'{user.name}', '{user.password}', 1, N'{user.address}', 'ROLE_USER', 1)";
                _dataProvider.ExcuteNonQuery(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi insert user: " + ex.Message);
            }
        }

        public User findById(int user_id)
        {
            string query = $"SELECT * FROM [User] WHERE user_id = {user_id}";
            try
            {
                DataTable dataTable = _dataProvider.excuteQuery(query);
                User user = new User
                {
                    user_id = int.Parse(dataTable.Rows[0]["user_id"].ToString()),
                    email = dataTable.Rows[0]["email"].ToString(),
                    password = dataTable.Rows[0]["password"].ToString(),
                    name = dataTable.Rows[0]["name"].ToString(),
                    role = dataTable.Rows[0]["role"].ToString(),
                    phone = dataTable.Rows[0]["phone"].ToString(),
                    address = dataTable.Rows[0]["address"].ToString(),
                    gender = bool.Parse(dataTable.Rows[0]["gender"].ToString()),
                    verify = bool.Parse(dataTable.Rows[0]["verify"].ToString())
                };
                return user;
            }
            catch
            {
                return null;
            }
        }

        public void update(User user)
        {
            try
            {
                string query = $"UPDATE [User] SET phone = '{user.phone}', [name] = N'{user.name}', [address] = N'{user.address}', [role] = '{user.role}', [password] = '{user.password}' WHERE [user_id] = {user.user_id}";
                _dataProvider.ExcuteNonQuery(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi insert user: " + ex.Message);
            }
        }

        public void delete(int user_id)
        {
            try
            {
                string query = $"DELETE FROM [User] WHERE user_id = {user_id}";
                _dataProvider.ExcuteNonQuery(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Không thể xóa : " + ex.Message);
            }
        }

        public void updatePasswordByEmail(string email, string password)
        {
            try
            {
                string query = $"UPDATE [User] SET [password] = '{password}' WHERE [email] = '{email}'";
                _dataProvider.ExcuteNonQuery(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi update password: " + ex.Message);
            }
        }
    }
}
