using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DACN.Models
{
    public class GioHang
    {
        DAChuyenNganhDataContext dataContext = new DAChuyenNganhDataContext();
        public int iIdProduct { get; set; }
        public int iSize { get; set; }
        public string iImageProduct { get; set; }
        public string iNameProduct { get; set; }
        public int iPriceProduct { get; set; }
        public int iQuantityProduct { get; set; }
        public string iSizeProduct { get; set; }
        public int? iMaxAmount { get; set; }//Số lượng tồn
        public int iThanhTien
        {
            get { return (int)(iQuantityProduct * iPriceProduct); }
        }
        //Khởi tạo giỏ hành theo Mã sản phẩm truyền vào với số lượng mạc định là 1
        public GioHang(int idProduct,int sizeProduct, int quantity)
        {
            iIdProduct = idProduct;
            iSize = sizeProduct;
            iQuantityProduct = quantity;
            SAN_PHAM product = dataContext.SAN_PHAMs.Single(n => n.MaSP == iIdProduct);
            iImageProduct = product.HinhAnh;
            iNameProduct = product.TenSP;
            iPriceProduct = (int)product.Gia;
            var sizeproduct = dataContext.SIZEs.FirstOrDefault(p => p.MaSize == iSize);
            iSizeProduct = sizeproduct.TenSize;
            var maxAmount = dataContext.CT_SANPHAMs.FirstOrDefault(p => p.MaSP == idProduct && p.SIZE == sizeproduct);
            iMaxAmount = maxAmount.SoLuong;
        }
    }
}