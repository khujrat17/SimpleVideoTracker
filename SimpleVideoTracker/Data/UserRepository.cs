using System;
using MySql.Data.MySqlClient;
using SimpleVideoTracker.Models;

namespace SimpleVideoTracker.Data
{
    public class UserRepository
    {
        public User GetUserByEmail(string email)
        {
            User user = null;
            
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT UserId, Email, Password, CreatedDate FROM Users WHERE Email = @Email";
                
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                UserId = reader.GetInt32("UserId"),
                                Email = reader.GetString("Email"),
                                Password = reader.GetString("Password"),
                            };
                        }
                    }
                }
            }
            
            return user;
        }

        public bool CreateUser(string email, string password)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO Users (Email, Password,CreatedDate) VALUES (@Email, @Password)";
                
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool ValidateUser(string email, string password)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND Password = @Password";
                
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }
    }
}
