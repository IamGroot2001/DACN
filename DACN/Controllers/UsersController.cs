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
using Facebook;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace DACN.Controllers
{
    public class UsersController : Controller
    {
        DAChuyenNganhDataContext data = new DAChuyenNganhDataContext ();
        private static readonly int CHECK_EMAIL = 1;
        public const string clientId = "387158749082-18vn0u13tibgpo919n6ghv80act738kd.apps.googleusercontent.com";
        public const string clientSecret = "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";
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
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
        //Gửi Mail
        public static void SendEmail(string address, string subject, string message)
        {
            if (new EmailAddressAttribute().IsValid(address)) // check có đúng mail khách hàng
            {
                string email = "buivanty15@gmail.com";
                var senderEmail = new MailAddress(email, "VAT Clother (tin nhắn tự động)");
                var receiverEmail = new MailAddress(address, "Receiver");
                var password = "dpukaghhwhgrokpo";
                var sub = subject;
                var body = message;
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)
                };
                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = sub,
                    Body = body
                })
                {
                    smtp.Send(mess);
                }
            }
        }
        private string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
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
                //sentmail
                string subject = "VAT Clother";
                string mess = "Chào mừng " + kh.HoTenKH + " đến với The VAT Clother";
                SendEmail(kh.EmailKH, subject, mess);
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

        public ActionResult SignInFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppID"],
                client_secret = ConfigurationManager.AppSettings["FbSecretID"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",

            });
            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppID"],
                client_secret = ConfigurationManager.AppSettings["FbSecretID"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code"
            });

            var accessToken = result.access_token;
            Session["AccessToken"] = accessToken;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                //Get the user's information, like email, first name, middle name etc
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string userName = me.email;
                string firstName = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;
                FormsAuthentication.SetAuthCookie(email, false);
                //neu chay dc hãy thử thêm sđt và địa chỉ

                var khachhang = new KHACH_HANG();
                khachhang.EmailKH = email;
                khachhang.TaiKhoanKH = email;
                khachhang.HoTenKH = firstName + " " + middlename + " " + lastname;
                data.KHACH_HANGs.InsertOnSubmit(khachhang);
                data.SubmitChanges();



            }
            return Redirect("/");
        }

        [HttpGet]
        public ActionResult Forget()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Forget(FormCollection form)
        {

            string email = form["Email"].ToString();
            if (String.IsNullOrEmpty(email))
            {
                ViewBag.MatKhau = "! Email Không Được Để Trống";
            }
            else
            {
                KHACH_HANG nv = data.KHACH_HANGs.FirstOrDefault(p => p.EmailKH == email);
                if (nv != null)
                {
                    string host = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, "");
                    string thoiHan = DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss");
                    string maRS = MD5Hash(thoiHan);
                    ResetPass reset = new ResetPass();
                    reset.maRS = maRS;
                    reset.TaiKhoanKH_KHACH_HANG = nv.TaiKhoanKH;
                    reset.ThoiHan = thoiHan;
                    data.ResetPasses.InsertOnSubmit(reset);
                    data.SubmitChanges();
                    SendEmail(email, "Khôi Phục Mật Khẩu", "Link Khôi Phục Mật Khẩu Của Bạn Là: " + host + "/Users/ResetPass?token=" + maRS);
                    return RedirectToAction("NotifForm", "Users", new { title = "Yêu Cầu Thành Công", msg = "Chúng tôi đã gửi link khôi phục về Email: " + email + " của bạn." });
                }
                else
                {
                    ViewBag.ThongBao = "Tài Khoản hoặc Email không hợp lệ!!";
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult NotifForm(string title, string msg)
        {
            ViewBag.TitleH = title;
            ViewBag.Msg = msg;
            return View();
        }

        [HttpGet]
        public ActionResult ResetPass(string token)
        {
            ResetPass reset = data.ResetPasses.FirstOrDefault(p => p.maRS == token);
            if (reset == null)
                return RedirectToAction("NotifForm", "Users", new { title = "Link Hết Hạn", msg = "Chúng tôi nhận thấy link của bạn đã hết hạn. Vui lòng tạo link quên mật khẩu mới!" });
            long ThoiHan = long.Parse(reset.ThoiHan);
            long Now = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
            if (Now >= ThoiHan)
                return RedirectToAction("NotifForm", "Users", new { title = "Link Hết Hạn", msg = "Chúng tôi nhận thấy link của bạn đã hết hạn. Vui lòng tạo link quên mật khẩu mới!" });
            if (ThoiHan == 1)
                return RedirectToAction("NotifForm", "Users", new { title = "Link Không Hợp Lệ", msg = "Chúng tôi nhận thấy link của bạn không hợp lệ. Vui lòng tạo link quên mật khẩu mới!" });

            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ResetPass(FormCollection form)
        {
            Console.Write(form);
            string maRS = form["token"];
            string Pass = form["password"];
            string rePass = form["repassword"];
            if (String.IsNullOrEmpty(maRS))
            {
                return RedirectToAction("NotifForm", "Users", new { title = "Lỗi Link", msg = "Chúng tôi đã phát hiện lỗi." });
            }
            else if (String.IsNullOrEmpty(Pass))
            {
                ViewBag.Pass = "! Không Được Để Mật Khẩu Trống";
            }
            else if (String.IsNullOrEmpty(rePass))
            {
                ViewBag.RePass = "! Không Được Để Mật Khẩu Trống";
            }
            else if (Pass != rePass)
            {
                ViewBag.RePass = "! Mật Khẩu Không Khớp";
            }
            else
            {
                ResetPass rs = data.ResetPasses.FirstOrDefault(p => p.maRS == maRS);
                if (rs != null)
                {
                    string email = rs.KHACH_HANG.EmailKH;
                    rs.KHACH_HANG.MatKhauKH = MD5Hash(Pass);
                    List<ResetPass> ListRS = data.ResetPasses.Where(p => p.TaiKhoanKH_KHACH_HANG == rs.TaiKhoanKH_KHACH_HANG).ToList();
                    foreach (var item in ListRS)
                    {
                        if (item.ThoiHan != "1")
                            data.ResetPasses.DeleteOnSubmit(item);
                    }
                    data.SubmitChanges();
                    SendEmail(email, "Đổi Mật Khẩu Thành Công", "Mật Khẩu của " + email + " đã được đổi thành công lúc " + DateTime.Now.ToString("dd/MM/yyyy - HH:mm"));
                    return RedirectToAction("NotifForm", "Users", new { title = "Yêu Cầu Thành Công", msg = "Mất Khẩu của bạn đã được đổi thành công!" });
                }
                else
                {
                    return RedirectToAction("NotifForm", "Users", new { title = "Link Hết Hạn", msg = "Chúng tôi nhận thấy link của bạn đã hết hạn. Vui lòng tạo link quên mật khẩu mới!" });
                }
            }
            return View();
        }
    }
}