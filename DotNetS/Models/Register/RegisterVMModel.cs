using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DotNetS.Models.Register
{
    public class RegisterVMModel
    {
        [Required]
        [DisplayName("UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [DisplayName("Retype Password")]
        public string ReTypePassword { get; set; }

        [DisplayName("Họ Tên")]
        public string FullName { get; set; }
        [DisplayName("Số điện thoại")]
        [Phone]
        public string PhoneNumber { get; set; }
        public int CreatedDate { get; set; }
        public int? GroupId { get; set; } = 2;
    }
}