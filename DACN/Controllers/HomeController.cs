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

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Shop()
        {
            var dsSanPham = from dssp in dataContext.SAN_PHAMs select dssp;
            return View(dsSanPham);
        }

        public ActionResult ProductDetail(int id)
        {
            var sanpham = from s in dataContext.SAN_PHAMs
                          where s.MaSP == id
                          select s;
            return View(sanpham.Single());
        }
    }
}