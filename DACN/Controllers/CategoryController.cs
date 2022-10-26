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

        /*public ActionResult ProductCount()
        {
            var slSanPham = (from slsp in dataContext.SAN_PHAMs select slsp).Count();
            ViewBag.message = "hello";
            return PartialView(slSanPham);
        }*/
    }
}