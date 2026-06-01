
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KeToanGiaThanhSanPham.Models;
using KeToanGiaThanhSanPham.Data;

public class PhanXuongsController : Controller
{
    private readonly ApplicationDbContext _context;

    public PhanXuongsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: PHANXUONGS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.PhanXuong.ToListAsync());
    }

    // GET: PHANXUONGS/Details/5
    public async Task<IActionResult> Details(int? phanxuongid)
    {
        if (phanxuongid == null)
        {
            return NotFound();
        }

        var phanxuong = await _context.PhanXuong
            .FirstOrDefaultAsync(m => m.PhanXuongId == phanxuongid);
        if (phanxuong == null)
        {
            return NotFound();
        }

        return View(phanxuong);
    }

    // GET: PHANXUONGS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PHANXUONGS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PhanXuongId,MaPhanXuong,TenPhanXuong,SanPhamCollection")] PhanXuong phanxuong)
    {
        if (ModelState.IsValid)
        {
            _context.Add(phanxuong);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(phanxuong);
    }

    // GET: PHANXUONGS/Edit/5
    public async Task<IActionResult> Edit(int? phanxuongid)
    {
        if (phanxuongid == null)
        {
            return NotFound();
        }

        var phanxuong = await _context.PhanXuong.FindAsync(phanxuongid);
        if (phanxuong == null)
        {
            return NotFound();
        }
        return View(phanxuong);
    }

    // POST: PHANXUONGS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? phanxuongid, [Bind("PhanXuongId,MaPhanXuong,TenPhanXuong,SanPhamCollection")] PhanXuong phanxuong)
    {
        if (phanxuongid != phanxuong.PhanXuongId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(phanxuong);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhanXuongExists(phanxuong.PhanXuongId))
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
        return View(phanxuong);
    }

    // GET: PHANXUONGS/Delete/5
    public async Task<IActionResult> Delete(int? phanxuongid)
    {
        if (phanxuongid == null)
        {
            return NotFound();
        }

        var phanxuong = await _context.PhanXuong
            .FirstOrDefaultAsync(m => m.PhanXuongId == phanxuongid);
        if (phanxuong == null)
        {
            return NotFound();
        }

        return View(phanxuong);
    }

    // POST: PHANXUONGS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? phanxuongid)
    {
        var phanxuong = await _context.PhanXuong.FindAsync(phanxuongid);
        if (phanxuong != null)
        {
            _context.PhanXuong.Remove(phanxuong);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PhanXuongExists(int? phanxuongid)
    {
        return _context.PhanXuong.Any(e => e.PhanXuongId == phanxuongid);
    }
}
