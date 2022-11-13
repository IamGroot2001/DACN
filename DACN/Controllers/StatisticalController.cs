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
            ViewBag.nhanvien = nhanvien();
            ViewBag.nhanviendaxoa = nvdaxoa();          
            return View();
        }
        private int nhanvien()
        {
            var count = (from s in db.NHAN_VIENs where s.MaCV == 2 && s.MaCV ==3 select s).Count();
            //var count = db.KHACH_HANGs.OrderByDescending(s => s.TaiKhoanKH).Count();
            return count;
        }
        private int nvdaxoa()
        {
            var count = (from s in db.NHAN_VIENs where s.MaCV == 4 select s).Count();
            return count;
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
            var hd = db.DON_HANGs.Select(p => p.TongTien).Count();
            var tt = db.DON_HANGs.Where(p => p.TrangThaiGiaoHang == true).Select(p => p.TongTien).Count();
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
                    tongTien = (int)db.DON_HANGs.Where(p => p.TrangThaiGiaoHang == true).Select(p => p.TongTien).Sum();
                }
            }
            return tongTien;
        }

        private int TongTienUocTinh()
        {
            int tongTien = 0;
            var hd = db.DON_HANGs.Select(p => p.TongTien).Count();
            if (hd == 0)
            {
                return tongTien;
            }
            else
            {
                tongTien =(int) db.DON_HANGs.Select(p => p.TongTien).Sum();
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
        [HttpPost]
        public ActionResult GetDoanhThuTheoThang()
        {
            long Thang1 = 0; long Thang2 = 0; long Thang3 = 0; long Thang4 = 0; long Thang5 = 0; long Thang6 = 0;
            long Thang7 = 0; long Thang8 = 0; long Thang9 = 0; long Thang10 = 0; long Thang11 = 0; long Thang12 = 0;
            List<DON_HANG> listHD = db.DON_HANGs.ToList();
            foreach (var item in listHD)
            {
                if (item.NgayLap.GetValueOrDefault().Year == DateTime.Now.Year)
                {
                    if (item.TrangThaiDonHang == true)
                    {
                        switch (item.NgayLap.GetValueOrDefault().Month)
                        {
                            case 1:
                                Thang1 += (long)db.CT_DONHANGs.Where(p => p.MaDH == item.MaDH).Sum(i => i.ThanhTien * i.SoLuong);
                                break;
                            case 2:
                                Thang2 += (long)db.CT_DONHANGs.Where(p => p.MaDH == item.MaDH).Sum(i => i.ThanhTien * i.SoLuong);
                                break;
                            case 3:
                                Thang3 += (long)db.CT_DONHANGs.Where(p => p.MaDH == item.MaDH).Sum(i => i.ThanhTien * i.SoLuong);
                                break;
                            case 4:
                                Thang4 += (long)db.CT_DONHANGs.Where(p => p.MaDH == item.MaDH).Sum(i => i.ThanhTien * i.SoLuong);
                                break;
                            case 5:
                                Thang5 += (long)db.CT_DONHANGs.Where(p => p.MaDH == item.MaDH).Sum(i => i.ThanhTien * i.SoLuong);
                                break;
                            case 6:
                                Thang6 += (long)db.CT_DONHANGs.Where(p => p.MaDH == item.MaDH).Sum(i => i.ThanhTien * i.SoLuong);
                                break;
                            case 7:
                                Thang7 += (long)db.CT_DONHANGs.Where(p => p.MaDH == item.MaDH).Sum(i => i.ThanhTien * i.SoLuong);
                                break;
                            case 8:
                                Thang8 += (long)db.CT_DONHANGs.Where(p => p.MaDH == item.MaDH).Sum(i => i.ThanhTien * i.SoLuong);
                                break;
                            case 9:
                                Thang9 += (long)db.CT_DONHANGs.Where(p => p.MaDH == item.MaDH).Sum(i => i.ThanhTien * i.SoLuong);
                                break;
                            case 10:
                                Thang10 += (long)db.CT_DONHANGs.Where(p => p.MaDH == item.MaDH).Sum(i => i.ThanhTien * i.SoLuong);
                                break;
                            case 11:
                                Thang11 += (long)db.CT_DONHANGs.Where(p => p.MaDH == item.MaDH).Sum(i => i.ThanhTien * i.SoLuong);
                                break;
                            case 12:
                                Thang12 += (long)db.CT_DONHANGs.Where(p => p.MaDH == item.MaDH).Sum(i => i.ThanhTien * i.SoLuong);
                                break;
                            default:
                                break;
                        }
                    }
                }

            }
            return Json(new { Thang1, Thang2, Thang3, Thang4, Thang5, Thang6, Thang7, Thang8, Thang9, Thang10, Thang11, Thang12 });
        }

        [HttpPost]
        public ActionResult GetPTShip()
        {
            long ShipCOD = 0;
            float cod = 0;
            long VNpay = 0;
            float momo = 0;
            List<DON_HANG> listHD = db.DON_HANGs.ToList();
            foreach (var item in listHD)
            {
                if (item.MaPTTT == 1)
                {
                    ShipCOD++;
                }
                else if (item.MaPTTT == 2)
                {
                    VNpay++;
                }

            }
            long tong = (ShipCOD + VNpay) == 0 ? 1 : (ShipCOD + VNpay);
            cod = (ShipCOD * 100) / tong;
            momo = (VNpay * 100) / tong;
            return Json(new { cod, momo });
        }
    }
}