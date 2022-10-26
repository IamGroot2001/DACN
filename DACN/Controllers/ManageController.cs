using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DACN.Models;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using DACN.Asset.csharp;
using System.Web.UI.HtmlControls;

namespace DACN.Controllers
{
    public class ManageController : BaseController
    {
        //DAChuyenNganhDataContext db = new DAChuyenNganhDataContext();
        //// GET: Admin/Manage
        //public static string MD5Hash(string input)
        //{
        //    StringBuilder hash = new StringBuilder();
        //    MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
        //    byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

        //    for (int i = 0; i < bytes.Length; i++)
        //    {
        //        hash.Append(bytes[i].ToString("x2"));
        //    }
        //    return hash.ToString();
        //}
        ////========================================================================================

        //public ActionResult Receipt()
        //{
        //    if (Session["admin"] == null)
        //    {
        //        return RedirectToAction("LogIn", "Account");
        //    }
        //    var list = db.DON_HANGs.OrderByDescending(s => s.MaDH).ToList();

        //    return View(list);
        //}
        //public ActionResult DetailReceipt(int id)
        //{
        //    //InvoiceDetail ct = db.InvoiceDetails.Where(n => n.IdInvoice == id);
        //    ViewBag.ma = db.DON_HANGs.SingleOrDefault(n => n.MaDH == id);
        //    var ct = (from s in db.CT_DONHANGs where s.MaDH == id select s).ToList();
        //    return View(ct);
        //}


        //public ActionResult ConfilmInvoice(int id, string TrangThai)
        //{
        //    //bool a = true;
        //    bool a = bool.Parse(TrangThai);
        //    var ct = db.DON_HANGs.SingleOrDefault(n => n.MaDH == id);
        //    //var ct = from c in db.Invoices where c.IdInvoice == id select c;
        //    int idp = (from i in db.CT_DONHANGs where i.MaDH == id select i.MaDH).FirstOrDefault();
        //    Session["idp"] = id;

        //    ct.TrangThaiDonHang = a;
        //    UpdateModel(ct);
        //    db.SubmitChanges();
        //    return RedirectToAction("DetailReceipt", "Manage", new { id = idp });
        //}
        //public ActionResult CloseInvoice(int id, string TrangThai)
        //{
        //    //bool a = true;
        //    bool a = bool.Parse(TrangThai);
        //    var ct = db.DON_HANGs.SingleOrDefault(n => n.MaDH == id);
        //    //var ct = from c in db.Invoices where c.IdInvoice == id select c;
        //    int idp = (from i in db.CT_DONHANGs where i.MaDH == id select i.MaDH).FirstOrDefault();
        //    Session["idp"] = id;

        //    ct.TrangThaiDonHang = a;
        //    UpdateModel(ct);
        //    db.SubmitChanges();
        //    return RedirectToAction("DetailReceipt", "Manage", new { id = idp });
        //}
        //public ActionResult Dagiao(int id, string TrangThai)
        //{
        //    //bool a = true;
        //    bool a = bool.Parse(TrangThai);
        //    var ct = db.DON_HANGs.SingleOrDefault(n => n.MaDH == id);
        //    //var ct = from c in db.Invoices where c.IdInvoice == id select c;
        //    int idp = (from i in db.CT_DONHANGs where i.MaDH == id select i.MaDH).FirstOrDefault();
        //    Session["idp"] = id;

        //    //ct.Paid = a;  lick
        //    UpdateModel(ct);
        //    db.SubmitChanges();
        //    return RedirectToAction("DetailReceipt", "Manage", new { id = idp });
        //}
        //public ActionResult Chugiao(int id, string TrangThai)
        //{
        //    //bool a = true;
        //    bool a = bool.Parse(TrangThai);
        //    var ct = db.DON_HANGs.SingleOrDefault(n => n.MaDH == id);
        //    //var ct = from c in db.Invoices where c.IdInvoice == id select c;
        //    int idp = (from i in db.CT_DONHANGs where i.MaDH == id select i.MaDH).FirstOrDefault();
        //    Session["idp"] = id;

        //    //ct.Paid = a;
        //    UpdateModel(ct);
        //    db.SubmitChanges();
        //    return RedirectToAction("DetailReceipt", "Manage", new { id = idp });
        //}
        ////========================================================================================

