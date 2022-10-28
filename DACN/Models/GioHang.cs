using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DACN.Models
{
    public class GioHang
    {
        DAChuyenNganhDataContext dataContext = new DAChuyenNganhDataContext();
        public int maSP { set; get; }
        public string tenSP { set; get; }
        /*public int sizeSP { set; get; }*/
        public string anhBia { set; get; }
        public int donGia { set; get; }
        public int soLuong { set; get; }
        public int thanhTien
        {
            get { return soLuong * donGia; }
        }

        public GioHang(int MaSP)
        {
            maSP = MaSP;
            SAN_PHAM sanPham = dataContext.SAN_PHAMs.Single(n => n.MaSP == maSP);
            tenSP = sanPham.TenSP;
            anhBia = sanPham.HinhAnh;
            donGia = int.Parse(sanPham.Gia.ToString());
            soLuong = 1;
        }
    }
}