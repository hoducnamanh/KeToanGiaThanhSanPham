using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KeToanGiaThanhSanPham.Controllers
{
    public class ChiPhiNhanCongController : Controller
    {
        private static List<BangLuongCongNhan> _bangLuong = new List<BangLuongCongNhan>
        {
            new BangLuongCongNhan { Id=1, MaNV="NV001", HoTen="Nguyễn Văn An", ToSanXuat="Tổ cơ khí", SoNgayCong=26, LuongNgay=350000, PhuCap=500000, TongLuong=9600000, LenhSanXuat="LSX-2025-001", TrangThai="Đã tập hợp" },
            new BangLuongCongNhan { Id=2, MaNV="NV002", HoTen="Trần Thị Bình", ToSanXuat="Tổ lắp ráp", SoNgayCong=24, LuongNgay=320000, PhuCap=400000, TongLuong=8080000, LenhSanXuat="LSX-2025-001", TrangThai="Chưa tập hợp" },
            new BangLuongCongNhan { Id=3, MaNV="NV003", HoTen="Lê Minh Châu", ToSanXuat="Tổ hoàn thiện", SoNgayCong=22, LuongNgay=300000, PhuCap=300000, TongLuong=6900000, LenhSanXuat="LSX-2025-002", TrangThai="Chưa tập hợp" },
        };

        public IActionResult Index()
        {
            return View(_bangLuong);
        }

        [HttpPost]
        public IActionResult TapHop(List<int> selectedIds)
        {
            if (selectedIds != null)
                foreach (var id in selectedIds)
                {
                    var item = _bangLuong.Find(x => x.Id == id);
                    if (item != null) item.TrangThai = "Đã tập hợp";
                }
            TempData["Success"] = $"Đã tập hợp chi phí nhân công cho {selectedIds?.Count ?? 0} công nhân!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult NhapMoi(BangLuongCongNhan model)
        {
            model.Id = _bangLuong.Count + 1;
            model.TongLuong = model.SoNgayCong * model.LuongNgay + model.PhuCap;
            model.TrangThai = "Chưa tập hợp";
            _bangLuong.Add(model);
            TempData["Success"] = "Đã thêm dữ liệu công nhân mới!";
            return RedirectToAction("Index");
        }
    }

    public class BangLuongCongNhan
    {
        public int Id { get; set; }
        public string MaNV { get; set; }
        public string HoTen { get; set; }
        public string ToSanXuat { get; set; }
        public decimal SoNgayCong { get; set; }
        public decimal LuongNgay { get; set; }
        public decimal PhuCap { get; set; }
        public decimal TongLuong { get; set; }
        public string LenhSanXuat { get; set; }
        public string TrangThai { get; set; }
    }
}