        //public ActionResult Customer()
        //{
        //    if (Session["admin"] == null)
        //    {
        //        return RedirectToAction("LogIn", "Account");
        //    }
        //    var list = db.KHACH_HANGs.OrderByDescending(s => s.TaiKhoanKH).ToList();
        //    return View(list);
        //}
        ////========================================================================================
        //public ActionResult Size()
        //{
        //    if (Session["admin"] == null)
        //    {
        //        return RedirectToAction("LogIn", "Account");
        //    }
        //    var list = db.SIZEs.OrderByDescending(s => s.MaSize).ToList();

        //    return View(list);
        //}

        //[HttpGet]
        //public ActionResult AddSize()
        //{
        //    if (Session["admin"] == null)
        //    {
        //        return RedirectToAction("LogIn", "Account");
        //    }

        //    return View();
        //}

        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult AddSize(SIZE pr, FormCollection collection)
        //{
        //    if (Session["admin"] == null)
        //    {
        //        return RedirectToAction("LogIn", "Account");
        //    }
        //    var ten = collection["name"];

        //    pr.TenSize = ten;
        //    db.SIZEs.InsertOnSubmit(pr);
        //    db.SubmitChanges();
        //    SetAlert("Thêm size thành công", "success");
        //    return RedirectToAction("Size", "Manage");
        //}


        //public ActionResult DeleteSize(int id)
        //{
        //    if (Session["admin"] == null)
        //    {
        //        return RedirectToAction("Product", "Manage");
        //    }
        //    else
        //    {
        //        //var sex = db.SIZEs.SingleOrDefault(n => n.MaSize = id);
        //        //if (sex == null)
        //        //{
        //        //    Response.StatusCode = 404;
        //        //    return null;
        //        //}
        //        //try
        //        //{
        //        //    db.SIZEs.DeleteOnSubmit(sex);
        //        //    db.SubmitChanges();
        //        //    SetAlert("Xóa size thành công", "success");
        //        //}
        //        //catch
        //        //{
        //        //    SetAlert("Không xóa được size", "error");
        //        //}

        //        return RedirectToAction("Size");
        //    }
        //}
        ////========================================================================================
        //public ActionResult TypesClothes()
        //{
        //    if (Session["admin"] == null)
        //    {
        //        return RedirectToAction("LogIn", "Account");
        //    }
        //    var list = db.LOAI_SAN_PHAMs.OrderByDescending(s => s.MaLSP).ToList();
        //    return View(list);
        //}

        //[HttpGet]
        //public ActionResult AddTypesClothes()
        //{
        //    if (Session["admin"] == null)
        //    {
        //        return RedirectToAction("LogIn", "Account");
        //    }

        //    return View();
        //}

        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult AddTypesClothes(LOAI_SAN_PHAM pr, FormCollection collection)
        //{
        //    if (Session["admin"] == null)
        //    {
        //        return RedirectToAction("LogIn", "Account");
        //    }
        //    var ten = collection["name"];
        //    var s = collection["sex"];
        //    if (int.Parse(s) != 1 || int.Parse(s) != 0)
        //    {
        //        ViewData["1"] = "Bạn đã nhập sai !";
        //    }
        //    pr.TenLSP = ten;
        //    //pr.IdSex = Int32.Parse(s);  click
        //    db.LOAI_SAN_PHAMs.InsertOnSubmit(pr);
        //    db.SubmitChanges();
        //    SetAlert("Thêm loại sản phẩm thành công", "success");
        //    return RedirectToAction("TypesClothes", "Manage");
        //}

        ////[HttpGet]
        ////public ActionResult EditTypesClothes(int id)
        ////{

        ////    if (Session["admin"] == null)
        ////    {
        ////        return RedirectToAction("Product", "Manage");
        ////    }
        ////    else
        ////    {
        ////        LOAI_SAN_PHAM type = db.LOAI_SAN_PHAMs.SingleOrDefault(n => n.MaLSP == id);
        ////        if (type == null)
        ////        {
        ////            Response.StatusCode = 404;
        ////            return null;
        ////        }
        ////        return View(type);
        ////    }
        ////}

