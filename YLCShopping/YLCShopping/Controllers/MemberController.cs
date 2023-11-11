using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using YLCShopping.Models;

namespace YLCShopping.Controllers
{
    public class MemberController : BaseController
    {
        // GET: Member
        // 会员注册页面
        public ActionResult Register()
        {
            return View();
        }

        // 密码哈希所需的 Salt 乱数值
        private string pwSalt = "A1rySq1oPe2Mh784QQwG6jRAfkdPpDa90J0i";

        // 写入会员资料
        [HttpPost]
        public ActionResult Register([Bind(Exclude = "RegisterOn,AuthCode")] Member member)
        {
            // 检查会员是否已存在
            var chk_member = db.Members.Where(p => p.Email == member.Email).FirstOrDefault();
            if (chk_member != null)
            {
               ModelState.AddModelError("Email", "您输入的 Email 已经有人注册过了!");
            }

            if (ModelState.IsValid)
            {
                // 将密码哈希(Salt)之后进行杂凑运算以提升会员密码的安全性
                member.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(pwSalt + member.Password, "SHA1");
                // 会员注册时间
                member.RegisterOn = DateTime.Now;
                // 会员验证码，采用 Guid 当成验证码内容，避免有会员使用到重覆的验证码
                member.AuthCode = Guid.NewGuid().ToString();

                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }
        }

        public ActionResult ValidateRegister(string id)
        {
            if (String.IsNullOrEmpty(id))
                return HttpNotFound();

            var member = db.Members.Where(p => p.AuthCode == id).FirstOrDefault();

            if (member != null)
            {
                TempData["LastTempMessage"] = "会员验证成功，您现在可以登入网站了!";
                // 验证成功后要将 member.AuthCode 的内容清空
                member.AuthCode = null;
                db.SaveChanges();
            }
            else
            {
                TempData["LastTempMessage"] = "查无此会员验证码，您可能已经验证过了!";
            }

            return RedirectToAction("Login", "Member");
        }

        /// <summary>
        /// 用户修改密码
        /// </summary>
      
        // 显示会员登入页面
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        // 执行会员登入
        [HttpPost]
        public ActionResult Login(string email, string password, string returnUrl)
        {
            if (ValidateUser(email, password))
            {
                FormsAuthentication.SetAuthCookie(email, false);

                if (String.IsNullOrEmpty(returnUrl))
                {

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(returnUrl);
                }
            }

            return View();
        }

        private bool ValidateUser(string email, string password)
        {
            var hash_pw = FormsAuthentication.HashPasswordForStoringInConfigFile(pwSalt + password, "SHA1");

            var member = (from p in db.Members
                          where p.Email == email && p.Password == hash_pw
                          select p).FirstOrDefault();

            // 如果 member 物件不为 null 则代表会员的帐号、密码输入正确
            if (member != null)
            {
                if (member.AuthCode == null)
                {
                    var id = member.Id;
                    Session["memberid"] = id;
                    Session["name"] = member.Nickname;
                    return true;
                }
                else
                {
                    ModelState.AddModelError("", "您尚未通过会员验证，请在邮箱点击会员验证链接!");
                    return false;
                }
            }
            else
            {
                ModelState.AddModelError("", "您输入的帐号或密码错误");
                return false;
            }
        }

        // 执行会员登出
        public ActionResult Logout()
        {
            // 清除表单验证的 Cookies
            FormsAuthentication.SignOut();

            // 清除所有曾经写入过的 Session 资料
            Session.Clear();

            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult CheckDup(string Email)
        {
            var member = db.Members.Where(p => p.Email == Email).FirstOrDefault();

            if (member != null)
                return Json(false);
            else
                return Json(true);
        }

        //商品图片
        public FileContentResult GetImage(int Id)
        {
            Member prod = db.Members
                .FirstOrDefault(p => p.Id == Id);
            if (prod.ImageData != null)
            {
                return File(prod.ImageData, prod.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}