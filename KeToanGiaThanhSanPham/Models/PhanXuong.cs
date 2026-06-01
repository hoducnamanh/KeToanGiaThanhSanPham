using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace KeToanGiaThanhSanPham.Models
{
    public class PhanXuong
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Không được để trống mã phân xưởng")]
        [Display(Name = "Mã PX")]
        public string MaPhanXuong { get; set; }

        [Required(ErrorMessage = "Không được để trống tên phân xưởng")]
        [Display(Name = "Tên PX")]
        public string TenPhanXuong { get; set; }
        [Display(Name ="Danh mục sản phẩm")]
        public ICollection<SanPham> SanPhamCollection { get; set; } = new List<SanPham>();

        public PhanXuong()
        {
            
        }
    }
}
