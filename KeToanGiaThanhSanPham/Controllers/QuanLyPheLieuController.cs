using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace KeToanGiaThanhSanPham.Controllers
{
    public class QuanLyPheLieuController : Controller
    {
        private static List<PheLieuSPHong> _danhSach = new List<PheLieuSPHong>
        {
            new PheLieuSPHong { Id=1, NgayGhiNhan=new DateTime(2025,6,2), LoaiPhat="Phế liệu", TenVatTu="Vụn thép", SoLuong=50, DonVi="kg", GiaThuHoi=8000, GiaTriThuHoi=400000, LenhSanXuat="LSX-2025-001", HuongXuLy="Bán phế liệu", TrangThai="Đã xử lý" },
            new PheLieuSPHong { Id=2, NgayGhiNhan=new DateTime(2025,6,3), LoaiPhat="SP hỏng", TenVatTu="Bánh răng lỗi", SoLuong=5, DonVi="cái", GiaThuHoi=50000, GiaTriThuHoi=250000, LenhSanXuat="LSX-2025-001", HuongXuLy="Tái chế", TrangThai="Chưa xử lý" },
            new PheLieuSPHong { Id=3, NgayGhiNhan=new DateTime(2025,6,5), LoaiPhat="Phế liệu", TenVatTu="Nhựa vụn", SoLuong=30, DonVi="kg", GiaThuHoi=5000, GiaTriThuHoi=150000, LenhSanXuat="LSX-2025-002", HuongXuLy="Bán phế liệu", TrangThai="Chưa xử lý" },
        };

        public IActionResult Index()
        {
            return View(_danhSach);
        }

        [HttpPost]
        public IActionResult XuLy(List<int> selectedIds, string huongXuLy)
        {
            if (selectedIds != null)
                foreach (var id in selectedIds)
                {
                    var item = _danhSach.Find(x => x.Id == id);
                    if (item != null) { item.TrangThai = "Đã xử lý"; item.HuongXuLy = huongXuLy ?? item.HuongXuLy; }
                }
            TempData["Success"] = $"Đã xử lý {selectedIds?.Count ?? 0} mục phế liệu/SP hỏng! Giá trị thu hồi đã được ghi giảm chi phí.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult NhapMoi(PheLieuSPHong model)
        {
            model.Id = _danhSach.Count + 1;
            model.GiaTriThuHoi = model.SoLuong * model.GiaThuHoi;
            model.TrangThai = "Chưa xử lý";
            _danhSach.Add(model);
            TempData["Success"] = "Đã ghi nhận phế liệu/SP hỏng mới!";
            return RedirectToAction("Index");
        }
    }

    public class PheLieuSPHong
    {
        public int Id { get; set; }
        public DateTime NgayGhiNhan { get; set; }
        public string LoaiPhat { get; set; }
        public string TenVatTu { get; set; }
        public decimal SoLuong { get; set; }
        public string DonVi { get; set; }
        public decimal GiaThuHoi { get; set; }
        public decimal GiaTriThuHoi { get; set; }
        public string LenhSanXuat { get; set; }
        public string HuongXuLy { get; set; }
        public string TrangThai { get; set; }
    }
}