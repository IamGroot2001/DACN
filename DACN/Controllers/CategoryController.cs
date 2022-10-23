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
        dbDAChuyenNganhDataContext dataContext = new dbDAChuyenNganhDataContext();
        public ActionResult ProductCategory()
        {
            var loaiSanPham = from lsp in dataContext.LOAI_SAN_PHAMs select lsp;
            return PartialView(loaiSanPham);
        }
    }
}