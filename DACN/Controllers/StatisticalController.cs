using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DACN.Models;

namespace DACN.Controllers
{
    
    public class StatisticalController : Controller
    {
        // GET: Admin/Statistical
        DAChuyenNganhDataContext db = new DAChuyenNganhDataContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Statistical()
        {
            ViewBag.Receiptcount = Receiptcount();
            ViewBag.Customercount = Customercount();
            ViewBag.Productcount = Productcount();
            ViewBag.SexMen = SexMen();
            ViewBag.TongTienUocTinh = TongTienUocTinh();
            ViewBag.TotalInvoid = TotalInvoid();
            ViewBag.StatusInvoic = StatusInvoic();
            ViewBag.rStatusInvoic = rStatusInvoic();
            ViewBag.paidf = paidInvoic();
            ViewBag.paidr = rpaidInvoic();
            return View();
        }
        private int Customercount()
        {
            var count = db.KHACH_HANGs.OrderByDescending(s => s.TaiKhoanKH).Count();
            return count;
        }
        private int Productcount()
        {
            var count = db.SAN_PHAMs.OrderByDescending(s => s.MaSP).Count();
            return count;
        }
        private int Receiptcount()
        {
            var count = db.DON_HANGs.OrderByDescending(s => s.MaDH).Count();
            return count;
        }
        private int SexMen()
        {
            var count = db.LOAI_SAN_PHAMs.OrderByDescending(s => s.MaLSP).Count();
            return count;
        }
        private int TotalInvoid()
        {
            int tongTien = 0;
            var hd = db.DON_HANGs.Select(p => p.TenNguoiNhan).Count();
            var tt = db.DON_HANGs.Where(p => p.TrangThaiGiaoHang == true).Select(p => p.MaDH).Count();
            if (hd == 0)
            {
                return tongTien;
            }
            else
            {
                if (tt == 0)
                {
                    return tongTien;
                }
                else
                {
                    tongTien = db.DON_HANGs.Where(p => p.TrangThaiGiaoHang == true).Select(p => p.MaDH).Sum();
                }
            }
            return tongTien;
        }

        private int TongTienUocTinh()
        {
            int tongTien = 0;
            var hd = db.DON_HANGs.Select(p => p.MaDH).Count();
            if (hd == 0)
            {
                return tongTien;
            }
            else
            {
                tongTien = db.DON_HANGs.Select(p => p.MaDH).Sum();
            }
            return tongTien;
        }
        private int StatusInvoic()
        {
            bool a = false;
            var count = (from s in db.DON_HANGs where s.TrangThaiDonHang == a select s).Count();
            //var count = db.Invoices.OrderByDescending(s => s.StatusInvoice = a).Count();
            return count;
        }
        private int rStatusInvoic()
        {
            bool a = true;
            var count = (from s in db.DON_HANGs where s.TrangThaiDonHang == a select s).Count();
            //var count = db.Invoices.OrderByDescending(s => s.StatusInvoice = a).Count();
            return count;
        }
        private int paidInvoic()
        {
            bool a = false;
            var count = (from s in db.DON_HANGs where s.TrangThaiGiaoHang == a select s).Count();
            //var count = db.Invoices.OrderByDescending(s => s.StatusInvoice = a).Count();
            return count;
        }
        private int rpaidInvoic()
        {
            bool a = true;
            var count = (from s in db.DON_HANGs where s.TrangThaiGiaoHang == a select s).Count();
            //var count = db.Invoices.OrderByDescending(s => s.StatusInvoice = a).Count();
            return count;
        }
    }
}