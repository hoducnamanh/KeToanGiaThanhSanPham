using Microsoft.AspNetCore.Mvc;
using KeToanGiaThanhSanPham.Models;
using System;
using System.Collections.Generic;

namespace KeToanGiaThanhSanPham.Controllers
{
    public class ChiPhiNVLController : Controller
    {
        // Dữ liệu mẫu (thay bằng DbContext thực tế)
        private static List<PhieuXuatKhoNVL> _danhSachPhieu = new List<PhieuXuatKhoNVL>
        {
            new PhieuXuatKhoNVL { Id = 1, SoPhieu = "PX001", NgayXuat = new DateTime(2025,6,1), TenNVL = "Thép tấm", DonVi = "kg", SoLuong = 500, DonGia = 25000, ThanhTien = 12500000, LenhSanXuat = "LSX-2025-001", TrangThai = "Đã tập hợp" },
            new PhieuXuatKhoNVL { Id = 2, SoPhieu = "PX002", NgayXuat = new DateTime(2025,6,2), TenNVL = "Nhựa PP", DonVi = "kg", SoLuong = 200, DonGia = 35000, ThanhTien = 7000000, LenhSanXuat = "LSX-2025-001", TrangThai = "Chưa tập hợp" },
            new PhieuXuatKhoNVL { Id = 3, SoPhieu = "PX003", NgayXuat = new DateTime(2025,6,3), TenNVL = "Sơn phủ", DonVi = "lít", SoLuong = 50, DonGia = 120000, ThanhTien = 6000000, LenhSanXuat = "LSX-2025-002", TrangThai = "Chưa tập hợp" },
        };

        public IActionResult Index()
        {
            var model = new ChiPhiNVLViewModel
            {
                DanhSachPhieu = _danhSachPhieu,
                TongChiPhi = 0,
                ThangNam = DateTime.Now.ToString("MM/yyyy")
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult TapHop(List<int> selectedIds, string lenhSanXuat)
        {
            if (selectedIds != null)
            {
                foreach (var id in selectedIds)
                {
                    var phieu = _danhSachPhieu.Find(p => p.Id == id);
                    if (phieu != null) phieu.TrangThai = "Đã tập hợp";
                }
            }
            TempData["Success"] = $"Đã tập hợp {selectedIds?.Count ?? 0} phiếu xuất kho thành công!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult NhapMoi(PhieuXuatKhoNVL model)
        {
            model.Id = _danhSachPhieu.Count + 1;
            model.ThanhTien = model.SoLuong * model.DonGia;
            model.TrangThai = "Chưa tập hợp";
            _danhSachPhieu.Add(model);
            TempData["Success"] = "Đã thêm phiếu xuất kho mới!";
            return RedirectToAction("Index");
        }
    }

    public class ChiPhiNVLViewModel
    {
        public List<PhieuXuatKhoNVL> DanhSachPhieu { get; set; }
        public decimal TongChiPhi { get; set; }
        public string ThangNam { get; set; }
    }

    public class PhieuXuatKhoNVL
    {
        public int Id { get; set; }
        public string SoPhieu { get; set; }
        public DateTime NgayXuat { get; set; }
        public string TenNVL { get; set; }
        public string DonVi { get; set; }
        public decimal SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public string LenhSanXuat { get; set; }
        public string TrangThai { get; set; }
    }
}