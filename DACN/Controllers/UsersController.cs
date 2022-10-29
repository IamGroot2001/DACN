using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DACN.Models;

namespace DACN.Controllers
{
    public class UsersController : Controller
    {
        DAChuyenNganhDataContext data = new DAChuyenNganhDataContext ();
        private static readonly int CHECK_EMAIL = 1;
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
        // GET: Users
        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(FormCollection collection)
        {
            var tendangnhap = collection["username"];
            var matkhau = collection["password"];
            var user = data.KHACH_HANGs.SingleOrDefault(p => p.TaiKhoanKH == tendangnhap);
           //var a= MD5Hash(matkhau).ToString();
            if (String.IsNullOrEmpty(tendangnhap) || String.IsNullOrEmpty(matkhau))
            {
                ViewData["Error"] = "Vui lòng điền đầy đủ nội dung";
                return this.SignIn();
            }
            else if (user == null)
            {
                ViewData["Error"] = "Sai tài khoản";
                return this.SignIn();
            }
            else if (!String.Equals(MD5Hash(matkhau), user.MatKhauKH))
            {
                ViewData["Error"] = "Sai mật khẩu";
                return this.SignIn();
            }
            else
            {
                Session["user"] = user;
                Session["name"] = user.HoTenKH;
               // Session["name"] = user.HoTenKH;
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult _User()
        {
            return PartialView();
        }
        public ActionResult LogOut()
        {
            Session["user"] = null;
            Session["name"] = null;
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(FormCollection collection, KHACH_HANG kh)
        {
            var taikhoan = collection["taikhoan"];
            var hoten = collection["name"];
            var email = collection["email"];
            var sodienthoai = collection["phone"];
            var address = collection["address"];
            var matkhau = collection["pass"];
            var matkhaunhaplai = collection["re_pass"];
            if (String.IsNullOrEmpty(hoten) || String.IsNullOrEmpty(email)
                || String.IsNullOrEmpty(matkhau) || String.IsNullOrEmpty(matkhaunhaplai)
                || String.IsNullOrEmpty(address) || String.IsNullOrEmpty(sodienthoai) 
                || String.IsNullOrEmpty(taikhoan))
            {
                ViewData["Error"] = "Vui lòng điền đầy đủ nội dung";
                return this.SignUp();
            }
            else if (checkUser(taikhoan, CHECK_EMAIL))
            {
                ViewData["Error"] = "Tài khoản đã tồn tại";
                return this.SignUp();
            }
            else if (sodienthoai.ToString().Length != 10)
            {
                ViewData["Error"] = "Số điện thoại phải 10 số";
                return this.SignUp();
            }
            else if (matkhau.ToString().Length >= 24 || matkhau.ToString().Length <= 5)
            {
                ViewData["Error"] = "Độ dài mật khẩu nhiều hơn 5 và ít hơn 24";
                return this.SignUp();
            }
            else if (!String.Equals(matkhau.ToString(), matkhaunhaplai.ToString()))
            {
                ViewData["Error"] = "Mật khẩu không khớp";
                return this.SignUp();
            }
            else
            {
                kh.TaiKhoanKH = taikhoan;
                kh.HoTenKH = hoten;
                kh.EmailKH = email;
                kh.SdtKH = sodienthoai;
                kh.DiaChiKH = address;
                kh.MatKhauKH = MD5Hash(matkhau);
                data.KHACH_HANGs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("SignIn", "Users");
            }
        }
        private bool checkUser(string str, int value)
        {
            if (value == 1)
            {
                var a = data.KHACH_HANGs.FirstOrDefault(p => p.TaiKhoanKH == str);
                if (a != null) return true;
            }
            return false;
        }
        [HttpGet]
        public ActionResult AccountInformation()
        {
            KHACH_HANG ac = (KHACH_HANG)Session["user"];
            return View(ac);
        }
        [HttpPost]
        public ActionResult AccountInformation(FormCollection collection)
        {
            KHACH_HANG ac = (KHACH_HANG)Session["user"];
            var User = data.KHACH_HANGs.SingleOrDefault(p => p.TaiKhoanKH == ac.TaiKhoanKH);
            var fullnameuser = collection["name"];
            var emailuser = collection["email"];
            var phoneuser = collection["phone"];
            var addressuser = collection["address"];
            var oldpassuser = collection["oldpass"];
            var newpassuser = collection["newpass"];
            var renewpassuser = collection["renewpass"];
            if (String.IsNullOrEmpty(fullnameuser) || String.IsNullOrEmpty(emailuser) || String.IsNullOrEmpty(phoneuser) ||
                String.IsNullOrEmpty(addressuser))
            {
                ViewBag.Error = "Thông tin không được để trống";
                return this.AccountInformation();
            }
            else if (String.IsNullOrEmpty(renewpassuser) && String.IsNullOrEmpty(newpassuser) && String.IsNullOrEmpty(oldpassuser) && !String.IsNullOrEmpty(fullnameuser) && !String.IsNullOrEmpty(emailuser) && !String.IsNullOrEmpty(phoneuser) &&
                !String.IsNullOrEmpty(addressuser))
            {
                User.HoTenKH = fullnameuser;
                User.EmailKH = emailuser;
                User.SdtKH = phoneuser;
                User.DiaChiKH = addressuser;
                Session["user"] = User;
                data.SubmitChanges();
                ViewData["Info"] = "Cập nhật thành công!";
                return this.AccountInformation();
            }
            else if (String.IsNullOrEmpty(renewpassuser) && String.IsNullOrEmpty(newpassuser) && !String.IsNullOrEmpty(oldpassuser) && !String.IsNullOrEmpty(fullnameuser) && !String.IsNullOrEmpty(emailuser) && !String.IsNullOrEmpty(phoneuser) &&
                !String.IsNullOrEmpty(addressuser))
            {
                ViewBag.Error = "Vui lòng nhập mật khẩu mới!";
                return this.AccountInformation();
            }
            else if (String.IsNullOrEmpty(renewpassuser) && !String.IsNullOrEmpty(newpassuser) && !String.IsNullOrEmpty(oldpassuser) && !String.IsNullOrEmpty(fullnameuser) && !String.IsNullOrEmpty(emailuser) && !String.IsNullOrEmpty(phoneuser) &&
                !String.IsNullOrEmpty(addressuser))
            {
                ViewBag.Error = "Vui lòng nhập lại mật khẩu mới";
                return this.AccountInformation();
            }
            else if (!String.IsNullOrEmpty(renewpassuser) && !String.IsNullOrEmpty(newpassuser) && !String.IsNullOrEmpty(oldpassuser) && !String.IsNullOrEmpty(fullnameuser) && !String.IsNullOrEmpty(emailuser) && !String.IsNullOrEmpty(phoneuser) &&
                !String.IsNullOrEmpty(addressuser))
            {
                if (!String.Equals(MD5Hash(oldpassuser), User.MatKhauKH))
                {
                    ViewBag.Error = "Mật khẩu không đúng!";
                    return this.AccountInformation();
                }
                else if (!String.Equals(newpassuser, renewpassuser))
                {
                    ViewBag.Error = "Mật khẩu mới và mật khẩu cũ không trùng khớp!";
                    return this.AccountInformation();
                }
                else
                {
                    User.HoTenKH = fullnameuser;
                    User.EmailKH = emailuser;
                    User.SdtKH = phoneuser;
                    User.DiaChiKH = addressuser;
                    User.MatKhauKH = MD5Hash(newpassuser);
                    Session["user"] = User;
                    data.SubmitChanges();
                    ViewData["Info"] = "Cập nhật thành công!";
                    return this.AccountInformation();
                }
            }
            return this.AccountInformation();
        }
    }
}