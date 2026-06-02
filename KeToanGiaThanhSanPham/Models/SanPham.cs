using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace KeToanGiaThanhSanPham.Models
{
    public class SanPham
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Không được để trống mã sản phẩm")]
        [Display(Name ="Mã SP")]
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
        public PhanXuong PhanXuong { get; set; } = new PhanXuong();
        public ICollection<DinhMucKyThuat> DinhMucKyThuatCollection { get; set; } = new List<DinhMucKyThuat>();

        public SanPham()
        {
            
        }

    }
}
