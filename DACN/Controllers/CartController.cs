using DACN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DACN.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult CheckOut()
        {
            return View();
        }

        public List<GioHang> LayGioHang()
        {
            List<GioHang> listgiohang = Session["GioHang"] as List<GioHang>;
            if (listgiohang == null)
            {
                //Nếu giỏ hàng chưa tồn tại thì khởi tạo listGioHang
                listgiohang = new List<GioHang>();
                Session["GioHang"] = listgiohang;
            }
            return listgiohang;
        }

        // thêm vào giỏ hàng
        public ActionResult ThemGioHang(int masp, string strUrl)
        {
            // lấy ra session Gio hang
            List<GioHang> gioHangs = LayGioHang();
            // kiểm tra sản phẩm này đã tồn tại trong giỏ hàng Session["GioHang"] chưa
            GioHang sp = gioHangs.Find(n => n.maSP == masp);
            if (sp == null)
            {
                sp = new GioHang(masp);
                gioHangs.Add(sp);
                return Redirect(strUrl);
            }
            else
            {
                sp.soLuong++;
                return Redirect(strUrl);
            }
        }

        // Tong so luong
        private int TongSoLuong()
        {
            int Tongsoluong = 0;
            List<GioHang> gioHangs = Session["GioHang"] as List<GioHang>;
            if (gioHangs != null)
            {
                Tongsoluong = gioHangs.Sum(n => n.soLuong);
            }
            Session["TongSoLuong"] = Tongsoluong;
            return Tongsoluong;
        }

        // Tính tổng tiền
        private double TongTien()
        {
            double tongtien = 0;
            List<GioHang> gioHangs = Session["GioHang"] as List<GioHang>;
            if (gioHangs != null)
            {
                tongtien = gioHangs.Sum(n => n.thanhTien);
            }
            return tongtien;
        }

        public ActionResult Cart()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if(lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View();
        }

    }
}