        ////[HttpPost]
        ////public ActionResult EditTypesClothes(FormCollection collection, int id)
        ////{
        ////    if (Session["admin"] == null)
        ////    {
        ////        return RedirectToAction("Product", "Manage");
        ////    }
        ////    else
        ////    {
        ////        var s = collection["sex"];
        ////        LOAI_SAN_PHAM type = db.LOAI_SAN_PHAMs.SingleOrDefault(n => n.MaLSP == id);
        ////        if (type == null)
        ////        {
        ////            Response.StatusCode = 404;
        ////            return null;
        ////        }
        ////        //type.IdSex = Int32.Parse(s); click
        ////        UpdateModel(type);
        ////        db.SubmitChanges();
        ////        return RedirectToAction("TypesClothes");
        ////    }
        ////}

        ////[HttpGet]
        ////public ActionResult DeleteProductType(int id)
        ////{
        ////    if (Session["admin"] == null)
        ////    {
        ////        return RedirectToAction("Product", "Manage");
        ////    }
        ////    else
        ////    {
        ////        LOAI_SAN_PHAM type = db.LOAI_SAN_PHAMs.SingleOrDefault(n => n.MaLSP == id);
        ////        if (type == null)
        ////        {
        ////            Response.StatusCode = 404;
        ////            return null;
        ////        }
        ////        return View(type);
        ////    }
        ////}

        ////[HttpPost, ActionName("DeleteProductType")]
        ////public ActionResult dDeleteProductType(int id)
        ////{
        ////    if (Session["admin"] == null)
        ////    {
        ////        return RedirectToAction("Product", "Manage");
        ////    }
        ////    else
        ////    {
        ////        LOAI_SAN_PHAM type = db.LOAI_SAN_PHAMs.SingleOrDefault(n => n.MaLSP == id);
        ////        if (type == null)
        ////        {
        ////            Response.StatusCode = 404;
        ////            return null;
        ////        }
        ////        try
        ////        {
        ////            db.LOAI_SAN_PHAMs.DeleteOnSubmit(type);
        ////            db.SubmitChanges();
        ////            SetAlert("Xóa loại sản phẩm thành công", "success");
        ////        }
        ////        catch
        ////        {
        ////            SetAlert("Không xóa được loại sản phẩm", "error");
        ////        }

        ////        return RedirectToAction("TypesClothes");
        ////    }
        ////}
        ////============================================================================================

        ////public ActionResult Product()
        ////{
        ////    if (Session["admin"] == null)
        ////    {
        ////        return RedirectToAction("LogIn", "Account");
        ////    }
        ////    var list = db.SAN_PHAMs.OrderByDescending(s => s.MaSP).ToList();
        ////    foreach (var item in list)
        ////    {
        ////        var soLuongTon = db.CT_SANPHAMs.Where(p => p.MaSP == item.MaSP).Select(p => p.SoLuongTon).ToList();
        ////        var demsanpham = soLuongTon.Sum(p => p.Value);
        ////        if (demsanpham > 0)
        ////        {
        ////            //item.StatusProduct = 1;
        ////        }
        ////        else
        ////        {
        ////            //item.StatusProduct = 0;
        ////        }

        ////    }
        ////    UpdateModel(list);
        ////    db.SubmitChanges();
        ////    return View(list);
        ////}


        //// hiển thị màn hình thêm sản phẩm
        //[HttpGet]
        //public ActionResult AddProduct()
        //{
        //    if (Session["admin"] == null)
        //    {
        //        return RedirectToAction("LogIn", "Account");
        //    }
        //    ViewBag.Loai = new SelectList(db.LOAI_SAN_PHAMs.ToList().OrderBy(n => n.TenLSP), "MaLSP", "TenLSP");
        //    ViewBag.Size = new SelectList(db.SIZEs.ToList().OrderBy(n => n.TenSize), "MaSize", "TenSize");
        //    return View();
        //}

        ////action thêm sản phẩm
        //[HttpPost]
        //public ActionResult AddProduct(SAN_PHAM pr, CT_SANPHAM dt, FormCollection collection, HttpPostedFileBase img)
        //{

