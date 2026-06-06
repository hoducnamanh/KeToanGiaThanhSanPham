using System;
using KeToanGiaThanhSanPham.Models; // adjust if your SanPham model lives in another namespace

namespace KeToanGiaThanhSanPham.Models.ViewModels.BaoCao
{
    public class BangTinhGiaThanhViewModel
    {
        // Product model that should expose TenSanPham
        public Models.SanPham? SanPham { get; set; }

        // Cost/properties used by the Razor view
        public decimal ChiPhiNVL { get; set; }
        public decimal ChiPhiNhanCong { get; set; }
        public decimal ChiPhiSXC { get; set; }
        public decimal TongGiaThanhSanXuat { get; set; }
        public int SoLuongSanXuat { get; set; }
        public decimal GiaThanhDonVi { get; set; }
    }
}