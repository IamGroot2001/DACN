using DACN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DACN.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {

        dbDAChuyenNganhDataContext db = new dbDAChuyenNganhDataContext();
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }
        // GET: Admin/Account
        [HttpPost]
        public ActionResult LogIn(FormCollection collection)
        {
            var tendangnhap = collection["username"];
            var matkhau = collection["password"];
            var user = db.AdminAccounts.SingleOrDefault(p => p.UserNameAdmin == tendangnhap);
            if (String.IsNullOrEmpty(tendangnhap) || String.IsNullOrEmpty(matkhau))
            {
                ViewData["1"] = "Vui lòng điền đầy đủ nội dung";
                return this.LogIn();
            }
            else if (user == null)
            {
                ViewData["1"] = "Sai tài khoản";
                return this.LogIn();
            }
            else if (!String.Equals(MD5Hash(matkhau), user.PasswordAdmin))
            {
                ViewData["2"] = "Sai mật khẩu";
                return this.LogIn();
            }
            else
            {
                Session["admin"] = user;
                return RedirectToAction("Statistical", "Statistical");
            }
        }
        public ActionResult LogOut()
        {
            Session["admin"] = null;
            return RedirectToAction("LogIn", "Account");
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(FormCollection collection)
        {
            AdminAccount ac = (AdminAccount)Session["admin"];

            var admin = db.AdminAccounts.SingleOrDefault(p => p.IdAdminAccount == ac.IdAdminAccount);
            var po = collection["passold"];
            var pn = collection["passnew"];
            var pa = collection["passagain"];
            if (String.IsNullOrEmpty(po) || String.IsNullOrEmpty(pn) || String.IsNullOrEmpty(pa))
            {
                ViewData["1"] = "Thông tin không được để trống";
            }
            else if (!String.IsNullOrEmpty(po) && String.IsNullOrEmpty(pn) && !String.IsNullOrEmpty(pa))
            {
                ViewData["3"] = "Vui lòng nhập mật khẩu mới!";
                return this.ChangePassword();
            }
            else if (!String.IsNullOrEmpty(po) && !String.IsNullOrEmpty(pn) && !String.IsNullOrEmpty(pa))
            {
                if (!String.Equals(MD5Hash(po), admin.PasswordAdmin))
                {
                    ViewData["1"] = "Mật khẩu không đúng!";
                    return this.ChangePassword();
                }
                else if (!String.Equals(pn, pa))
                {
                    ViewData["3"] = "Mật khẩu nhập lại không trùng khớp!";
                    return this.ChangePassword();
                }
                else
                {
                    admin.PasswordAdmin = MD5Hash(pn);
                    Session["admin"] = admin;
                    db.SubmitChanges();
                    ViewData["3"] = "Cập nhật thành công!";
                    return RedirectToAction("Statistical", "Statistical");
                }
            }
            return this.ChangePassword();
        }
    }
}