        //    if (Session["admin"] == null)
        //    {
        //        return RedirectToAction("LogIn", "Account");
        //    }
        //    ViewBag.Loai = new SelectList(db.LOAI_SAN_PHAMs.ToList().OrderBy(n => n.TenLSP), "MaLSP", "TenLSP");
        //    ViewBag.Size = new SelectList(db.SIZEs.ToList().OrderBy(n => n.TenSize), "MaSize", "TenSize");

        //    //HtmlTextArea txtImageupload = (HtmlTextArea)(frm.FindControl("txtImagename1"));
        //    //string imagename = txtImageupload.Value;
        //    var ten = collection["name"];
        //    var gia = collection["price"];
        //    var date = DateTime.UtcNow.Date;
        //    var mota = collection["Mota"];
        //    var loai = collection["Loai"];
        //    var size = collection["Size"];
        //    var sl = collection["quality"];
        //    int status;
        //    if (int.Parse(sl) > 0)
        //    {
        //        status = 1;
        //    } else
        //    {
        //        status = 0;
        //    }

        //    var filename = Path.GetFileName(img.FileName);
        //    var path = Path.Combine(Server.MapPath("~/Assets/img/Clothes"), filename);

        //    img.SaveAs(path);
        //    pr.TenSP = ten;
        //    pr.HinhAnh = filename;
        //    pr.Gia = int.Parse(gia);
        //    pr.MoTa = mota;
        //    pr.NgayThem = date;
        //    pr.NgayCapNhat = date;
        //    //pr.MaLSP = Int32.Parse(loai);
        //    //pr.StatusProduct = status;

        //    db.SAN_PHAMs.InsertOnSubmit(pr);
        //    db.SubmitChanges();

        //    //dt.MaSize = Int32.Parse(size);
        //    dt.MaSP = pr.MaSP;
        //    dt.SoLuong = int.Parse(sl);
        //    db.CT_SANPHAMs.InsertOnSubmit(dt);
        //    db.SubmitChanges();
        //    return RedirectToAction("Product", "Manage");
        //}

        ////hiển thị màn hình chỉnh sửa sản phẩm
        //[HttpGet]
        //public ActionResult EditProduct(int id)
        //{
        //    if (Session["admin"] == null)
        //    {
        //        return RedirectToAction("Product", "Manage");
        //    }
        //    else
        //    {
        //        SAN_PHAM sp = db.SAN_PHAMs.SingleOrDefault(n => n.MaSP == id);
        //        if (sp == null)
        //        {
        //            Response.StatusCode = 404;
        //            return null;
        //        }
        //        ViewBag.Loai = new SelectList(db.LOAI_SAN_PHAMs.ToList().OrderBy(n => n.MaLSP), "IdProductType", "NameProductType");
        //        return View(sp);
        //    }
        //}
        ////action sửa sản phẩm
        //[HttpPost]
        //public ActionResult EditProduct(int id, HttpPostedFileBase img)
        //{
        //    ViewBag.Loai = new SelectList(db.LOAI_SAN_PHAMs.ToList().OrderBy(n => n.MaLSP), "IdProductType", "NameProductType");
        //    SAN_PHAM sp = db.SAN_PHAMs.SingleOrDefault(n => n.MaSP == id);
        //    var date = DateTime.UtcNow.Date;
        //    if (img != null)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var filename = Path.GetFileName(img.FileName);
        //            var path = Path.Combine(Server.MapPath("~/Assets/img/Clothes"), filename);
        //            if (!System.IO.File.Exists(path))
        //            {
        //                img.SaveAs(path);
        //                sp.HinhAnh = filename;
        //            }
        //        }
        //    }

        //    sp.NgayCapNhat = date;
        //    UpdateModel(sp);
        //    db.SubmitChanges();
        //    return RedirectToAction("Product");
        //}

        ////[HttpGet]
        ////public ActionResult DeleteProduct(int id)
        ////{
        ////    if (Session["admin"] == null)
        ////    {
        ////        return RedirectToAction("Product", "Manage");
        ////    }
        ////    else
        ////    {
        ////        SAN_PHAM sp = db.SAN_PHAMs.SingleOrDefault(n => n.IdProduct == id);
        ////        if (sp == null)
        ////        {
        ////            Response.StatusCode = 404;
        ////            return null;
        ////        }
        ////        return View(sp);
        ////    }
        ////}

