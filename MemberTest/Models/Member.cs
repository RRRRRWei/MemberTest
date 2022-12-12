using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MemberTest.Models
{
    public class Member
    {
        public int Id { get; set; }

        [DisplayName("帳號")]
        [Required(ErrorMessage ="請輸入帳號")]
        [StringLength(30,ErrorMessage ="帳號長度介於 6 ~ 30 之間",MinimumLength =6)]
        [Remote("AccountCheck","Member",ErrorMessage ="此帳號已註冊")]
        public string Account { get; set; }

        
        public string Password { get; set; }

        [DisplayName("姓名")]
        [Required(ErrorMessage = "請輸入姓名")]
        [StringLength(20, ErrorMessage = "姓名長度介於 2 ~ 30 之間", MinimumLength = 2)]
        public string Username { get; set; }

    }
}