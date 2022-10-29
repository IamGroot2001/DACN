using DACN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DACN.Controllers
{
    public class HistoryController : Controller
    {
        DAChuyenNganhDataContext data = new DAChuyenNganhDataContext();
        // GET: History
        public ActionResult History()
        {
            var ac = (KHACH_HANG)Session["user"];
            var history = from h in data.DON_HANGs where h.TaiKhoanKH == ac.TaiKhoanKH orderby h.MaDH descending select h;
            foreach (var item in history)
            {
                if (item.TrangThaiDonHang == false)
                {
                    ViewBag.StatusInvoice = "Chưa giao hàng";
                }
                else if (item.TrangThaiDonHang == true)
                {
                    ViewBag.StatusInvoice = "Đã giao hàng";
                }
                if (item.TrangThaiGiaoHang == false)
                {
                    ViewBag.Paid = "Chưa thanh toán";
                }
                else if (item.TrangThaiGiaoHang == true)
                {
                    ViewBag.Paid = "Đã thanh toán";
                }
            }
            return View(history);
        }
        //public ActionResult HistoryDetail()
        //{
        //    int madh = Int32.Parse(Request.QueryString["MaDH"]);
        //    ViewBag.idInvoice = madh;
        //    var chitiet = from ind in data.CT_DONHANGs
        //                        join i in data.DON_HANGs on ind.MaDH equals i.MaDH
        //                        join s in data.SIZEs on ind.MaSize equals s.MaSize
        //                        join p in data.SAN_PHAMs on ind.MaSP equals p.MaSP
        //                        where ind.MaDH == madh
        //                        select new CT_DONHANG
        //                        {
        //                            MaSP = p.MaSP,
        //                            ImageProduct = p.HinhAnh,
        //                             = p.TenSP,
        //                             = s.TenSize,
        //                            Quantity = ind.SoLuong,
        //                            UnitPrice = ind.ThanhTien,
        //                            ThanhTienInvoiceDetails = ind.SoLuong * ind.ThanhTien
        //                        };

        //    return View(chitiet);
        //}
    }
}