using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MemberTest.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MemberTest.ViewModels
{
    public class MemberRegisterViewModel
    {
        public Member newmember { get; set; }

        [DisplayName("密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        [StringLength(30, ErrorMessage = "密碼長度介於 6 ~ 30 之間", MinimumLength = 6)]
        public string Password { get; set; }

        [DisplayName("確認密碼")]
        [Required(ErrorMessage ="請輸入確認密碼")]
        [Compare("Password",ErrorMessage ="兩次密碼不相符")]
        public string PasswordCheck { get; set; }
    }
}