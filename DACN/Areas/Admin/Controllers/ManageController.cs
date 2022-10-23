using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn_Web_SellClothes.Models;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using DoAn_Web_SellClothes.Assets.csharp;
using System.Web.UI.HtmlControls;

namespace DoAn_Web_SellClothes.Areas.Admin.Controllers
{
    public class ManageController : BaseController
    {
        DataClasses1DataContext db =new DataClasses1DataContext();
        // GET: Admin/Manage
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
        //========================================================================================

        public ActionResult Receipt()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            var list = db.Invoices.OrderByDescending(s => s.IdInvoice).ToList();
            
            return View(list);
        }
        public ActionResult DetailReceipt(int id)
        {
            //InvoiceDetail ct = db.InvoiceDetails.Where(n => n.IdInvoice == id);
            ViewBag.ma = db.Invoices.SingleOrDefault(n => n.IdInvoice == id);
            var ct = (from s in db.InvoiceDetails where s.IdInvoice == id select s).ToList();
            return View(ct);
        }

       
        public ActionResult ConfilmInvoice(int id, string TrangThai)
        {
            //bool a = true;
            bool a = bool.Parse(TrangThai);
            var ct = db.Invoices.SingleOrDefault(n => n.IdInvoice == id);
            //var ct = from c in db.Invoices where c.IdInvoice == id select c;
            int idp = (from i in db.InvoiceDetails where i.IdInvoice == id select i.IdInvoice).FirstOrDefault();
            Session["idp"] = id; 

            ct.StatusInvoice = a;
            UpdateModel(ct);
            db.SubmitChanges();
            return RedirectToAction("DetailReceipt", "Manage", new { id = idp });
        }
        public ActionResult CloseInvoice(int id, string TrangThai)
        {
            //bool a = true;
            bool a = bool.Parse(TrangThai);
            var ct = db.Invoices.SingleOrDefault(n => n.IdInvoice == id);
            //var ct = from c in db.Invoices where c.IdInvoice == id select c;
            int idp = (from i in db.InvoiceDetails where i.IdInvoice == id select i.IdInvoice).FirstOrDefault();
            Session["idp"] = id;

            ct.StatusInvoice = a;
            UpdateModel(ct);
            db.SubmitChanges();
            return RedirectToAction("DetailReceipt", "Manage", new { id = idp });
        }
        public ActionResult Dagiao(int id, string TrangThai)
        {
            //bool a = true;
            bool a = bool.Parse(TrangThai);
            var ct = db.Invoices.SingleOrDefault(n => n.IdInvoice == id);
            //var ct = from c in db.Invoices where c.IdInvoice == id select c;
            int idp = (from i in db.InvoiceDetails where i.IdInvoice == id select i.IdInvoice).FirstOrDefault();
            Session["idp"] = id;

            ct.Paid = a;
            UpdateModel(ct);
            db.SubmitChanges();
            return RedirectToAction("DetailReceipt", "Manage", new { id = idp });
        }
        public ActionResult Chugiao(int id, string TrangThai)
        {
            //bool a = true;
            bool a = bool.Parse(TrangThai);
            var ct = db.Invoices.SingleOrDefault(n => n.IdInvoice == id);
            //var ct = from c in db.Invoices where c.IdInvoice == id select c;
            int idp = (from i in db.InvoiceDetails where i.IdInvoice == id select i.IdInvoice).FirstOrDefault();
            Session["idp"] = id;

            ct.Paid = a;
            UpdateModel(ct);
            db.SubmitChanges();
            return RedirectToAction("DetailReceipt", "Manage", new { id = idp });
        }
        //========================================================================================

