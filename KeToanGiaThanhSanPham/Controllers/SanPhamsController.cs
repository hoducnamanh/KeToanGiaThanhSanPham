
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KeToanGiaThanhSanPham.Models;
using KeToanGiaThanhSanPham.Data;

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
        return View(await _context.SanPham.ToListAsync());
    }

    // GET: SANPHAMS/Details/5
    public async Task<IActionResult> Details(int? sanphamid)
    {
        if (sanphamid == null)
        {
            return NotFound();
        }

        var sanpham = await _context.SanPham
            .FirstOrDefaultAsync(m => m.SanPhamId == sanphamid);
        if (sanpham == null)
        {
            return NotFound();
        }

        return View(sanpham);
    }

    // GET: SANPHAMS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: SANPHAMS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("SanPhamId,MaSanPham,TenSanPham,DonViTinh,PhanXuongId,PhanXuong,DinhMucKyThuatCollection")] SanPham sanpham)
    {
        if (ModelState.IsValid)
        {
            _context.Add(sanpham);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(sanpham);
    }

    // GET: SANPHAMS/Edit/5
    public async Task<IActionResult> Edit(int? sanphamid)
    {
        if (sanphamid == null)
        {
            return NotFound();
        }

        var sanpham = await _context.SanPham.FindAsync(sanphamid);
        if (sanpham == null)
        {
            return NotFound();
        }
        return View(sanpham);
    }

    // POST: SANPHAMS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? sanphamid, [Bind("SanPhamId,MaSanPham,TenSanPham,DonViTinh,PhanXuongId,PhanXuong,DinhMucKyThuatCollection")] SanPham sanpham)
    {
        if (sanphamid != sanpham.SanPhamId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(sanpham);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SanPhamExists(sanpham.SanPhamId))
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
        return View(sanpham);
    }

    // GET: SANPHAMS/Delete/5
    public async Task<IActionResult> Delete(int? sanphamid)
    {
        if (sanphamid == null)
        {
            return NotFound();
        }

        var sanpham = await _context.SanPham
            .FirstOrDefaultAsync(m => m.SanPhamId == sanphamid);
        if (sanpham == null)
        {
            return NotFound();
        }

        return View(sanpham);
    }

    // POST: SANPHAMS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? sanphamid)
    {
        var sanpham = await _context.SanPham.FindAsync(sanphamid);
        if (sanpham != null)
        {
            _context.SanPham.Remove(sanpham);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SanPhamExists(int? sanphamid)
    {
        return _context.SanPham.Any(e => e.SanPhamId == sanphamid);
    }
}
