
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
    public async Task<IActionResult> Index()    
    {
        var SanPham = await _context.SanPham
            .Include(sp => sp.PhanXuong)
            .ToListAsync();

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
                MaSanPham = viewModel.MaSanPham,
                TenSanPham = viewModel.TenSanPham,
                DonViTinh = viewModel.DonViTinh,
                PhanXuongId = viewModel.PhanXuongId
            };
            _context.Add(sanpham);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

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

        var sanpham = await _context.SanPham.FindAsync(id);
        if (sanpham == null)
        {
            return NotFound();
        }

        var viewModel = new SanPhamEditViewModel
        {
            Id = sanpham.Id,
            MaSanPham = sanpham.MaSanPham,
            TenSanPham = sanpham.TenSanPham,
            DonViTinh = sanpham.DonViTinh,
            PhanXuongId = sanpham.PhanXuongId,

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
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var sanPham = await _context.SanPham.FindAsync(id);
                if (sanPham == null) return NotFound();

                sanPham.MaSanPham = viewModel.MaSanPham;
                sanPham.TenSanPham = viewModel.TenSanPham;
                sanPham.DonViTinh = viewModel.DonViTinh;
                sanPham.PhanXuongId = viewModel.PhanXuongId;

                _context.Update(sanPham);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SanPhamExists(viewModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

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
