using DACN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DACN.Controllers
{
    public class HomeController : Controller
    {
        DAChuyenNganhDataContext dataContext = new DAChuyenNganhDataContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(FormCollection collection, FEEDBACK fb)
        {
            var namefeedback = collection["fullnamefeedback"];
            var emailfeedback = collection["emailfeedback"];
            var describefeedback = collection["textfeedback"];
            if (String.IsNullOrEmpty(namefeedback) || String.IsNullOrEmpty(emailfeedback) || String.IsNullOrEmpty(describefeedback))
            {
                ViewData["Error"] = "Vui lòng điền đầy đủ nội dung";
                return this.Contact();
            }
            else
            {
                fb.Ten = namefeedback;
                fb.Email = emailfeedback;
                fb.NoiDung = describefeedback;
                dataContext.FEEDBACKs.InsertOnSubmit(fb);
                dataContext.SubmitChanges();
                return RedirectToAction("Contact");
            }
        }

        public ActionResult Shop()
        {
            var dsSanPham = from dssp in dataContext.SAN_PHAMs select dssp;
            return View(dsSanPham);
        }
        [HttpGet]
        public ActionResult ProductDetail(int id)
        {
            var sanPham = dataContext.SAN_PHAMs.FirstOrDefault(p => p.MaSP == id);
            var maSize = dataContext.CT_SANPHAMs.Where(p => p.MaSP == id).Select(p => p.MaSize).ToList();
            var soLuongTon = dataContext.CT_SANPHAMs.Where(p => p.MaSP == id).Select(p => p.SoLuong).ToList();
            List<string> nameSize = new List<string>();
            SIZE size;
            foreach (var item in maSize)
            {
                size = dataContext.SIZEs.FirstOrDefault(p => p.MaSize == item);
                nameSize.Add(size.TenSize);
            }
            sanPham.idSize = maSize;
            sanPham.soluongton = soLuongTon;
            sanPham.sizeProduct = nameSize;

            var demsanpham = soLuongTon.Sum(p => p.Value);
            if (demsanpham <= 0)
            {
                sanPham.TrangThai = false;
            }
            else
            {
                sanPham.TrangThai = true;
            }
            return View(sanPham);
        }

        //public ActionResult SizeOfProduct()
        //{
        //    var sizeSanPham = from size in dataContext.SIZEs
        //                      join ct_sp in dataContext.CT_SANPHAMs
        //                      on size.MaSize equals ct_sp.MaSize
        //                      join sanpham in dataContext.SAN_PHAMs
        //                      on ct_sp.MaSP equals sanpham.MaSP
        //                      select size;
        //    return PartialView(sizeSanPham);
        //}
    }
}