using DACN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace DACN.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        DAChuyenNganhDataContext dataContext = new DAChuyenNganhDataContext();
        public ActionResult ProductCategory()
        {
            var loaiSanPham = from lsp in dataContext.LOAI_SAN_PHAMs select lsp;
            return PartialView(loaiSanPham);
        }

        public ActionResult ProductList()
        {
            var dsSanPham = from dssp in dataContext.SAN_PHAMs select dssp;
            return PartialView(dsSanPham);
        }

        public ActionResult SizeList()
        {
            var dsSize = from dss in dataContext.SIZEs select dss;
            return PartialView(dsSize);
        }

        public ActionResult FilteredProductTypeList(int id)
        {
            var product = from pd in dataContext.SAN_PHAMs where pd.MaLSP == id select pd;
            return View(product);
        }

        public ActionResult FilteredProductPriceList0To100()
        {
            int gia = 100000;
            var product = from pd in dataContext.SAN_PHAMs where pd.Gia <= gia select pd;
            return View(product);
        }

        public ActionResult FilteredProductPriceList100To250()
        {
            int giabatdau = 100000;
            int giaketthuc = 250000;
            var product = from pd in dataContext.SAN_PHAMs where pd.Gia > giabatdau && pd.Gia <= giaketthuc select pd;
            return View(product);
        }

        public ActionResult FilteredProductPriceList250To500()
        {
            int giabatdau = 250000;
            int giaketthuc = 500000;
            var product = from pd in dataContext.SAN_PHAMs where pd.Gia > giabatdau && pd.Gia <= giaketthuc select pd;
            return View(product);
        }

        public ActionResult FilteredProductPriceListOver500()
        {
            var product = from pd in dataContext.SAN_PHAMs where pd.Gia > 500000 select pd;
            return View(product);
        }
        public ActionResult SortProduct(string kieuSapXep, int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 8;
            if (kieuSapXep == null)
            {
                kieuSapXep = Session["KieuSapXep"].ToString();
                if (kieuSapXep == "giamdan")
                {
                    Session["KieuSapXep"] = "giamdan";
                    ViewBag.SapXepSanPham = "Sản phẩm theo giá giảm dàn";
                    var sanphamgiamdan = dataContext.SAN_PHAMs.OrderByDescending(s => s.Gia);
                    return View(sanphamgiamdan.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    Session["KieuSapXep"] = "tangdan";
                    ViewBag.SapXepSanPham = "Sản phẩm theo giá tăng dần";
                    var sanphamtangdan = dataContext.SAN_PHAMs.OrderBy(s => s.Gia);
                    return View(sanphamtangdan.ToPagedList(pageNumber, pageSize));
                }
            }
            else
            {
                if (kieuSapXep == "giamdan")
                {
                    Session["KieuSapXep"] = "giamdan";
                    ViewBag.SapXepSanPham = "Sản phẩm theo giá giảm dàn";
                    var sanphamgiamdan = dataContext.SAN_PHAMs.OrderByDescending(s => s.Gia);
                    return View(sanphamgiamdan.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    Session["KieuSapXep"] = "tangdan";
                    ViewBag.SapXepSanPham = "Sản phẩm theo giá tăng dần";
                    var sanphamtangdan = dataContext.SAN_PHAMs.OrderBy(s => s.Gia);
                    return View(sanphamtangdan.ToPagedList(pageNumber, pageSize));
                }
            }
        }

        [HttpPost]
        public ActionResult SearchingProduct(FormCollection collection, int? page)
        {
            string sTuKhoa = collection["txtTimKiem"].ToString();
            if (sTuKhoa == null)
            {
                List<SAN_PHAM> lstKQTK = dataContext.SAN_PHAMs.Where(n => n.TenSP.Contains((string)Session["TuKhoa"])).ToList();
                //phan trang
                int pageNumber = (page ?? 1);
                int pageSize = 8;
                if (lstKQTK.Count == 0)
                {
                    ViewBag.ThongBao = "Không tìm thấy sản phẩm nào cả";
                    return View(lstKQTK.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
                }
                ViewBag.ThongBao = "Đã tìm thấy " + lstKQTK.Count + " kêt quả!";
                return View(lstKQTK.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
            }
            else
            {
                Session["TuKhoa"] = sTuKhoa;
                List<SAN_PHAM> lstKQTK = dataContext.SAN_PHAMs.Where(n => n.TenSP.Contains(sTuKhoa)).ToList();
                //Phân trang 
                int pageNumber = (page ?? 1);
                int pageSize = 8;
                if (lstKQTK.Count == 0)
                {
                    ViewBag.ThongBao = "Không tìm thấy sản phẩm nào cả";
                    return View(lstKQTK.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
                }
                ViewBag.ThongBao = "Đã tìm thấy " + lstKQTK.Count + " kêt quả!";
                return View(lstKQTK.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
            }
        }
        [HttpGet]
        public ActionResult SearchingProduct(int? page, string sTuKhoa)
        {
            if (sTuKhoa != null)
            {
                Session["TuKhoa"] = sTuKhoa;
                List<SAN_PHAM> lstKQTK = dataContext.SAN_PHAMs.Where(n => n.TenSP.Contains(sTuKhoa)).ToList();
                //Phân trang 
                int pageNumber = (page ?? 1);
                int pageSize = 8;
                if (lstKQTK.Count == 0)
                {
                    ViewBag.ThongBao = "Không tìm thấy sản phẩm nào cả";
                    return View(lstKQTK.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
                }
                ViewBag.ThongBao = "Đã tìm thấy " + lstKQTK.Count + " kêt quả!";
                return View(lstKQTK.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
            }
            else
            {
                List<SAN_PHAM> lstKQTK = dataContext.SAN_PHAMs.Where(n => n.TenSP.Contains((string)Session["TuKhoa"])).ToList();
                //phan trang
                int pageNumber = (page ?? 1);
                int pageSize = 8;
                if (lstKQTK.Count == 0)
                {
                    ViewBag.ThongBao = "Không tìm thấy sản phẩm nào cả";
                    return View(lstKQTK.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
                }
                ViewBag.ThongBao = "Đã tìm thấy " + lstKQTK.Count + " kêt quả!";
                return View(lstKQTK.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
            }
        }


        /*public ActionResult ProductCount()
        {
            var slSanPham = (from slsp in dataContext.SAN_PHAMs select slsp).Count();
            ViewBag.message = "hello";
            return PartialView(slSanPham);
        }*/
    }
}