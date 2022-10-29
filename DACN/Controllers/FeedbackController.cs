using DACN.Models;
//using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DACN.Controllers
{
    public class FeedbackController : Controller
    {
        // GET: Admin/Feedback
        DAChuyenNganhDataContext db = new DAChuyenNganhDataContext();
        public ActionResult Feedback()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }

            var list = db.FEEDBACKs.OrderByDescending(s => s.Id).ToList();
            return View(list);
        }
         
    }
}