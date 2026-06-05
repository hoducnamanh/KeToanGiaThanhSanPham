using System.ComponentModel.DataAnnotations;

namespace KeToanGiaThanhSanPham.Models.ViewModels.TinhGiaThanh
{
    public enum LoaiKy { Thang = 1, Quy = 2, Nam = 3 }

    public enum PhuongPhapWIP
    {
        TheoNVLChinh = 1,
        TheoTyLeHoanThanh = 2
    }

    public enum TieuThucPhanBo
    {
        TheoNVLTrucTiep = 1,
        TheoGioCong = 2,
        TheoSoLuongSanXuat = 3
    }

    public class KyTinhGiaThanhViewModel
    {
        [Required(ErrorMessage = "Vui lòng chọn loại kỳ")]
        [Display(Name = "Loại kỳ")]
        public LoaiKy LoaiKy { get; set; } = LoaiKy.Thang;

        [Display(Name = "Tháng")]
        [Range(1, 12, ErrorMessage = "Tháng phải từ 1-12")]
        public int? Thang { get; set; }

        [Display(Name = "Quý")]
        [Range(1, 4, ErrorMessage = "Quý phải từ 1-4")]
        public int? Quy { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập năm")]
        [Display(Name = "Năm")]
        [Range(2000, 2100, ErrorMessage = "Năm không hợp lệ")]
        public int Nam { get; set; } = DateTime.Now.Year;
    }
}