using System.ComponentModel.DataAnnotations;

namespace KeToanGiaThanhSanPham.Models.ViewModels.TinhGiaThanh
{
    public class WIPItem
    {
        public int SanPhamId { get; set; }
        public string MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string DonViTinh { get; set; }

        [Display(Name = "SL dở dang cuối kỳ")]
        [Range(0, double.MaxValue, ErrorMessage = "Số lượng không hợp lệ")]
        public decimal SoLuongDoDang { get; set; }

        [Display(Name = "Mức độ hoàn thành (%)")]
        [Range(0, 100, ErrorMessage = "Tỷ lệ từ 0-100")]
        public decimal TyLeHoanThanh { get; set; }
    }

    public class WIPViewModel
    {
        public KyTinhGiaThanhViewModel KyTinh { get; set; }

        [Required]
        public PhuongPhapWIP PhuongPhap { get; set; } = PhuongPhapWIP.TheoNVLChinh;

        public List<WIPItem> DanhSachWIP { get; set; } = new();
    }
}