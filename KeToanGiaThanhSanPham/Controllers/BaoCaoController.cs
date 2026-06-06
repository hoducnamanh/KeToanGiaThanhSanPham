using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KeToanGiaThanhSanPham.Data;
using KeToanGiaThanhSanPham.Models;
using KeToanGiaThanhSanPham.Models.ViewModels.BaoCao;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        // New: Tổng hợp chi phí sản xuất — phân tích theo khoản mục và theo phân xưởng
        public IActionResult TongHopChiPhiIndex()
        {
            var danhSach = KeToanGiaThanhSanPham.Controllers.ChiPhiSanXuatChungController.DanhSach;

            var byKhoan = danhSach
                .GroupBy(d => d.LoaiChiPhi ?? "Không phân loại")
                .Select(g => new GroupItem { Key = g.Key, Total = g.Sum(x => x.SoTien) })
                .OrderByDescending(g => g.Total)
                .ToList();

            var byPX = danhSach
                .GroupBy(d => string.IsNullOrWhiteSpace(d.PhanXuong) ? "Không xác định" : d.PhanXuong)
                .Select(g => new GroupItem { Key = g.Key, Total = g.Sum(x => x.SoTien) })
                .OrderByDescending(g => g.Total)
                .ToList();

            var vm = new TongHopChiPhiViewModel
            {
                ByKhoanMuc = byKhoan,
                ByPhanXuong = byPX,
                TongCong = danhSach.Sum(d => d.SoTien)
            };

            return View(vm);
        }

        // Báo cáo phân tích biến động (Variance Analysis)
        // So sánh Giá thành định mức và Giá thành thực tế (dùng dữ liệu mẫu từ ChiPhiNVLController để lấy giá thực tế)
        public async Task<IActionResult> BienDongChiPhiIndex()
        {
            // Load products + định mức + NVL
            var sanPhams = await _context.SanPham
                .Include(s => s.PhanXuong)
                .Include(s => s.DinhMucKyThuatCollection)
                    .ThenInclude(d => d.NguyenVatLieu)
                .AsNoTracking()
                .ToListAsync();

            // Dữ liệu thực tế: lấy từ danh sách phiếu xuất kho mẫu
            var phieuXuat = KeToanGiaThanhSanPham.Controllers.ChiPhiNVLController.DanhSachPhieu;

            // Tạo map: TenNVL -> average actual unit price (nếu có)
            var actualPriceByNVL = phieuXuat
                .GroupBy(p => p.TenNVL?.Trim() ?? "")
                .ToDictionary(g => g.Key, g => g.Average(p => p.DonGia));

            var vm = new VarianceAnalysisViewModel();

            foreach (var sp in sanPhams)
            {
                decimal standardUnitCost = 0m;
                decimal actualUnitCost = 0m;

                var productEntry = new VarianceProduct
                {
                    SanPhamId = sp.Id,
                    MaSanPham = sp.MaSanPham,
                    TenSanPham = sp.TenSanPham,
                    TenPhanXuong = sp.PhanXuong?.TenPhanXuong ?? "N/A"
                };

                foreach (var dm in sp.DinhMucKyThuatCollection)
                {
                    var nvl = dm.NguyenVatLieu;
                    var tenNvl = nvl?.TenNguyenVatLieu?.Trim() ?? "";
                    decimal stdUnitPrice = nvl?.DonGia ?? 0m;
                    decimal stdQty = dm.SoLuongDinhMuc;

                    decimal actualUnitPrice = stdUnitPrice;
                    if (!string.IsNullOrWhiteSpace(tenNvl) && actualPriceByNVL.TryGetValue(tenNvl, out var ap))
                    {
                        actualUnitPrice = ap;
                    }

                    var nvlItem = new VarianceNVLItem
                    {
                        TenNVL = tenNvl == "" ? (nvl?.MaNguyenVatLieu ?? "N/A") : tenNvl,
                        StandardQtyPerUnit = stdQty,
                        StandardUnitPrice = stdUnitPrice,
                        ActualUnitPrice = actualUnitPrice
                    };

                    productEntry.NVLDetails.Add(nvlItem);

                    standardUnitCost += nvlItem.StandardCost;
                    actualUnitCost += nvlItem.ActualCost;
                }

                productEntry.StandardUnitCost = standardUnitCost;
                productEntry.ActualUnitCost = actualUnitCost;

                vm.Products.Add(productEntry);

                vm.TotalStandard += standardUnitCost;
                vm.TotalActual += actualUnitCost;
            }

            return View(vm);
        }

        // Báo cáo Lãi/Lỗ gộp theo sản phẩm
        public async Task<IActionResult> LaiLoGopIndex()
        {
            // Load products + định mức + NVL
            var sanPhams = await _context.SanPham
                .Include(s => s.PhanXuong)
                .Include(s => s.DinhMucKyThuatCollection)
                    .ThenInclude(d => d.NguyenVatLieu)
                .AsNoTracking()
                .ToListAsync();

            // Sample sales data (mẫu). Thay bằng dữ liệu thực tế nếu có.
            // Key: SanPhamId -> (UnitsSold, UnitPrice)
            var sampleSales = new Dictionary<int, (decimal UnitsSold, decimal UnitPrice)>();
            // If no real sales set, create sample values proportional to Id
            foreach (var sp in sanPhams)
            {
                // Example: UnitsSold = 100 + Id*10, UnitPrice = NVL_cost*1.5 (approx)
                sampleSales[sp.Id] = (UnitsSold: 100 + sp.Id * 10, UnitPrice: 0m);
            }

            // Compute NVL unit cost for each product
            var productNVLUnitCost = new Dictionary<int, decimal>();
            foreach (var sp in sanPhams)
            {
                decimal nvlUnit = sp.DinhMucKyThuatCollection
                    .Sum(d => (d.NguyenVatLieu?.DonGia ?? 0m) * d.SoLuongDinhMuc);
                productNVLUnitCost[sp.Id] = nvlUnit;
                // If sample unit price not set, set to 1.5x NVL unit cost as demo selling price
                var current = sampleSales[sp.Id];
                if (current.UnitPrice == 0m)
                    sampleSales[sp.Id] = (current.UnitsSold, UnitPrice: nvlUnit * 1.5m);
            }

            // Total NVL cost weighted by units sold (for SXC allocation)
            decimal totalNVLWeighted = sanPhams.Sum(sp =>
            {
                var units = sampleSales.TryGetValue(sp.Id, out var s) ? s.UnitsSold : 0m;
                return productNVLUnitCost.GetValueOrDefault(sp.Id) * units;
            });

            // Total SXC available (sample)
            decimal totalSXC = KeToanGiaThanhSanPham.Controllers.ChiPhiSanXuatChungController.DanhSach.Sum(s => s.SoTien);

            var vm = new GrossMarginViewModel();

            foreach (var sp in sanPhams)
            {
                var unitsSold = sampleSales.TryGetValue(sp.Id, out var s) ? s.UnitsSold : 0m;
                var unitPrice = sampleSales.TryGetValue(sp.Id, out s) ? s.UnitPrice : 0m;
                var nvlUnit = productNVLUnitCost.GetValueOrDefault(sp.Id);

                // allocate SXC total to this product proportional to NVL*units
                decimal productNVLTotal = nvlUnit * unitsSold;
                decimal allocatedSXCForProductTotal = 0m;
                if (totalNVLWeighted > 0m)
                {
                    allocatedSXCForProductTotal = totalSXC * (productNVLTotal / totalNVLWeighted);
                }
                decimal sxcPerUnit = unitsSold > 0 ? allocatedSXCForProductTotal / unitsSold : 0m;

                var costPerUnit = nvlUnit + sxcPerUnit;

                var item = new GrossMarginItem
                {
                    SanPhamId = sp.Id,
                    MaSanPham = sp.MaSanPham,
                    TenSanPham = sp.TenSanPham,
                    TenPhanXuong = sp.PhanXuong?.TenPhanXuong ?? "N/A",
                    UnitsSold = unitsSold,
                    UnitPrice = unitPrice,
                    CostPerUnit = costPerUnit
                };

                vm.Items.Add(item);
                vm.TotalRevenue += item.Revenue;
                vm.TotalCost += item.CostTotal;
            }

            return View(vm);
        }
    }
}