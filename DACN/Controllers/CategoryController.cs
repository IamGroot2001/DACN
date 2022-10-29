using DACN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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



        /*public ActionResult ProductCount()
        {
            var slSanPham = (from slsp in dataContext.SAN_PHAMs select slsp).Count();
            ViewBag.message = "hello";
            return PartialView(slSanPham);
        }*/
    }
}