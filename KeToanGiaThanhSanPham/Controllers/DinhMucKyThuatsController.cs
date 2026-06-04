
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KeToanGiaThanhSanPham.Models;
using KeToanGiaThanhSanPham.Data;
using KeToanGiaThanhSanPham.Models.ViewModels.DinhMucKyThuat;
using Microsoft.AspNetCore.Mvc.Rendering;

public class DinhMucKyThuatsController : Controller
{
    private readonly ApplicationDbContext _context;

    public DinhMucKyThuatsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: DINHMUCKYTHUATS
    public async Task<IActionResult> Index(int sanPhamId)    
    {
        var sanPham = await _context.SanPham.FindAsync(sanPhamId);
        if (sanPham == null) return NotFound();

        //Map data từ EntityModel -> ViewModel
        var viewModel = new DinhMucIndexViewModel
        {
            SanPhamId = sanPham.Id,
            MaSanPham = sanPham.MaSanPham,
            TenSanPham = sanPham.TenSanPham,

            //Query danh sách định mức hiện có
            DinhMucKyThuatList = await _context.DinhMucKyThuat
                .Where(dm => dm.SanPhamId == sanPhamId)
                .Select(dm => new DinhMucChiTietViewModel
                {
                    Id = dm.Id,
                    TenNguyenVatLieu = dm.NguyenVatLieu.TenNguyenVatLieu,
                    DonViTinh = dm.NguyenVatLieu.DonViTinh,
                    SoLuongDinhMuc = dm.SoLuongDinhMuc
                }).ToListAsync(),
            //Nạp danh sách NVL cho Dropdown
            NguyenVatLieuInput = new DinhMucCreateViewModel
            {
                SanPhamId = sanPham.Id,
                NguyenVatLieuList = await _context.NguyenVatLieu
                    .Select(nvl => new SelectListItem
                    {
                        Value = nvl.Id.ToString(),
                        Text = $"{nvl.TenNguyenVatLieu} ({nvl.DonViTinh})"
                    }).ToListAsync()
            }
        };     
        return View(viewModel);
    }

    // GET: DINHMUCKYTHUATS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var dinhmuckythuat = await _context.DinhMucKyThuat
            .FirstOrDefaultAsync(m => m.Id == id);
        if (dinhmuckythuat == null)
        {
            return NotFound();
        }

        return View(dinhmuckythuat);
    }

    // GET: DINHMUCKYTHUATS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: DINHMUCKYTHUATS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DinhMucCreateViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            //Kiểm tra trùng lặp cặp SP và NVL
            var IsExisted = await _context.DinhMucKyThuat
                .AnyAsync(dm => dm.SanPhamId == viewModel.SanPhamId && dm.NguyenVatLieuId == viewModel.NguyenVatLieuId);

            if (IsExisted)
            {
                TempData["ErrorMessage"] = "Nguyên vật liệu này đã có trong định mức của sản phẩm.";
                return RedirectToAction(nameof(Index), new { sanPhamId = viewModel.SanPhamId });
            }

            //Map data từ ViewModel -> EntityModel
            var dinhMucMoi = new DinhMucKyThuat
            {
                SanPhamId = viewModel.SanPhamId,
                NguyenVatLieuId = viewModel.NguyenVatLieuId,
                SoLuongDinhMuc = viewModel.SoLuong 
            };

            _context.DinhMucKyThuat.Add(dinhMucMoi);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Thêm định mức vật tư thành công!";
            return RedirectToAction(nameof(Index), new { sanPhamId = viewModel.SanPhamId });
        }

        TempData["ErrorMessage"] = "Dữ liệu nhập vào không hợp lệ.";
        return RedirectToAction(nameof(Index), new { sanPhamId = viewModel.SanPhamId });
    }

    // GET: DINHMUCKYTHUATS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        
        //Tìm định mức cần sửa, kèm theo thông tin NVL
        var dinhmuckythuat = await _context.DinhMucKyThuat
            .Include(dm => dm.NguyenVatLieu)
            .FirstOrDefaultAsync(dm => dm.Id == id);

        if (dinhmuckythuat == null) return NotFound();

        var viewModel = new DinhMucEditViewModel
        {
            Id = dinhmuckythuat.Id,
            SanPhamId = dinhmuckythuat.SanPhamId,
            TenNguyenVatLieu = dinhmuckythuat.NguyenVatLieu.TenNguyenVatLieu,
            SoLuong = dinhmuckythuat.SoLuongDinhMuc
        };

        return View(viewModel);
    }

    // POST: DINHMUCKYTHUATS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, DinhMucEditViewModel viewModel)
    {
        if (id != viewModel.Id) return NotFound();

        if (ModelState.IsValid)
        {
            var dinhMuc = await _context.DinhMucKyThuat.FindAsync(id);
            if (dinhMuc == null) return NotFound();

            //Update lại số lượng
            dinhMuc.SoLuongDinhMuc = viewModel.SoLuong;

            _context.Update(dinhMuc);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật định mức thành công!";

            return RedirectToAction(nameof(Index), new { sanPhamId = viewModel.SanPhamId});
        }
        return View(viewModel);
    }

    // GET: DINHMUCKYTHUATS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        
        var dinhMuc = await _context.DinhMucKyThuat
            .FirstOrDefaultAsync(m => m.Id == id);

        if (dinhMuc == null) return NotFound();
        return View(dinhMuc);
    }

    // POST: DINHMUCKYTHUATS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id, int sanPhamId)
    {
        var dinhMuc = await _context.DinhMucKyThuat.FindAsync(id);
        if (dinhMuc != null)
        {
            _context.DinhMucKyThuat.Remove(dinhMuc);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Xóa định mức thành công!";
        }

        return RedirectToAction("Index", "DinhMucKyThuats", new { sanPhamId = sanPhamId });
    }

    private bool DinhMucKyThuatExists(int? id)
    {
        return _context.DinhMucKyThuat.Any(e => e.Id == id);
    }
}
