using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace KeToanGiaThanhSanPham.Models
{
    public class NguyenVatLieu
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Không được để trống mã nguyên vật liệu")]
        [Display(Name = "Mã NVL")]
        public string MaNguyenVatLieu { get; set; }

        [Required(ErrorMessage = "Không được để trống tên nguyên vật liệu")]
        [Display(Name = "Tên NVL")]
        public string TenNguyenVatLieu { get; set; }

        [Required(ErrorMessage = "Không được để trống đơn vị tính")]
        [Display(Name = "ĐVT")]
        public string DonViTinh { get; set; }

        [Required(ErrorMessage = "Không được để trống đơn giá NVL")]
        [Display(Name = "Đơn giá NVL")]
        public decimal DonGia { get; set; }

        public NguyenVatLieu()
        {
            
        }
    }
}
