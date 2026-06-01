using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace KeToanGiaThanhSanPham.Models
{
    public class PhanXuong
    {
        [Key]
        public int PhanXuongId { get; set; }

        [Required(ErrorMessage = "Không được để trống mã phân xưởng")]
        [Display(Name = "Mã PX")]
        public string MaPhanXuong { get; set; }

        [Required(ErrorMessage = "Không được để trống tên phân xưởng")]
        [Display(Name = "Mã PX")]
        public string TenPhanXuong { get; set; }
        public ICollection<SanPham> SanPhamCollection { get; set; }
    }
}
