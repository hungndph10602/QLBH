using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.Models
{
    public class LoginModel
    {
        [Display(Name ="Email đăng nhập")]
        [Required(ErrorMessage ="Bạn phải nhập email")]
        public string username { get; set; }
        [Display(Name = "Mật Khẩu đăng nhập")]
        [Required(ErrorMessage = "Bạn phải nhập mật khẩu")]
        public string password { get; set; }
    }
}