        public ActionResult Customer()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }                   
            var list = db.Accounts.OrderByDescending(s => s.IdAccount ).ToList();
            return View(list);
        }
        //========================================================================================
        public ActionResult Size()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            var list = db.SizeProducts.OrderByDescending(s => s.IdSizeProduct).ToList();

            return View(list);
        }

        [HttpGet]
        public ActionResult AddSize()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddSize(SizeProduct pr, FormCollection collection)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            var ten = collection["name"];

            pr.NameSizeProduct = ten;
            db.SizeProducts.InsertOnSubmit(pr);
            db.SubmitChanges();
            SetAlert("Thêm size thành công", "success");
            return RedirectToAction("Size", "Manage");
        }       


        public ActionResult DeleteSize(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Product", "Manage");
            }
            else
            {
                var sex = db.SizeProducts.SingleOrDefault(n => n.IdSizeProduct == id);
                if (sex == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                try
                {
                    db.SizeProducts.DeleteOnSubmit(sex);
                    db.SubmitChanges();
                    SetAlert("Xóa size thành công", "success");
                }
                catch
                {
                    SetAlert("Không xóa được size", "error");
                }
               
                return RedirectToAction("Size");
            }
        }       
        //========================================================================================
        public ActionResult TypesClothes()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            var list = db.ProductTypes.OrderByDescending(s => s.IdProductType ).ToList();
            return View(list);
        }
        
        [HttpGet]
        public ActionResult AddTypesClothes()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            
            return View();
        }     

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddTypesClothes(ProductType pr, FormCollection collection)
        {           
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            var ten = collection["name"];        
            var s = collection["sex"];
            if( int.Parse(s) != 1 || int.Parse(s) != 0)
            {
                ViewData["1"] = "Bạn đã nhập sai !";
            }
            pr.NameProductType = ten;
            pr.IdSex = Int32.Parse(s);
            db.ProductTypes.InsertOnSubmit(pr);
            db.SubmitChanges();
            SetAlert("Thêm loại sản phẩm thành công", "success");
            return RedirectToAction("TypesClothes", "Manage");
        } 

        [HttpGet]
        public ActionResult EditTypesClothes(int id)
        {

            if (Session["admin"] == null)
            {
                return RedirectToAction("Product", "Manage");
            }
            else
            {
                ProductType type = db.ProductTypes.SingleOrDefault(n => n.IdProductType == id);
                if (type == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }                             
                return View(type);
            }
        }

        [HttpPost]
        public ActionResult EditTypesClothes(FormCollection collection, int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Product", "Manage");
            }
            else
            {
                var s = collection["sex"];
                ProductType type = db.ProductTypes.SingleOrDefault(n => n.IdProductType == id);
                if (type == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                type.IdSex = Int32.Parse(s);
                UpdateModel(type);
                db.SubmitChanges();
                return RedirectToAction("TypesClothes");
            }
        }

        [HttpGet]
        public ActionResult DeleteProductType(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Product", "Manage");
            }
            else
            {
                ProductType type = db.ProductTypes.SingleOrDefault(n => n.IdProductType == id);
                if (type == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(type);
            }
        }

        [HttpPost, ActionName("DeleteProductType")]
        public ActionResult dDeleteProductType(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Product", "Manage");
            }
            else
            {
                ProductType type = db.ProductTypes.SingleOrDefault(n => n.IdProductType == id);
                if (type == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                try
                {
                    db.ProductTypes.DeleteOnSubmit(type);
                    db.SubmitChanges();
                    SetAlert("Xóa loại sản phẩm thành công", "success");
                }
                catch
                {
                    SetAlert("Không xóa được loại sản phẩm", "error");
                }
              
                return RedirectToAction("TypesClothes");
            }
        }
        //============================================================================================

        public ActionResult Product()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            var list = db.Products.OrderByDescending(s => s.IdProduct).ToList();
            foreach (var item in list)
            {
                var soLuongTon = db.ProductDetails.Where(p => p.IdProduct == item.IdProduct).Select(p => p.SoLuongTon).ToList();
                var demsanpham = soLuongTon.Sum(p => p.Value);
                if (demsanpham > 0)
                {
                    item.StatusProduct = 1;
                }
                else
                {
                    item.StatusProduct = 0;
                }

            }
            UpdateModel(list);
            db.SubmitChanges();
            return View(list);
        }


        // hiển thị màn hình thêm sản phẩm
        [HttpGet]
        public ActionResult AddProduct()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            ViewBag.Loai = new SelectList(db.ProductTypes.ToList().OrderBy(n => n.NameProductType), "IdProductType", "NameProductType");
            ViewBag.Size = new SelectList(db.SizeProducts.ToList().OrderBy(n => n.NameSizeProduct), "IdSizeProduct", "NameSizeProduct");
            return View();
        }

        //action thêm sản phẩm
        [HttpPost]
        public ActionResult AddProduct(Product pr, ProductDetail dt, FormCollection collection, HttpPostedFileBase img)
        {
            
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            ViewBag.Loai = new SelectList(db.ProductTypes.ToList().OrderBy(n => n.IdProductType), "IdProductType", "NameProductType");
            ViewBag.Size = new SelectList(db.SizeProducts.ToList().OrderBy(n => n.NameSizeProduct), "IdSizeProduct", "NameSizeProduct");

            //HtmlTextArea txtImageupload = (HtmlTextArea)(frm.FindControl("txtImagename1"));
            //string imagename = txtImageupload.Value;
            var ten = collection["name"];
            var gia = collection["price"];
            var date = DateTime.UtcNow.Date;
            var mota = collection["Mota"];
            var loai = collection["Loai"];
            var size = collection["Size"];
            var sl = collection["quality"];
            int status;
            if (int.Parse(sl) > 0) 
            {
                status = 1;
            }else
            {
                status = 0;
            }

            var filename = Path.GetFileName(img.FileName); 
            var path = Path.Combine(Server.MapPath("~/Assets/img/Clothes"), filename);

            img.SaveAs(path);
            pr.NameProduct = ten;
            pr.ImageProduct = filename;
            pr.PriceProduct = int.Parse(gia);
            pr.DescribeProduct = mota;
            pr.CreateDate = date;
            pr.UpdateDate = date;
            pr.IdProductType = Int32.Parse(loai);
            pr.StatusProduct = status;

            db.Products.InsertOnSubmit(pr);
            db.SubmitChanges();
           
            dt.IdSizeProduct = Int32.Parse(size);
            dt.IdProduct = pr.IdProduct;
            dt.SoLuongTon = int.Parse(sl);
            db.ProductDetails.InsertOnSubmit(dt);
            db.SubmitChanges();
            return RedirectToAction("Product", "Manage");
        }

        //hiển thị màn hình chỉnh sửa sản phẩm
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Product", "Manage");
            }
            else
            {
                Product sp = db.Products.SingleOrDefault(n => n.IdProduct == id);
                if (sp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                ViewBag.Loai = new SelectList(db.ProductTypes.ToList().OrderBy(n => n.IdProductType), "IdProductType", "NameProductType");
                return View(sp);
            }
        }
        //action sửa sản phẩm
        [HttpPost]
        public ActionResult EditProduct(int id, HttpPostedFileBase img)
        {
            ViewBag.Loai = new SelectList(db.ProductTypes.ToList().OrderBy(n => n.IdProductType), "IdProductType", "NameProductType");
            Product sp = db.Products.SingleOrDefault(n => n.IdProduct == id);
            var date = DateTime.UtcNow.Date;
            if (img != null)
            {
                if (ModelState.IsValid)
                {
                    var filename = Path.GetFileName(img.FileName);
                    var path = Path.Combine(Server.MapPath("~/Assets/img/Clothes"), filename);
                    if (!System.IO.File.Exists(path))
                    {
                        img.SaveAs(path);
                        sp.ImageProduct = filename;
                    }
                }
            }
           
            sp.UpdateDate = date;
            UpdateModel(sp);
            db.SubmitChanges();
            return RedirectToAction("Product");
        }

        [HttpGet]
        public ActionResult DeleteProduct(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Product", "Manage");
            }
            else
            {
                Product sp = db.Products.SingleOrDefault(n => n.IdProduct == id);
                if (sp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(sp);
            }
        }

        [HttpPost, ActionName("DeleteProduct")]
        public ActionResult dDeleteProduct(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Product", "Manage");
            }
            else
            {
                Product sp = db.Products.SingleOrDefault(n => n.IdProduct == id);
                if (sp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                try
                {
                    db.Products.DeleteOnSubmit(sp);
                    db.SubmitChanges();
                    SetAlert("Xóa sản phẩm thành công", "success");
                }
                catch
                {
                    SetAlert("Không xóa được sản phẩm", "error");
                }
               
                return RedirectToAction("Product");
            }
        }
        //========================================================================================
        public ActionResult DetailProduct( int id)
        {
            //var ct = db.ProductDetails.SingleOrDefault(n => n.IdProduct == id);
            var ct = from c in db.ProductDetails where c.IdProduct == id select c;
            int idp = (from i in db.ProductDetails where i.IdProduct == id select i.IdProduct).FirstOrDefault();
            Session["idp"]=id;
            return View(ct);
        }
        [HttpGet]
        public ActionResult AddProductDetailSize()
        {
            ViewBag.Size = new SelectList(db.SizeProducts.ToList().OrderBy(n => n.NameSizeProduct), "IdSizeProduct", "NameSizeProduct");
            ViewBag.IdProduct = Session["idp"];
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddProductDetailSize( ProductDetail pr, FormCollection collection, string url)
        {
            ViewBag.Size = new SelectList(db.SizeProducts.ToList().OrderBy(n => n.NameSizeProduct), "IdSizeProduct", "NameSizeProduct");
            var sl = collection["Sl"];
            var size = collection["Size"];
            int idpd = (int)Session["idp"];
            int idsize = Int32.Parse(size);
            var idsizeProduct = (from s in db.ProductDetails where s.IdProduct == idpd select s).ToList();
            foreach(var item in idsizeProduct)
            {
                if (idsize == item.IdSizeProduct)
                {
                    pr.IdProduct = idpd;
                    pr.IdSizeProduct = idsize;
                    item.SoLuongTon += int.Parse(sl);
                    pr.SoLuongTon = item.SoLuongTon;
                    db.SubmitChanges();
                    return RedirectToAction("DetailProduct", new {id = idpd});
                }
            }
            pr.IdProduct = idpd;
            pr.IdSizeProduct = idsize;
            pr.SoLuongTon = int.Parse(sl);
            db.ProductDetails.InsertOnSubmit(pr);
            db.SubmitChanges();
            return RedirectToAction("DetailProduct", new { id = idpd});
        }

        public ActionResult DeleteProductDetail(int id)
        {
            int size = int.Parse(Request.QueryString["size"]);
            ViewBag.IdProduct = Session["idp"];
            int idpd = (int)Session["idp"];
            ProductDetail sp = db.ProductDetails.Where(n => n.IdProduct == id && n.IdSizeProduct == size).SingleOrDefault();

            try
            {
                db.ProductDetails.DeleteOnSubmit(sp);
                db.SubmitChanges();
                SetAlert("Xóa size thành công", "success");
            }
            catch
            {
                SetAlert("Không xóa được size", "error");
            }
            
            return RedirectToAction("DetailProduct", new { id = idpd });
        }

        public ActionResult DetailProducts(int id)
        {
            Product ct = db.Products.SingleOrDefault(n => n.IdProduct == id);
            return View(ct);
        }
    }
}