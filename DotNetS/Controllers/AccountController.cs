using AutoMapper;
using DotNetS.Common;
using DotNetS.Models;
using DotNetS.Models.Register;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DotNetS.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Account
        public ActionResult Login()
        {
            CookieProvider.Remove(Cookies.TOKEN);
            SessionProvider.Remove(Common.Session.USER);
            CookieProvider.Remove(Common.Cookies.LOGIN_FIELD);
            return View();
        }
        private bool Authentication(string password_hash, string password)
        {
            if (password_hash.Equals(GenerateHash(password)))
            {
                return true;
            }

            return false;
        }
        private string GenerateHash(string pass)
        {
            using (var sha512 = SHA512.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(pass));
                // Get the hashed string.  
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                // Print the string.   
                return hash;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string sql = "Select UserId, UserName, FullName, Password, PhoneNumber, CreatedDate, GroupId from Users where username = @username";
                    //string sql = "SELECT * FROM user WHERE login_id = @id AND del_flg = 0";
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        SqlCommand command = new SqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@username", model.UserName);
                        User user = new User();
                        try
                        {
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();

                            while (reader.Read())
                            {
                                user.UserId = Convert.ToInt32(reader[0]);
                                user.UserName = reader[1].ToString();
                                user.FullName = reader[2].ToString();
                                user.Password = reader[3].ToString();
                                user.PhoneNumber = reader[4].ToString();
                                user.CreatedDate = Convert.ToInt32(reader[5]);
                                user.GroupId = Convert.ToInt32(reader[6]);
                            }
                            reader.Close();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        if (!String.IsNullOrEmpty(user.UserName))
                        {
                            if (Authentication(user.Password, model.Password) == false)
                            {
                                ModelState.AddModelError("", "(*) Tên đăng nhập hoặc mật khẩu không khớp, xin kiểm tra lại.");
                                return View();
                            }

                            SessionProvider.Set(Common.Session.USER, user);

                            string access_key = Guid.NewGuid().ToString();

                            //Set Cookie
                            var claimsIdentity = new ClaimsIdentity();

                            claimsIdentity.AddClaim(new Claim(ClaimTypeConst.USER_ID, user.UserId.ToString()));
                            claimsIdentity.AddClaim(new Claim(ClaimTypeConst.USER_NAME, user.UserName));
                            claimsIdentity.AddClaim(new Claim(ClaimTypeConst.USER_FULLNAME, user.FullName));
                            claimsIdentity.AddClaim(new Claim(ClaimTypeConst.USER_PHONENUMBER, user.PhoneNumber));
                            claimsIdentity.AddClaim(new Claim(ClaimTypeConst.USER_CREATEDDATE, user.CreatedDate.ToString()));
                            claimsIdentity.AddClaim(new Claim(ClaimTypeConst.USER_GROUPID, user.GroupId.ToString()));
                            claimsIdentity.AddClaim(new Claim(Cookies.ACCESS_KEY, access_key));

                            string token = JwtManager.GenerateToken(claimsIdentity);
                            CookieProvider.Set(Cookies.TOKEN, token);
                            CookieProvider.Set(Cookies.ACCESS_KEY, access_key);

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "(*) Tên đăng nhập hoặc mật khẩu không khớp, xin kiểm tra lại.");
                            return View();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "(*) Thông tin đăng nhập chưa đúng, xin kiểm tra lại.");
                    return View();
                }    
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);
                return View();
            }
        }
        public ActionResult Logout(int userId)
        {
            CookieProvider.Remove(Cookies.TOKEN);
            CookieProvider.Remove(Cookies.ACCESS_KEY);
            SessionProvider.Remove(Common.Session.USER);
            SessionProvider.Remove(Common.Session.MOD_CONST);
            return Redirect("/account/login");
        }
        [HttpGet]
        public ActionResult Register()
        {
            List<SelectItems> items = new List<SelectItems>(){
                new SelectItems()
                {
                    id = "0",
                    text = "Admin"
                },
                new SelectItems()
                {
                    id = "1",
                    text = "Quản trị viên"
                },
                new SelectItems()
                {
                    id = "2",
                    text = "Người dùng"
                }
            };
            ViewBag.GroupIds = new SelectList(items, "id", "text");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVMModel userVM)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    userVM.Password = GenerateHash(userVM.Password);
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<RegisterVMModel, RegisterModel>());
                    var mapper = config.CreateMapper();
                    var user = mapper.Map<RegisterModel>(userVM);

                    user.CreatedDate = (int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                    connection.Open();
                    //Check user name is exist

                    string sql = "Select count(*) from Users where username = @username";
                    SqlCommand cmd1 = new SqlCommand(sql, connection);
                    cmd1.Parameters.AddWithValue("@username", user.UserName);
                    int isExist = (int)cmd1.ExecuteScalar();

                    if(isExist <= 0)
                    {
                        SqlCommand cmd = SqlProvider.CreateSQLCommand(user, "Users", null, true, connection, "userId");
                        cmd.ExecuteNonQuery();

                        return RedirectToAction("Login", "Account");
                    }
                }
            }

            return View();
        }
    }
}