using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace KeToanGiaThanhSanPham.Models.ViewModels
{
    public class SanPhamCreateViewModel
    {
        [Required(ErrorMessage = "Không được để trống mã sản phẩm")]
        [Display(Name = "Mã SP")]
        public string MaSanPham { get; set; }

        [Required(ErrorMessage = "Không được để trống tên sản phẩm")]
        [Display(Name = "Tên SP")]
        public string TenSanPham { get; set; }

        [Required(ErrorMessage = "Không được để trống đơn vị tính")]
        [Display(Name = "ĐVT")]
        public string DonViTinh { get; set; }

        [Required(ErrorMessage = "Không được để trống phân xưởng")]
        [Display(Name = "Phân xưởng")]
        public int PhanXuongId { get; set; }
        public List<SelectListItem>? PhanXuongList { get; set; }
    }
}
