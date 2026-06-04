using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace KeToanGiaThanhSanPham.Models.ViewModels.DinhMucKyThuat
{
    public class DinhMucIndexViewModel
    {
        public int SanPhamId { get; set; }
        public string MaSanPham { get; set; }
        public string TenSanPham { get; set; }

        public List<DinhMucChiTietViewModel> DinhMucKyThuatList { get; set; }

        public DinhMucCreateViewModel NguyenVatLieuInput { get; set; }
    }
    public class DinhMucChiTietViewModel
    {
        public int Id { get; set; }
        public string TenNguyenVatLieu { get; set; }
        public string DonViTinh { get; set; }
        public decimal SoLuongDinhMuc { get; set; }

    }
}
