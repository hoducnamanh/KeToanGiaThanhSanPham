using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace KeToanGiaThanhSanPham.Models.ViewModels.DinhMucKyThuat
{
    public class DinhMucCreateViewModel
    {
        public int SanPhamId { get; set; }

        [Required(ErrorMessage = "Không được để trống nguyên vật liệu")]
        public int NguyenVatLieuId { get; set; }

        [Required(ErrorMessage = "Không được để trống số lượng")]
        [Range(0, double.MaxValue, ErrorMessage ="Số lượng định mức phải lớn hơn 0")]
        [Display(Name ="Số lượng định mức")]
        public decimal SoLuong { get; set; }
        public List<SelectListItem>? NguyenVatLieuList { get; set; }
    }
}
