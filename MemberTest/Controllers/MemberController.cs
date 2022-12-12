using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MemberTest.Services;
using MemberTest.ViewModels;
using System.Web.Security;

namespace MemberTest.Controllers
{
    public class MemberController : Controller
    {
        private readonly MemberDBService memberDBService=new MemberDBService();


        // GET: Member
        public ActionResult Index()
        {
            return View();
        }

        #region 註冊
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(MemberRegisterViewModel RegisterMember)
        {
            if (ModelState.IsValid)
            {
                RegisterMember.newmember.Password=RegisterMember.Password;
                memberDBService.Register(RegisterMember.newmember);
                return RedirectToAction("RegisterResult");
            }            
            return View();
        }

        public JsonResult AccountCheck(MemberRegisterViewModel RegisterMember)
        {
            return Json(memberDBService.AccountCheck(RegisterMember.newmember.Account),
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult RegisterResult()
        {
            return View();
        }

        #endregion

        #region 登入
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Login(MemberLoginViewModel MemberLogin)
        {
            string ValidateStr = memberDBService.LoginCheck(MemberLogin.Account, MemberLogin.Password);
            if (ModelState.IsValid)
            {
                if (ValidateStr != "密碼錯誤" && ValidateStr != "無此帳號")
                {
                    Session["Name"] = ValidateStr;
                    Session["isvalid"] = 1;
                    return RedirectToAction("LoginResult", new { Username = ValidateStr });
                }
                else
                {
                    if (ValidateStr != "密碼錯誤" && ValidateStr != "無此帳號")
                        ViewBag.e = "";
                    else
                        ViewBag.e = ValidateStr;
                    //ModelState.AddModelError("", ValidateStr);
                    MemberLogin.Password = null;
                    return View(MemberLogin);
                }
            }
            return View();
                        
        }
        public ActionResult LoginResult(string Username)
        {
            if (Session["Name"] == null || string.IsNullOrWhiteSpace(Session["Name"].ToString()))
            {
                return RedirectToAction("Login");
            }
            ViewBag.m = "歡迎，" + Username + "。";
            return View();
        }
        #endregion

        #region 登出
        public ActionResult Logout()
        {
            Session["isvalid"] = null;
            return RedirectToAction("Login", "Member");
        }
        #endregion
    }
}