using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetS.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public int CreatedDate { get; set; }
        public int? GroupId { get; set; }
    }
}