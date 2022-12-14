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

        public ActionResult FilteredProductTypeList(int id, int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            var product = from pd in dataContext.SAN_PHAMs where pd.MaLSP == id select pd;
            return View(product.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult FilteredProductPriceList0To100(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            int gia = 100000;
            var product = from pd in dataContext.SAN_PHAMs where pd.Gia <= gia select pd;
            return View(product.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult FilteredProductPriceList100To250(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            int giabatdau = 100000;
            int giaketthuc = 250000;
            var product = from pd in dataContext.SAN_PHAMs where pd.Gia > giabatdau && pd.Gia <= giaketthuc select pd;
            return View(product.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult FilteredProductPriceList250To500(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            int giabatdau = 250000;
            int giaketthuc = 500000;
            var product = from pd in dataContext.SAN_PHAMs where pd.Gia > giabatdau && pd.Gia <= giaketthuc select pd;
            return View(product.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult FilteredProductPriceListOver500(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            var product = from pd in dataContext.SAN_PHAMs where pd.Gia > 500000 select pd;
            return View(product.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult SortProduct(string kieuSapXep, int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 9;
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
        public List<SAN_PHAM> getNewProduct(int count)
        {
            return dataContext.SAN_PHAMs.OrderByDescending(a => a.NgayThem).Take(count).ToList();
        }

        public ActionResult NewProduct()
        {
            var newProduct = getNewProduct(4);
            return PartialView(newProduct);
        }

        public ActionResult SanPhamCungLoai(int id)
        {
            var product = from sp in dataContext.SAN_PHAMs
                          where sp.MaLSP == id
                          select sp;
            return PartialView(product);
        }

        //public ActionResult SanPhamBanChay()
        //{
        //    /*var product = (from sp in dataContext.SAN_PHAMs
        //                   join ctdh in dataContext.CT_DONHANGs on sp.MaSP equals ctdh.MaSP
        //                   where sp.MaSP == ctdh.MaSP 
        //                   select sp);*/

        //    /*var product = (from sp in dataContext.SAN_PHAMs
        //                   join ctdh in dataContext.CT_DONHANGs on sp.MaSP equals ctdh.MaSP
        //                   group sp by sp.MaSP into g
        //                   orderby g descending
        //                   select g).Take(8).Distinct().Count();*/

        //    /*var product = dataContext.SAN_PHAMs
        //        .Join(dataContext.CT_DONHANGs, 
        //        sanpham => sanpham.MaSP,
        //        ctdh => ctdh.MaSP,
        //        (sanpham, ctdh) => new {Sanpham = sanpham, Ctdh = ctdh})
        //        .Where(sp => sp.Sanpham.MaSP == )*/

        //    /*var product = from sp in dataContext.SAN_PHAMs
        //                  join ctdh in dataContext.CT_DONHANGs on sp.MaSP equals ctdh.MaSP
        //                  group sp by ctdh.MaSP into g
        //                  select new
        //                  {
        //                      g = g.Key,
        //                  };*/

        //    var product = (from p in .SAN_PHAMs
        //                   let totalQuantify = (from ctdh in dataContext.CT_DONHANGs
        //                                        join dh in dataContext.DON_HANGs on ctdh.MaDH equals dh.MaDH
        //                                        where ctdh.MaDH == dh.MaDH
        //                                        select ctdh.SoLuong).Sum()
        //                   where totalQuantify > 0
        //                   orderby totalQuantify descending
        //                   select p);

        //    /*var product = from SP in dataContext.SAN_PHAMs
        //                  join CTDH in dataContext.CT_DONHANGs on SP.MaSP equals CTDH.MaSP
        //               group CTDH by new { SP.MaSP, SP.TenSP } into g
        //               select new
        //               {
        //                   g.Key.MaSP,
        //                   g.Key.TenSP,
        //                   SLBanDc = g.Sum(p => p.SoLuong)
        //               };*/        

        //    return PartialView(product);
        //}

        /*public ActionResult ProductCount()
        {
            var slSanPham = (from slsp in dataContext.SAN_PHAMs select slsp).Count();
            ViewBag.message = "hello";
            return PartialView(slSanPham);
        }*/
    }
}