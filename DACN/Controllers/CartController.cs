using DACN.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using DACN.Asset.csharp;
using DACN.VNPAY;

namespace DACN.Controllers
{
    public class CartController : Controller
    {
        DAChuyenNganhDataContext data = new DAChuyenNganhDataContext();
        // GET: Cart
        public List<GioHang> LayGioHang()
        {
            List<GioHang> listgiohang = Session["Giohang"] as List<GioHang>;
            if (listgiohang == null)
            {
                //Nếu giỏ hàng chưa tồn tại thì khởi tạo listGioHang
                listgiohang = new List<GioHang>();
                Session["Giohang"] = listgiohang;
            }
            return listgiohang;
        }
        //Tổng số lượng
        private int TongSoLuong()
        {
            int tong = 0;
            List<GioHang> listgiohang = Session["Giohang"] as List<GioHang>;
            if (listgiohang != null)
            {
                tong = (int)listgiohang.Sum(n => n.iQuantityProduct);
            }
            return tong;
        }
        //Tính tổng tiền
        private int TongTien()
        {
            int tongtien = 0;
            List<GioHang> listgiohang = Session["Giohang"] as List<GioHang>;
            if (listgiohang != null)
            {
                tongtien = listgiohang.Sum(n => n.iThanhTien);
            }
            return tongtien;
        }
        //Cập nhật số lượng tồn của mỗi sản phẩm
        private void updateSoLuong(CT_DONHANG cthd)
        {
            var sp = data.CT_SANPHAMs.Single(p => p.MaSP == cthd.MaSP && p.MaSize == cthd.MaSize);
            sp.SoLuong = sp.SoLuong - cthd.SoLuong;
            data.SubmitChanges();
        }
        //Thêm sản phẩm vào giỏ hàng
        [HttpPost]
        public ActionResult ThemGioHang(int? idProduct, string strURL)
        {
            int? sizeid = null;
            //Lấy ra session    
            List<GioHang> listgiohang = LayGioHang();
            Session["Size"] = Request.Form["nameSize"];
            if (Request.Form["nameSize"] == null)
            {
                Session["Error"] = "Vui lòng chọn Size sản phẩm!";
                return Redirect(strURL);
            }
            else sizeid = Int32.Parse(Request.Form["nameSize"].ToString());
            int sl = Int32.Parse(Request.Form["quantity"].ToString());
            int soLuongTon = data.CT_SANPHAMs.SingleOrDefault(p => p.MaSP == idProduct && p.MaSize == sizeid).SoLuong.Value;
            if (sl > soLuongTon)
            {
                Session["sl"] = 1;
                Session["Error1"] = "Số lượng mua lớn hơn số lượng tồn! Vui lòng chọn lại!";
                return Redirect(strURL);
            }
            else
            {
                Session["sl"] = null;
            }
            //Kiểm tra sản phẩm này tồn tại trong Session["Giohang"] chưa?
            GioHang giohang = listgiohang.Find(n => n.iIdProduct == idProduct && n.iSize == sizeid);
            if (giohang == null)
            {

                giohang = new GioHang(idProduct, sizeid, sl);
                listgiohang.Add(giohang);
                //SetAlert("Thêm vào giỏ hàng thành công", "success");
                return Redirect(strURL);

            }
            else
            {
                giohang.iQuantityProduct++;
                //SetAlert("Thêm vào giỏ hàng thành công", "success");
                return Redirect(strURL);
            }
        }
        [HttpGet]
        public ActionResult Cart()
        {
            List<GioHang> listgiohang = LayGioHang();
            if (listgiohang.Count == 0)
            {
                return RedirectToAction("Shop", "Home");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.TongTienShip = TongTien() + 25000;
            return View(listgiohang);
        }
        //Xóa 1 món hàng ra khỏi giỏ hàng
        public ActionResult RemoveItemInCart(int iProductId, int iSizeId)
        {
            List<GioHang> listProductInCart = LayGioHang();
            GioHang sp = listProductInCart.SingleOrDefault(n => n.iIdProduct == iProductId && n.iSize == iSizeId);
            if (sp != null)
            {
                listProductInCart.Remove(sp);
                return RedirectToAction("Cart");
            }
            if (listProductInCart.Count == 0)
            {
                return RedirectToAction("ProductPage", "Product");
            }
            return RedirectToAction("Cart");
        }
        //Cập nhật lại số lượng trong giỏ hàng
        public ActionResult UpdateItemInCart(int iProductId, int iSizeId, FormCollection collection)
        {
            List<GioHang> listProductInCart = LayGioHang();
            GioHang sp = listProductInCart.SingleOrDefault(n => n.iIdProduct == iProductId && n.iSize == iSizeId);
            if (sp != null)
            {
                sp.iQuantityProduct = int.Parse(collection["quantity1"]);
            }
            return RedirectToAction("Cart");
        }
        //Xóa toàn bộ giỏ hàng
        public ActionResult RemoveCart()
        {
            List<GioHang> listProductInCart = LayGioHang();
            listProductInCart.Clear();
            return RedirectToAction("Cart", "Cart");
        }

        [HttpGet]
        public ActionResult Checkout()
        {
            KHACH_HANG ac = (KHACH_HANG)Session["user"];
            if (ac != null)
            {
                Session["name"] = ac.HoTenKH;
                Session["phone"] = ac.SdtKH;
                Session["address"] = ac.DiaChiKH;
            }

            List<GioHang> listgiohang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.TongTienShip = TongTien();
            return View(listgiohang);
        }
        [HttpPost]
        public ActionResult Checkout(string strURL, FormCollection collection)
        {
            Session["billing_name"] = null;
            Session["billing_phone"] = null;
            Session["billing_address"] = null;
            Session["billing_note"] = null;

            Session["billing_name"] = collection["billing_name"];
            Session["billing_phone"] = collection["billing_phone"];
            Session["billing_address"] = collection["billing_address"];
            Session["billing_note"] = collection["billing_note"];
            string httt = collection["Payment"];
            if (httt == "Thanh toán khi nhận hàng")
            {
                KHACH_HANG ac = (KHACH_HANG)Session["user"];
                DON_HANG ddh = new DON_HANG();
                List<GioHang> gh = LayGioHang();
                //List<InfoCustomerBill> info = null;

                if (Session["user"] != null)
                {
                    ddh.TaiKhoanKH = ac.TaiKhoanKH;
                    ddh.TenNguoiNhan = collection["billing_name"];
                    ddh.SdtNguoiNhan = collection["billing_phone"];
                    ddh.DiaChiNhanHang = collection["billing_address"];
                    ddh.GhiChu = collection["billing_note"];
                    ddh.NgayLap = DateTime.Now;
                    ddh.TongTien = TongTien();
                    ddh.MaPTTT = 1;
                    ddh.TrangThaiDonHang = false;
                    ddh.TrangThaiGiaoHang = false;
                    data.DON_HANGs.InsertOnSubmit(ddh);
                    data.SubmitChanges();
                    Session["idInvoice"] = ddh.MaDH;
                    foreach (var item in gh)
                    {
                        CT_DONHANG ctdh = new CT_DONHANG();
                        ctdh.MaSize = (int)item.iSize;
                        ctdh.MaSP = (int)item.iIdProduct;
                        ctdh.MaDH = ddh.MaDH;
                        ctdh.SoLuong = item.iQuantityProduct;
                        ctdh.ThanhTien = item.iPriceProduct;
                        int soLuongTon = data.CT_SANPHAMs.SingleOrDefault(p => p.MaSP == item.iIdProduct && p.MaSize == item.iSize).SoLuong.Value;
                        if (soLuongTon < ctdh.SoLuong)
                        {
                            ViewBag.SoLuongTon = "Sản phẩm hết hàng hoặc quá số lượng tồn, sản phẩm hết hàng sẽ được xóa khỏi gio hàng!";
                            List<GioHang> listProductInCart = LayGioHang();
                            GioHang sp = listProductInCart.SingleOrDefault(n => n.iIdProduct == item.iIdProduct && n.iSize == item.iSize);
                            listProductInCart.Remove(sp);
                            return this.Checkout();
                        }
                        updateSoLuong(ctdh);
                        data.CT_DONHANGs.InsertOnSubmit(ctdh);
                        data.SubmitChanges();
                        Session["Giohang"] = null;
                    }
                }
                else
                {
                    var ten = collection["ten"];
                    var diachi = collection["diachi"];
                    var sdt = collection["sdt"];
                    var ghichu = collection["ghichu"];
                    String a = "khachhangvanglai";
                    ddh.TaiKhoanKH = a;
                    ddh.TenNguoiNhan = ten;
                    ddh.SdtNguoiNhan = diachi;
                    ddh.DiaChiNhanHang = sdt;
                    ddh.GhiChu = ghichu;
                    ddh.NgayLap = DateTime.Now;
                    ddh.TongTien = TongTien();
                    ddh.MaPTTT = 1;
                    ddh.TrangThaiDonHang = false;
                    ddh.TrangThaiGiaoHang = false;
                    data.DON_HANGs.InsertOnSubmit(ddh);
                    data.SubmitChanges();
                    Session["idInvoice"] = ddh.MaDH;
                    foreach (var item in gh)
                    {
                        CT_DONHANG ctdh = new CT_DONHANG();
                        ctdh.MaSize = (int)item.iSize;
                        ctdh.MaSP = (int)item.iIdProduct;
                        ctdh.MaDH = ddh.MaDH;
                        ctdh.SoLuong = item.iQuantityProduct;
                        ctdh.ThanhTien = item.iPriceProduct;
                        int soLuongTon = data.CT_SANPHAMs.SingleOrDefault(p => p.MaSP == item.iIdProduct && p.MaSize == item.iSize).SoLuong.Value;
                        if (soLuongTon < ctdh.SoLuong)
                        {
                            ViewBag.SoLuongTon = "Sản phẩm hết hàng hoặc quá số lượng tồn, sản phẩm hết hàng sẽ được xóa khỏi gio hàng!";
                            List<GioHang> listProductInCart = LayGioHang();
                            GioHang sp = listProductInCart.SingleOrDefault(n => n.iIdProduct == item.iIdProduct && n.iSize == item.iSize);
                            listProductInCart.Remove(sp);
                            return this.Checkout();
                        }
                        updateSoLuong(ctdh);
                        data.CT_DONHANGs.InsertOnSubmit(ctdh);
                        data.SubmitChanges();
                        Session["Giohang"] = null;
                    }
                }
                return RedirectToAction("Thanks", "Cart");
            }
            else if (httt == "Momo")
            {
                List<GioHang> gh = Session["GioHang"] as List<GioHang>;
                //request params need to request to MoMo system
                string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
                string partnerCode = "MOMO_ATM_DEV";
                string accessKey = "w9gEg8bjA2AM2Cvr";
                string serectkey = "mD9QAVi4cm9N844jh5Y2tqjWaaJoGVFM";
                string orderInfo = "Thanh toán qua Ví MoMo";
                string returnUrl = "https://localhost:44359/Cart/ConfirmPaymentClient";
                string notifyurl = "https://53d0-123-21-167-207.ap.ngrok.io/GioHang/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

                string amount = gh.Sum(n => n.iThanhTien).ToString();//sửa lại giá để lấi giá đơn hàng
                string orderid = DateTime.Now.Ticks.ToString();
                string requestId = DateTime.Now.Ticks.ToString();
                string extraData = "";

                //Before sign HMAC SHA256 signature
                string rawHash = "partnerCode=" +
                    partnerCode + "&accessKey=" +
                    accessKey + "&requestId=" +
                    requestId + "&amount=" +
                    amount + "&orderId=" +
                    orderid + "&orderInfo=" +
                    orderInfo + "&returnUrl=" +
                    returnUrl + "&notifyUrl=" +
                    notifyurl + "&extraData=" +
                    extraData;

                MoMoSecurity crypto = new MoMoSecurity();
                //sign signature SHA256
                string signature = crypto.signSHA256(rawHash, serectkey);

                //build body json request
                JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };

                string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
                JObject jmessage = JObject.Parse(responseFromMomo);
                //luu database
               
               
                    KHACH_HANG ac = (KHACH_HANG)Session["user"];
                    DON_HANG ddh = new DON_HANG();
                    //List<GioHang> gh = LayGioHang();
                    //List<InfoCustomerBill> info = null;

                    if (Session["user"] != null)
                    {
                        ddh.TaiKhoanKH = ac.TaiKhoanKH;
                        ddh.TenNguoiNhan = collection["billing_name"];
                        ddh.SdtNguoiNhan = collection["billing_phone"];
                        ddh.DiaChiNhanHang = collection["billing_address"];
                        ddh.GhiChu = collection["billing_note"];
                        ddh.NgayLap = DateTime.Now;
                        ddh.TongTien = TongTien();
                        ddh.MaPTTT = 1;
                        ddh.TrangThaiDonHang = false;
                        ddh.TrangThaiGiaoHang = false;
                        data.DON_HANGs.InsertOnSubmit(ddh);
                        data.SubmitChanges();
                        Session["idInvoice"] = ddh.MaDH;
                        foreach (var item in gh)
                        {
                            CT_DONHANG ctdh = new CT_DONHANG();
                            ctdh.MaSize = (int)item.iSize;
                            ctdh.MaSP = (int)item.iIdProduct;
                            ctdh.MaDH = ddh.MaDH;
                            ctdh.SoLuong = item.iQuantityProduct;
                            ctdh.ThanhTien = item.iPriceProduct;
                            int soLuongTon = data.CT_SANPHAMs.SingleOrDefault(p => p.MaSP == item.iIdProduct && p.MaSize == item.iSize).SoLuong.Value;
                            if (soLuongTon < ctdh.SoLuong)
                            {
                                ViewBag.SoLuongTon = "Sản phẩm hết hàng hoặc quá số lượng tồn, sản phẩm hết hàng sẽ được xóa khỏi gio hàng!";
                                List<GioHang> listProductInCart = LayGioHang();
                                GioHang sp = listProductInCart.SingleOrDefault(n => n.iIdProduct == item.iIdProduct && n.iSize == item.iSize);
                                listProductInCart.Remove(sp);
                                return this.Checkout();
                            }
                            updateSoLuong(ctdh);
                            data.CT_DONHANGs.InsertOnSubmit(ctdh);
                            data.SubmitChanges();
                            Session["Giohang"] = null;
                        }
                    }
                    else
                    {
                        var ten = collection["ten"];
                        var diachi = collection["diachi"];
                        var sdt = collection["sdt"];
                        var ghichu = collection["ghichu"];
                        String a = "khachhangvanglai";
                        ddh.TaiKhoanKH = a;
                        ddh.TenNguoiNhan = ten;
                        ddh.SdtNguoiNhan = diachi;
                        ddh.DiaChiNhanHang = sdt;
                        ddh.GhiChu = ghichu;
                        ddh.NgayLap = DateTime.Now;
                        ddh.TongTien = TongTien();
                        ddh.MaPTTT = 1;
                        ddh.TrangThaiDonHang = false;
                        ddh.TrangThaiGiaoHang = false;
                        data.DON_HANGs.InsertOnSubmit(ddh);
                        data.SubmitChanges();
                        Session["idInvoice"] = ddh.MaDH;
                        foreach (var item in gh)
                        {
                            CT_DONHANG ctdh = new CT_DONHANG();
                            ctdh.MaSize = (int)item.iSize;
                            ctdh.MaSP = (int)item.iIdProduct;
                            ctdh.MaDH = ddh.MaDH;
                            ctdh.SoLuong = item.iQuantityProduct;
                            ctdh.ThanhTien = item.iPriceProduct;
                            int soLuongTon = data.CT_SANPHAMs.SingleOrDefault(p => p.MaSP == item.iIdProduct && p.MaSize == item.iSize).SoLuong.Value;
                            if (soLuongTon < ctdh.SoLuong)
                            {
                                ViewBag.SoLuongTon = "Sản phẩm hết hàng hoặc quá số lượng tồn, sản phẩm hết hàng sẽ được xóa khỏi gio hàng!";
                                List<GioHang> listProductInCart = LayGioHang();
                                GioHang sp = listProductInCart.SingleOrDefault(n => n.iIdProduct == item.iIdProduct && n.iSize == item.iSize);
                                listProductInCart.Remove(sp);
                                return this.Checkout();
                            }
                            updateSoLuong(ctdh);
                            data.CT_DONHANGs.InsertOnSubmit(ctdh);
                            data.SubmitChanges();
                            Session["Giohang"] = null;
                            return Redirect(jmessage.GetValue("payUrl").ToString());

                        }

                    }
                return RedirectToAction("ConfirmPaymentClient", "Cart");
            }
            else if(httt=="Ví VNPAY")
            {
                List<GioHang> gioHangs = Session["GioHang"] as List<GioHang>;

                
                KHACH_HANG ac = (KHACH_HANG)Session["user"];
                DON_HANG ddh = new DON_HANG();
                List<GioHang> gh = LayGioHang();
                //List<InfoCustomerBill> info = null;

                if (Session["user"] != null)
                {
                    ddh.TaiKhoanKH = ac.TaiKhoanKH;
                    ddh.TenNguoiNhan = collection["billing_name"];
                    ddh.SdtNguoiNhan = collection["billing_phone"];
                    ddh.DiaChiNhanHang = collection["billing_address"];
                    ddh.GhiChu = collection["billing_note"];
                    ddh.NgayLap = DateTime.Now;
                    ddh.TongTien = TongTien();
                    ddh.MaPTTT = 2;
                    ddh.TrangThaiDonHang = false;
                    ddh.TrangThaiGiaoHang = false;
                    data.DON_HANGs.InsertOnSubmit(ddh);
                    data.SubmitChanges();
                    Session["idInvoice"] = ddh.MaDH;
                    string url = ConfigurationManager.AppSettings["Url"];
                    string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
                    string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
                    string hashSecret = ConfigurationManager.AppSettings["HashSecret"];
                    string amount = gioHangs.Sum(n => n.iThanhTien * 100).ToString();//sửa lại giá để lấi giá đơn hàng

                    XuLy pay = new XuLy();

                    //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
                    pay.AddRequestData("vnp_Version", "2.1.0");

                    //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
                    pay.AddRequestData("vnp_Command", "pay");

                    //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
                    pay.AddRequestData("vnp_TmnCode", tmnCode);

                    //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
                    pay.AddRequestData("vnp_Amount", amount);

                    //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
                    pay.AddRequestData("vnp_BankCode", "");

                    //ngày thanh toán theo định dạng yyyyMMddHHmmss
                    pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));

                    //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
                    pay.AddRequestData("vnp_CurrCode", "VND");

                    //Địa chỉ IP của khách hàng thực hiện giao dịch
                    pay.AddRequestData("vnp_IpAddr", ChuyenDoi.GetIpAddress());

                    //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
                    pay.AddRequestData("vnp_Locale", "vn");

                    //Thông tin mô tả nội dung thanh toán
                    pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang");

                    //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
                    pay.AddRequestData("vnp_OrderType", "other");

                    //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
                    pay.AddRequestData("vnp_ReturnUrl", returnUrl);

                    //mã hóa đơn
                    pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());
                    string paymentUrl = pay.CreateRequestUrl(url, hashSecret);
                    foreach (var item in gh)
                    {
                        CT_DONHANG ctdh = new CT_DONHANG();
                        ctdh.MaSize = (int)item.iSize;
                        ctdh.MaSP = (int)item.iIdProduct;
                        ctdh.MaDH = ddh.MaDH;
                        ctdh.SoLuong = item.iQuantityProduct;
                        ctdh.ThanhTien = item.iPriceProduct;
                        int soLuongTon = data.CT_SANPHAMs.SingleOrDefault(p => p.MaSP == item.iIdProduct && p.MaSize == item.iSize).SoLuong.Value;
                        if (soLuongTon < ctdh.SoLuong)
                        {
                            ViewBag.SoLuongTon = "Sản phẩm hết hàng hoặc quá số lượng tồn, sản phẩm hết hàng sẽ được xóa khỏi gio hàng!";
                            List<GioHang> listProductInCart = LayGioHang();
                            GioHang sp = listProductInCart.SingleOrDefault(n => n.iIdProduct == item.iIdProduct && n.iSize == item.iSize);
                            listProductInCart.Remove(sp);
                            return this.Checkout();
                        }
                        updateSoLuong(ctdh);
                        data.CT_DONHANGs.InsertOnSubmit(ctdh);
                        data.SubmitChanges();
                        Session["Giohang"] = null;
                    }
                    return Redirect(paymentUrl);
                }
                else
                {
                    var ten = collection["ten"];
                    var diachi = collection["diachi"];
                    var sdt = collection["sdt"];
                    var ghichu = collection["ghichu"];
                    String a = "khachhangvanglai";
                    ddh.TaiKhoanKH = a;
                    ddh.TenNguoiNhan = ten;
                    ddh.SdtNguoiNhan = diachi;
                    ddh.DiaChiNhanHang = sdt;
                    ddh.GhiChu = ghichu;
                    ddh.NgayLap = DateTime.Now;
                    ddh.TongTien = TongTien();
                    ddh.MaPTTT = 2;
                    ddh.TrangThaiDonHang = false;
                    ddh.TrangThaiGiaoHang = false;
                    data.DON_HANGs.InsertOnSubmit(ddh);
                    data.SubmitChanges();
                    Session["idInvoice"] = ddh.MaDH;
                    string url = ConfigurationManager.AppSettings["Url"];
                    string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
                    string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
                    string hashSecret = ConfigurationManager.AppSettings["HashSecret"];
                    string amount = gioHangs.Sum(n => n.iThanhTien * 100).ToString();//sửa lại giá để lấi giá đơn hàng

                    XuLy pay = new XuLy();

                    //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
                    pay.AddRequestData("vnp_Version", "2.1.0");

                    //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
                    pay.AddRequestData("vnp_Command", "pay");

                    //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
                    pay.AddRequestData("vnp_TmnCode", tmnCode);

                    //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
                    pay.AddRequestData("vnp_Amount", amount);

                    //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
                    pay.AddRequestData("vnp_BankCode", "");

                    //ngày thanh toán theo định dạng yyyyMMddHHmmss
                    pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));

                    //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
                    pay.AddRequestData("vnp_CurrCode", "VND");

                    //Địa chỉ IP của khách hàng thực hiện giao dịch
                    pay.AddRequestData("vnp_IpAddr", ChuyenDoi.GetIpAddress());

                    //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
                    pay.AddRequestData("vnp_Locale", "vn");

                    //Thông tin mô tả nội dung thanh toán
                    pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang");

                    //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
                    pay.AddRequestData("vnp_OrderType", "other");

                    //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
                    pay.AddRequestData("vnp_ReturnUrl", returnUrl);

                    //mã hóa đơn
                    pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());
                    string paymentUrl = pay.CreateRequestUrl(url, hashSecret);
                    

                    foreach (var item in gh)
                    {
                        CT_DONHANG ctdh = new CT_DONHANG();
                        ctdh.MaSize = (int)item.iSize;
                        ctdh.MaSP = (int)item.iIdProduct;
                        ctdh.MaDH = ddh.MaDH;
                        ctdh.SoLuong = item.iQuantityProduct;
                        ctdh.ThanhTien = item.iPriceProduct;
                        int soLuongTon = data.CT_SANPHAMs.SingleOrDefault(p => p.MaSP == item.iIdProduct && p.MaSize == item.iSize).SoLuong.Value;
                        if (soLuongTon < ctdh.SoLuong)
                        {
                            ViewBag.SoLuongTon = "Sản phẩm hết hàng hoặc quá số lượng tồn, sản phẩm hết hàng sẽ được xóa khỏi gio hàng!";
                            List<GioHang> listProductInCart = LayGioHang();
                            GioHang sp = listProductInCart.SingleOrDefault(n => n.iIdProduct == item.iIdProduct && n.iSize == item.iSize);
                            listProductInCart.Remove(sp);
                            return this.Checkout();
                        }
                        updateSoLuong(ctdh);
                        data.CT_DONHANGs.InsertOnSubmit(ctdh);
                        data.SubmitChanges();
                        Session["Giohang"] = null;
                    }
                    return Redirect(paymentUrl);
                }
                //ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;
               
            }
            return RedirectToAction("Thanks", "Cart");
        }
        public ActionResult Thanks()
        {
            return View();
        }
        public ActionResult Payment(FormCollection collection)
        {
            List<GioHang> gh = Session["GioHang"] as List<GioHang>;
            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOHFYY20220624";
            string accessKey = "WnATmOxM3j81MiWC";
            string serectkey = "abJUbwX6vQRQx55Tj1HZccoJqMMIuyaz";
            string orderInfo = "test";
            string returnUrl = "https://localhost:44359/Cart/ConfirmPaymentClient";
            string notifyurl = "https://53d0-123-21-167-207.ap.ngrok.io/GioHang/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            string amount = gh.Sum(n => n.iThanhTien).ToString();//sửa lại giá để lấi giá đơn hàng
            string orderid = DateTime.Now.Ticks.ToString();
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
            JObject jmessage = JObject.Parse(responseFromMomo);
            //luu database
            Session["billing_name"] = null;
            Session["billing_phone"] = null;
            Session["billing_address"] = null;
            Session["billing_note"] = null;

            Session["billing_name"] = collection["billing_name"];
            Session["billing_phone"] = collection["billing_phone"];
            Session["billing_address"] = collection["billing_address"];
            Session["billing_note"] = collection["billing_note"];
            string httt = collection["Payment"];
            if (httt == "Momo")
            {
                KHACH_HANG ac = (KHACH_HANG)Session["user"];
                DON_HANG ddh = new DON_HANG();
                //List<GioHang> gh = LayGioHang();
                //List<InfoCustomerBill> info = null;

                if (Session["user"] != null)
                {
                    ddh.TaiKhoanKH = ac.TaiKhoanKH;
                    ddh.TenNguoiNhan = collection["billing_name"];
                    ddh.SdtNguoiNhan = collection["billing_phone"];
                    ddh.DiaChiNhanHang = collection["billing_address"];
                    ddh.GhiChu = collection["billing_note"];
                    ddh.NgayLap = DateTime.Now;
                    ddh.TongTien = TongTien();
                    ddh.MaPTTT = 1;
                    ddh.TrangThaiDonHang = false;
                    ddh.TrangThaiGiaoHang = false;
                    data.DON_HANGs.InsertOnSubmit(ddh);
                    data.SubmitChanges();
                    Session["idInvoice"] = ddh.MaDH;
                    foreach (var item in gh)
                    {
                        CT_DONHANG ctdh = new CT_DONHANG();
                        ctdh.MaSize = (int)item.iSize;
                        ctdh.MaSP = (int)item.iIdProduct;
                        ctdh.MaDH = ddh.MaDH;
                        ctdh.SoLuong = item.iQuantityProduct;
                        ctdh.ThanhTien = item.iPriceProduct;
                        int soLuongTon = data.CT_SANPHAMs.SingleOrDefault(p => p.MaSP == item.iIdProduct && p.MaSize == item.iSize).SoLuong.Value;
                        if (soLuongTon < ctdh.SoLuong)
                        {
                            ViewBag.SoLuongTon = "Sản phẩm hết hàng hoặc quá số lượng tồn, sản phẩm hết hàng sẽ được xóa khỏi gio hàng!";
                            List<GioHang> listProductInCart = LayGioHang();
                            GioHang sp = listProductInCart.SingleOrDefault(n => n.iIdProduct == item.iIdProduct && n.iSize == item.iSize);
                            listProductInCart.Remove(sp);
                            return this.Checkout();
                        }
                        updateSoLuong(ctdh);
                        data.CT_DONHANGs.InsertOnSubmit(ctdh);
                        data.SubmitChanges();
                        Session["Giohang"] = null;
                    }
                }
                else
                {
                    var ten = collection["ten"];
                    var diachi = collection["diachi"];
                    var sdt = collection["sdt"];
                    var ghichu = collection["ghichu"];
                    String a = "khachhangvanglai";
                    ddh.TaiKhoanKH = a;
                    ddh.TenNguoiNhan = ten;
                    ddh.SdtNguoiNhan = diachi;
                    ddh.DiaChiNhanHang = sdt;
                    ddh.GhiChu = ghichu;
                    ddh.NgayLap = DateTime.Now;
                    ddh.TongTien = TongTien();
                    ddh.MaPTTT = 1;
                    ddh.TrangThaiDonHang = false;
                    ddh.TrangThaiGiaoHang = false;
                    data.DON_HANGs.InsertOnSubmit(ddh);
                    data.SubmitChanges();
                    Session["idInvoice"] = ddh.MaDH;
                    foreach (var item in gh)
                    {
                        CT_DONHANG ctdh = new CT_DONHANG();
                        ctdh.MaSize = (int)item.iSize;
                        ctdh.MaSP = (int)item.iIdProduct;
                        ctdh.MaDH = ddh.MaDH;
                        ctdh.SoLuong = item.iQuantityProduct;
                        ctdh.ThanhTien = item.iPriceProduct;
                        int soLuongTon = data.CT_SANPHAMs.SingleOrDefault(p => p.MaSP == item.iIdProduct && p.MaSize == item.iSize).SoLuong.Value;
                        if (soLuongTon < ctdh.SoLuong)
                        {
                            ViewBag.SoLuongTon = "Sản phẩm hết hàng hoặc quá số lượng tồn, sản phẩm hết hàng sẽ được xóa khỏi gio hàng!";
                            List<GioHang> listProductInCart = LayGioHang();
                            GioHang sp = listProductInCart.SingleOrDefault(n => n.iIdProduct == item.iIdProduct && n.iSize == item.iSize);
                            listProductInCart.Remove(sp);
                            return this.Checkout();
                        }
                        updateSoLuong(ctdh);
                        data.CT_DONHANGs.InsertOnSubmit(ctdh);
                        data.SubmitChanges();
                        Session["Giohang"] = null;
                        //return Redirect(jmessage.GetValue("payUrl").ToString());

                    }
                }
               // return RedirectToAction("Thanks", "Cart");
            }
            //return RedirectToAction("Thanks", "Cart");
            return Redirect(jmessage.GetValue("payUrl").ToString());
        }
        public ActionResult ConfirmPaymentClient()
        {
            return PartialView();
        }
        public ActionResult PaymentConfirm(FormCollection collection)
        {
           

            if (Request.QueryString.Count > 0)
            {
                string hashSecret = ConfigurationManager.AppSettings["HashSecret"]; //Chuỗi bí mật
                var vnpayData = Request.QueryString;
                XuLy pay = new XuLy();

                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }
                //mã hóa đơn
                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef"));

                //mã giao dịch tại hệ thống VNPAY
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo"));

                //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode");
                //hash của dữ liệu trả về
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                //check chữ ký đúng hay không?
                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret);

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")// đúng
                    {
                        ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;
                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                    }
                }
                else
                {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }

            return View();
        }

    }
}
