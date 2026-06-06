using System.Collections.Generic;

namespace KeToanGiaThanhSanPham.Models.ViewModels.BaoCao
{
    public class TongHopChiPhiViewModel
    {
        // Key = Khoản mục / Tên phân xưởng
        public List<GroupItem> ByKhoanMuc { get; set; } = new List<GroupItem>();
        public List<GroupItem> ByPhanXuong { get; set; } = new List<GroupItem>();
        public decimal TongCong { get; set; }
    }

    public class GroupItem
    {
        public string Key { get; set; }
        public decimal Total { get; set; }
    }
}