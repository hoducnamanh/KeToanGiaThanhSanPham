using System.ComponentModel.DataAnnotations;

namespace KeToanGiaThanhSanPham.Models.ViewModels.TinhGiaThanh
{
    public class ChiPhiSanXuatChung
    {
        [Display(Name = "Tên khoản chi phí")]
        [Required(ErrorMessage = "Vui lòng nhập tên khoản chi phí")]
        public string TenChiPhi { get; set; } = string.Empty;

        [Display(Name = "Số tiền (VNĐ)")]
        [Range(0, double.MaxValue, ErrorMessage = "Số tiền không hợp lệ")]
        public decimal SoTien { get; set; }
    }

    public class PhanBoChiPhiViewModel
    {
        public KyTinhGiaThanhViewModel KyTinh { get; set; }
        public WIPViewModel WIPData { get; set; }

        [Required]
        public TieuThucPhanBo TieuThuc { get; set; } = TieuThucPhanBo.TheoNVLTrucTiep;

        public List<ChiPhiSanXuatChung> DanhSachChiPhi { get; set; } = new()
        {
            new ChiPhiSanXuatChung { TenChiPhi = "Chi phí nhân công gián tiếp" },
            new ChiPhiSanXuatChung { TenChiPhi = "Chi phí khấu hao máy móc" },
            new ChiPhiSanXuatChung { TenChiPhi = "Chi phí điện, nước" },
            new ChiPhiSanXuatChung { TenChiPhi = "Chi phí bảo trì, sửa chữa" },
        };

        // Thông tin bổ sung cho tiêu thức theo giờ công / số lượng
        public List<SanPhamTieuThuc> ChiTietTieuThuc { get; set; } = new();
    }

    public class SanPhamTieuThuc
    {
        public int SanPhamId { get; set; }
        public string TenSanPham { get; set; }

        [Display(Name = "Giờ công / SP")]
        [Range(0, double.MaxValue)]
        public decimal GioCongMoiSP { get; set; }

        [Display(Name = "SL sản xuất")]
        [Range(0, double.MaxValue)]
        public decimal SoLuongSanXuat { get; set; }
    }
}