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
            return View();
        }

        public ActionResult ProductDetail()
        {
            return View();
        }
    }
}