        //[HttpPost, ActionName("DeleteProduct")]
        //public ActionResult dDeleteProduct(int id)
        //{
        //    if (Session["admin"] == null)
        //    {
        //        return RedirectToAction("Product", "Manage");
        //    }
        //    else
        //    {
        //        SAN_PHAM sp = db.SAN_PHAMs.SingleOrDefault(n => n.MaSP == id);
        //        if (sp == null)
        //        {
        //            Response.StatusCode = 404;
        //            return null;
        //        }
        //        try
        //        {
        //            db.SAN_PHAMs.DeleteOnSubmit(sp);
        //            db.SubmitChanges();
        //            SetAlert("Xóa sản phẩm thành công", "success");
        //        }
        //        catch
        //        {
        //            SetAlert("Không xóa được sản phẩm", "error");
        //        }

        //        return RedirectToAction("Product");
        //    }
        //}
        ////========================================================================================
        //public ActionResult DetailProduct(int id)
        //{
        //    //var ct = db.ProductDetails.SingleOrDefault(n => n.IdProduct == id);
        //    var ct = from c in db.CT_SANPHAMs where c.MaSP == id select c;
        //    int idp = (from i in db.CT_SANPHAMs where i.MaSP == id select i.MaSP).FirstOrDefault();
        //    Session["idp"] = id;
        //    return View(ct);
        //}
        //[HttpGet]
        //public ActionResult AddProductDetailSize()
        //{
        //    ViewBag.Size = new SelectList(db.SIZEs.ToList().OrderBy(n => n.TenSize), "IdSizeProduct", "NameSizeProduct");
        //    ViewBag.IdProduct = Session["idp"];
        //    return View();
        //}

        //[HttpPost]
        //[ValidateInput(false)]
        ////public ActionResult AddProductDetailSize( CT_SANPHAM pr, FormCollection collection, string url)
        ////{
        ////    ViewBag.Size = new SelectList(db.SIZEs.ToList().OrderBy(n => n.TenSize), "IdSizeProduct", "NameSizeProduct");
        ////    var sl = collection["Sl"];
        ////    var size = collection["Size"];
        ////    int idpd = (int)Session["idp"];
        ////    int idsize = Int32.Parse(size);
        ////    var idsizeProduct = (from s in db.CT_SANPHAMs where s.MaSP == idpd select s).ToList();
        ////    foreach(var item in idsizeProduct)
        ////    {
        ////        if (idsize == item.MaSize)
        ////        {
        ////            pr.IdProduct = idpd;
        ////            pr.IdSizeProduct = idsize;
        ////            item.SoLuongTon += int.Parse(sl);
        ////            pr.SoLuongTon = item.SoLuongTon;
        ////            db.SubmitChanges();
        ////            return RedirectToAction("DetailProduct", new {id = idpd});
        ////        }
        ////    }
        ////    pr.IdProduct = idpd;
        ////    pr.IdSizeProduct = idsize;
        ////    pr.SoLuongTon = int.Parse(sl);
        ////    db.ProductDetails.InsertOnSubmit(pr);
        ////    db.SubmitChanges();
        ////    return RedirectToAction("DetailProduct", new { id = idpd});
        ////}

        ////public ActionResult DeleteProductDetail(int id)
        ////{
        ////    int size = int.Parse(Request.QueryString["size"]);
        ////    ViewBag.IdProduct = Session["idp"];
        ////    int idpd = (int)Session["idp"];
        ////    ProductDetail sp = db.ProductDetails.Where(n => n.IdProduct == id && n.IdSizeProduct == size).SingleOrDefault();

        ////    try
        ////    {
        ////        db.ProductDetails.DeleteOnSubmit(sp);
        ////        db.SubmitChanges();
        ////        SetAlert("Xóa size thành công", "success");
        ////    }
        ////    catch
        ////    {
        ////        SetAlert("Không xóa được size", "error");
        ////    }

        ////    return RedirectToAction("DetailProduct", new { id = idpd });
        ////}

        ////public ActionResult DetailProducts(int id)
        ////{
        ////    Product ct = db.Products.SingleOrDefault(n => n.IdProduct == id);
        ////    return View(ct);
        ////}

    } 
}