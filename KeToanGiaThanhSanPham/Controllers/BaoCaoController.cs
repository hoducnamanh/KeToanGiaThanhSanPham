using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KeToanGiaThanhSanPham.Data;
using KeToanGiaThanhSanPham.Models;
using KeToanGiaThanhSanPham.Models.ViewModels.BaoCao;
using System.Linq;
using System.Threading.Tasks;

namespace KeToanGiaThanhSanPham.Controllers
{
    public class BaoCaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BaoCaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Show product list for user to select product to view detailed cost composition
        public async Task<IActionResult> BangTinhGiaThanhIndex()
        {
            var sanPhams = await _context.SanPham
                .AsNoTracking()
                .ToListAsync();
            return View(sanPhams);
        }

        // Show cost breakdown for one product (using available sample data)
        public async Task<IActionResult> BangTinhGiaThanhChiTiet(int sanPhamId)
        {
            var sanPham = await _context.SanPham.FindAsync(sanPhamId);
            if (sanPham == null) return NotFound();

            // Aggregate costs from sample in-memory controllers (made public static)
            var chiPhiNVL = KeToanGiaThanhSanPham.Controllers.ChiPhiNVLController.DanhSachPhieu.Sum(p => p.ThanhTien);
            var chiPhiNC = KeToanGiaThanhSanPham.Controllers.ChiPhiNhanCongController.BangLuong.Sum(b => b.TongLuong);
            var chiPhiSXC = KeToanGiaThanhSanPham.Controllers.ChiPhiSanXuatChungController.DanhSach.Sum(s => s.SoTien);

            // If SanPham had quantity, use it; current model lacks SoLuongSanXuat -> default to 1
            int soLuongSanXuat = 1;

            var vm = new BangTinhGiaThanhViewModel
            {
                SanPham = sanPham,
                ChiPhiNVL = chiPhiNVL,
                ChiPhiNhanCong = chiPhiNC,
                ChiPhiSXC = chiPhiSXC,
                TongGiaThanhSanXuat = chiPhiNVL + chiPhiNC + chiPhiSXC,
                SoLuongSanXuat = soLuongSanXuat
            };

            vm.GiaThanhDonVi = vm.SoLuongSanXuat > 0 ? vm.TongGiaThanhSanXuat / vm.SoLuongSanXuat : 0m;

            return View(vm);
        }
    }
}