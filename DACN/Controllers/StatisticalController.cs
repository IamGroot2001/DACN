using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DACN.Models;

namespace DACN.Controllers
{
    
    public class StatisticalController : Controller
    {
        // GET: Admin/Statistical
        DAChuyenNganhDataContext db = new DAChuyenNganhDataContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Statistical()
        {
            //ViewBag.Receiptcount = Receiptcount();
            //ViewBag.Customercount = Customercount();
            //ViewBag.Productcount = Productcount();
            //ViewBag.SexMen = SexMen();
            //ViewBag.TongTienUocTinh = TongTienUocTinh();
            //ViewBag.TotalInvoid = TotalInvoid();
            //ViewBag.StatusInvoic = StatusInvoic();
            //ViewBag.rStatusInvoic = rStatusInvoic();
            //ViewBag.paidf = paidInvoic();
            //ViewBag.paidr = rpaidInvoic();
            return View();
        }
        //private int Customercount()
        //{
        //   var count = db.Accounts.OrderByDescending(s => s.IdAccount).Count();
        //    return count;
        //}
        //private int Productcount()
        //{
        //    var count = db.Products.OrderByDescending(s => s.IdProduct).Count();
        //    return count;
        //}
        //private int Receiptcount()
        //{
        //    var count = db.Invoices.OrderByDescending(s => s.IdInvoice).Count();
        //    return count;
        //}
        //private int SexMen()
        //{
        //    var count = db.ProductTypes.OrderByDescending(s => s.IdProductType).Count();
        //    return count;
        //}
        //private int TotalInvoid()
        //{
        //    int tongTien = 0;
        //    var hd = db.Invoices.Select(p => p.TotalInvoice).Count();
        //    var tt = db.Invoices.Where(p => p.Paid == true).Select(p => p.TotalInvoice).Count();
        //    if (hd == 0)
        //    {
        //        return tongTien;
        //    }
        //    else 
        //    {
        //        if(tt == 0)
        //        {
        //            return tongTien;
        //        }
        //        else
        //        {
        //            tongTien = db.Invoices.Where(p => p.Paid == true).Select(p => p.TotalInvoice).Sum();
        //        }
        //    }
        //    return tongTien;
        //}

        //private int TongTienUocTinh()
        //{
        //    int tongTien = 0;
        //    var hd = db.Invoices.Select(p => p.TotalInvoice).Count();
        //    if (hd  == 0)
        //    {
        //        return tongTien;
        //    }
        //    else
        //    {
        //        tongTien = db.Invoices.Select(p => p.TotalInvoice).Sum();
        //    }
        //    return tongTien;
        //}
        //private int StatusInvoic()
        //{
        //    bool a = false;
        //    var count = (from s in db.Invoices where s.StatusInvoice == a select s).Count();
        //    //var count = db.Invoices.OrderByDescending(s => s.StatusInvoice = a).Count();
        //    return count;
        //}
        //private int rStatusInvoic()
        //{
        //    bool a = true;
        //    var count = (from s in db.Invoices where s.StatusInvoice == a select s).Count();
        //    //var count = db.Invoices.OrderByDescending(s => s.StatusInvoice = a).Count();
        //    return count;
        //}
        //private int paidInvoic()
        //{
        //    bool a = false;
        //    var count = (from s in db.Invoices where s.Paid == a select s).Count();
        //    //var count = db.Invoices.OrderByDescending(s => s.StatusInvoice = a).Count();
        //    return count;
        //}
        //private int rpaidInvoic()
        //{
        //    bool a = true;
        //    var count = (from s in db.Invoices where s.Paid == a select s).Count();
        //    //var count = db.Invoices.OrderByDescending(s => s.StatusInvoice = a).Count();
        //    return count;
        //}
    }
}