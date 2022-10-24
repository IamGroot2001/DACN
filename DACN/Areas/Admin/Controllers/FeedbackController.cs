using DoAn_Web_SellClothes.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn_Web_SellClothes.Areas.Admin.Controllers
{
    public class FeedbackController : Controller
    {
        // GET: Admin/Feedback
        DataClasses1DataContext db = new DataClasses1DataContext();
        public ActionResult Feedback()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }

            var list = db.Feedbacks.OrderByDescending(s => s.IdFeedback).ToList();
            return View(list);
        }
         
    }
}