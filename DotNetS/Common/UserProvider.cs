using DotNetS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DotNetS.Common
{
    public class UserProvider
    {
        public static string GetConn()
        {
            return ConfigurationManager.ConnectionStrings["DotNetSConn"].ToString();
        }
        public static User GetCurrentUser(HttpContextBase context)
        {
            User user = context.Session[Constant.USER_INFO] as User;
            if (user == null)
            {
                string token = context.Request.Cookies[Constant.TOKEN].Value;
                string userId = JwtManager.GetClaim(token, ClaimTypeConst.USER_ID);
                string connectionString = JwtManager.GetClaim(token, GetConn());

                string sql = "SELECT u.UserId, " +
                            "u.UserName, " +
                            "u.FullName, " +
                            "u.Password, " +
                            "u.PhoneNumber, " +
                            "u.CreatedDate, " +
                            "u.GroupId " +
                            "FROM users u WHERE u.user_id = @userId";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@userId", userId);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            User u = new User();
                            u.UserId = Convert.ToInt32(reader[0]);
                            u.UserName = reader[1].ToString();
                            u.FullName = reader[2].ToString();
                            u.Password = null;
                            u.PhoneNumber = reader[4].ToString();
                            u.CreatedDate = Convert.ToInt32(reader[5]);
                            u.GroupId = Convert.ToInt32(reader[6]);

                            user = u;
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                context.Session[Constant.USER_INFO] = user;
            }
            return user;
        }
    }
}