using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KeToanGiaThanhSanPham.Models;
using KeToanGiaThanhSanPham.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using KeToanGiaThanhSanPham.Models.ViewModels.SanPham;

public class SanPhamsController : Controller
{
    private readonly ApplicationDbContext _context;

    public SanPhamsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: SANPHAMS
    public async Task<IActionResult> Index(string searchString, string sortOrder)    
    {
        // Các tùy chọn sắp xếp
        ViewData["CurrentSort"] = sortOrder;
        ViewData["MaSanPhamSort"] = String.IsNullOrEmpty(sortOrder) ? "maSanPham_desc" : "";
        ViewData["TenSanPhamSort"] = sortOrder == "TenSanPham" ? "TenSanPham_desc" : "TenSanPham";
        ViewData["CurrentFilter"] = searchString;

        var sanPhamQuery = _context.SanPham
            .Include(sp => sp.PhanXuong)
            .AsQueryable();

        // Tìm kiếm
        if (!String.IsNullOrEmpty(searchString))
        {
            sanPhamQuery = sanPhamQuery.Where(sp =>
                sp.MaSanPham.Contains(searchString) ||
                sp.TenSanPham.Contains(searchString) ||
                sp.DonViTinh.Contains(searchString) ||
                sp.PhanXuong.TenPhanXuong.Contains(searchString)
            );
        }

        // Sắp xếp
        switch (sortOrder)
        {
            case "maSanPham_desc":
                sanPhamQuery = sanPhamQuery.OrderByDescending(s => s.MaSanPham);
                break;
            case "TenSanPham":
                sanPhamQuery = sanPhamQuery.OrderBy(s => s.TenSanPham);
                break;
            case "TenSanPham_desc":
                sanPhamQuery = sanPhamQuery.OrderByDescending(s => s.TenSanPham);
                break;
            default:
                sanPhamQuery = sanPhamQuery.OrderBy(s => s.MaSanPham);
                break;
        }

        var SanPham = await sanPhamQuery.ToListAsync();
        return View(SanPham);
    }

    // GET: SANPHAMS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var sanpham = await _context.SanPham
            .FirstOrDefaultAsync(m => m.Id == id);
        if (sanpham == null)
        {
            return NotFound();
        }

        return View(sanpham);
    }

    // GET: SANPHAMS/Create
    public async Task<IActionResult> Create()
    {
        var viewModel = new SanPhamCreateViewModel
        {   
            //Lấy danh sách phân xưởng từ DB để nạp vào Dropdown
            PhanXuongList = await _context.PhanXuong.Select(px => new SelectListItem
            {
                Value = px.Id.ToString(),
                Text = px.TenPhanXuong
            }).ToListAsync()
        };
        return View(viewModel);
    }

    // POST: SANPHAMS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SanPhamCreateViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var sanpham = new SanPham
            {
                //Map data từ ViewModel -> EntityModel
                MaSanPham = viewModel.MaSanPham,
                TenSanPham = viewModel.TenSanPham,
                DonViTinh = viewModel.DonViTinh,
                PhanXuongId = viewModel.PhanXuongId
            };
            _context.Add(sanpham);
            await _context.SaveChangesAsync();
            //Quay về Index
            return RedirectToAction(nameof(Index));
            //Nạp lại danh sách phân xưởng nếu data invalid
            viewModel.PhanXuongList = await _context.PhanXuong
                .Select(px => new SelectListItem
                {
                    Value = px.Id.ToString(),
                    Text = px.TenPhanXuong
                }).ToListAsync();
        }
        return View(viewModel);
    }

    // GET: SANPHAMS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        //Tìm sản phẩm trong DB theo id
        var sanpham = await _context.SanPham.FindAsync(id);
        if (sanpham == null)
        {
            return NotFound();
        }

        //Map data từ EntityModel -> ViewModel
        var viewModel = new SanPhamEditViewModel
        {
            Id = sanpham.Id,
            MaSanPham = sanpham.MaSanPham,
            TenSanPham = sanpham.TenSanPham,
            DonViTinh = sanpham.DonViTinh,
            PhanXuongId = sanpham.PhanXuongId,
            //Nạp danh sách phân xưởng để đổi phân xưởng
            PhanXuongList = await _context.PhanXuong
                .Select(px => new SelectListItem
                {
                    Value = px.Id.ToString(),
                    Text = px.TenPhanXuong
                }).ToListAsync()
        };
        return View(viewModel);
    }

    // POST: SANPHAMS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, SanPhamEditViewModel viewModel)
    {
        if (id != viewModel.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                //Tìm sản phẩm trong DB theo id
                var sanPham = await _context.SanPham.FindAsync(id);
                if (sanPham == null) return NotFound();
                //Map data từ ViewModel -> EntityModel
                sanPham.MaSanPham = viewModel.MaSanPham;
                sanPham.TenSanPham = viewModel.TenSanPham;
                sanPham.DonViTinh = viewModel.DonViTinh;
                sanPham.PhanXuongId = viewModel.PhanXuongId;
                //Lưu
                _context.Update(sanPham);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SanPhamExists(viewModel.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }

        //Nạp lại danh sách phân xưởng nếu data invalid
        viewModel.PhanXuongList = await _context.PhanXuong
            .Select(px => new SelectListItem
            {
                Value = px.Id.ToString(),
                Text = px.TenPhanXuong
            }).ToListAsync();

        return View(viewModel);
    }

    // GET: SANPHAMS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var sanpham = await _context.SanPham
            .FirstOrDefaultAsync(m => m.Id == id);
        if (sanpham == null)
        {
            return NotFound();
        }

        return View(sanpham);
    }

    // POST: SANPHAMS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var sanpham = await _context.SanPham.FindAsync(id);
        if (sanpham != null)
        {
            _context.SanPham.Remove(sanpham);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SanPhamExists(int? id)
    {
        return _context.SanPham.Any(e => e.Id == id);
    }
}
