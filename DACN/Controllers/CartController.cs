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
                return RedirectToAction("Shop", "Home");
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
            return RedirectToAction("Thanks", "Cart");
        }
        public ActionResult Thanks()
        {
            return View();
        }

    }
}
