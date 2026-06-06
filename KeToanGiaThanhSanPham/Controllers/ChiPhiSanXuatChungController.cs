using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace KeToanGiaThanhSanPham.Controllers
{
    public class ChiPhiSanXuatChungController : Controller
    {
        // Exposed as public static so reports can read sample data
        public static List<ChiPhiSXChung> DanhSach { get; } = new List<ChiPhiSXChung>
        {
            new ChiPhiSXChung { Id=1, LoaiChiPhi="Khấu hao máy móc", MoTa="Khấu hao dây chuyền đúc", SoTien=15000000, TuNgay=new DateTime(2025,6,1), DenNgay=new DateTime(2025,6,30), TrangThai="Đã ghi nhận" },
            new ChiPhiSXChung { Id=2, LoaiChiPhi="Điện sản xuất", MoTa="Tiền điện tháng 6/2025", SoTien=8500000, TuNgay=new DateTime(2025,6,1), DenNgay=new DateTime(2025,6,30), TrangThai="Chưa ghi nhận" },
            new ChiPhiSXChung { Id=3, LoaiChiPhi="Nước sản xuất", MoTa="Tiền nước tháng 6/2025", SoTien=1200000, TuNgay=new DateTime(2025,6,1), DenNgay=new DateTime(2025,6,30), TrangThai="Chưa ghi nhận" },
            new ChiPhiSXChung { Id=4, LoaiChiPhi="Thuê xưởng", MoTa="Thuê mặt bằng phân xưởng A", SoTien=20000000, TuNgay=new DateTime(2025,6,1), DenNgay=new DateTime(2025,6,30), TrangThai="Đã ghi nhận" },
            new ChiPhiSXChung { Id=5, LoaiChiPhi="Lương quản lý PX", MoTa="Lương quản đốc và tổ trưởng", SoTien=12000000, TuNgay=new DateTime(2025,6,1), DenNgay=new DateTime(2025,6,30), TrangThai="Chưa ghi nhận" },
        };

        public IActionResult Index()
        {
            return View(DanhSach);
        }

        [HttpPost]
        public IActionResult GhiNhan(List<int> selectedIds)
        {
            if (selectedIds != null)
                foreach (var id in selectedIds)
                {
                    var item = DanhSach.Find(x => x.Id == id);
                    if (item != null) item.TrangThai = "Đã ghi nhận";
                }
            TempData["Success"] = $"Đã ghi nhận {selectedIds?.Count ?? 0} khoản chi phí sản xuất chung!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult NhapMoi(ChiPhiSXChung model)
        {
            model.Id = DanhSach.Count + 1;
            model.TrangThai = "Chưa ghi nhận";
            DanhSach.Add(model);
            TempData["Success"] = "Đã thêm khoản chi phí SX chung mới!";
            return RedirectToAction("Index");
        }
    }

    public class ChiPhiSXChung
    {
        public int Id { get; set; }
        public string LoaiChiPhi { get; set; }
        public string MoTa { get; set; }
        public decimal SoTien { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public string TrangThai { get; set; }
    }
}