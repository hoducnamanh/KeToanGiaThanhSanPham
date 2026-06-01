using System.ComponentModel.DataAnnotations;


namespace KeToanGiaThanhSanPham.Models
{
    public class DinhMucKyThuat
    {
        [Key]
        public int DinhMucKyThuatId { get; set; }
 
        [Display(Name = "SP")]
        public int SanPhamId { get; set; }
        public SanPham SanPham { get; set; }
        
        [Display(Name = "NVL")]
        public int NguyenVatLieuId { get; set; }
        public NguyenVatLieu NguyenVatLieu { get; set; }

        [Required(ErrorMessage = "Không được để trống số lượng định mức")]
        [Display(Name = "SL định mức")]
        public decimal SoLuongDinhMuc { get; set; }
    }
}
