using System.ComponentModel.DataAnnotations;

namespace KeToanGiaThanhSanPham.Models.ViewModels.DinhMucKyThuat
{
    public class DinhMucEditViewModel
    {
        public int Id { get; set; }
        public int SanPhamId { get; set; }

        [Display(Name = "Tên NVL")]
        public string TenNguyenVatLieu { get; set; }

        [Required(ErrorMessage = "Không được để trống số lượng định mức mới")]
        [Range(0, double.MaxValue, ErrorMessage = "Số lượng định mức mới phải lớn hơn 0")]
        [Display(Name ="Số lượng định mức mới")]
        public decimal SoLuong { get; set; }
    }
}
