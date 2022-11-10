using DACN.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DACN.Controllers
{
    public class AccountController : Controller
    {
     
        DAChuyenNganhDataContext db = new DAChuyenNganhDataContext();
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
            var user = db.NHAN_VIENs.SingleOrDefault(p => p.TaiKhoanNV == tendangnhap);
            NHAN_VIEN sp = db.NHAN_VIENs.Where(n => n.TaiKhoanNV == tendangnhap && n.MaCV == 4).SingleOrDefault();

            if (String.IsNullOrEmpty(tendangnhap) || String.IsNullOrEmpty(matkhau))
            {
                ViewData["1"] = "Vui lòng điền đầy đủ nội dung";
                return this.LogIn();
            }
            else if (sp != null)
            {
                ViewData["1"] = "Tài khoản đã bị xóa";
                return this.LogIn();
            }
            else if (!String.Equals(MD5Hash(matkhau), user.MatKhau))
            {
                ViewData["2"] = "Sai mật khẩu";
                return this.LogIn();
            }
            else if (tendangnhap == null)
            {
                ViewData["2"] = "taif ";
                return this.LogIn();
            }
            else
            {
                Session["admin"] = user;
                Session["nv"] = user.TaiKhoanNV;
                if (user.MaCV == 1)
                {
                    Session["admin1"] = null;
                    Session["sale"] = user.HoTenNV;
                    Session["quanly"] = user.HoTenNV;
                }
                else if(user.MaCV == 2)
                {
                    Session["admin1"] = user.HoTenNV;
                    Session["sale"] = null;
                    Session["quanly"] = user.HoTenNV;
                }
                else 
                {
                    Session["admin1"] = user.HoTenNV;
                    Session["sale"] = user.HoTenNV;
                    Session["quanly"] = null;
                }
                Session["ten"] = user.HoTenNV.ToString();
                //Session["l"] = user.MaCV;
                return RedirectToAction("Statistical", "Statistical");
            }
        }
        public ActionResult id()
        {
            return PartialView();
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
            NHAN_VIEN ac = (NHAN_VIEN)Session["admin"];

            var admin = db.NHAN_VIENs.SingleOrDefault(p => p.TaiKhoanNV == ac.TaiKhoanNV);
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
                if (!String.Equals(MD5Hash(po), admin.MatKhau))
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
                    admin.MatKhau = MD5Hash(pn);
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