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
    public async Task<IActionResult> Index(string searchString, string sortOrder)    
    {
        // Các tùy chọn sắp xếp
        ViewData["CurrentSort"] = sortOrder;
        ViewData["MaPXSort"] = String.IsNullOrEmpty(sortOrder) ? "maPX_desc" : "";
        ViewData["TenPXSort"] = sortOrder == "TenPX" ? "TenPX_desc" : "TenPX";
        ViewData["CurrentFilter"] = searchString;

        var pxQuery = _context.PhanXuong.AsQueryable();

        // Tìm kiếm
        if (!String.IsNullOrEmpty(searchString))
        {
            pxQuery = pxQuery.Where(px =>
                px.MaPhanXuong.Contains(searchString) ||
                px.TenPhanXuong.Contains(searchString)
            );
        }

        // Sắp xếp
        switch (sortOrder)
        {
            case "maPX_desc":
                pxQuery = pxQuery.OrderByDescending(p => p.MaPhanXuong);
                break;
            case "TenPX":
                pxQuery = pxQuery.OrderBy(p => p.TenPhanXuong);
                break;
            case "TenPX_desc":
                pxQuery = pxQuery.OrderByDescending(p => p.TenPhanXuong);
                break;
            default:
                pxQuery = pxQuery.OrderBy(p => p.MaPhanXuong);
                break;
        }

        var phanXuongs = await pxQuery.ToListAsync();
        return View(phanXuongs);
    }

    // GET: PHANXUONGS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var phanxuong = await _context.PhanXuong
            .FirstOrDefaultAsync(m => m.Id == id);
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
    public async Task<IActionResult> Create([Bind("Id,MaPhanXuong,TenPhanXuong,SanPhamCollection")] PhanXuong phanxuong)
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
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var phanxuong = await _context.PhanXuong.FindAsync(id);
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
    public async Task<IActionResult> Edit(int? id, [Bind("Id,MaPhanXuong,TenPhanXuong,SanPhamCollection")] PhanXuong phanxuong)
    {
        if (id != phanxuong.Id)
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
                if (!PhanXuongExists(phanxuong.Id))
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
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var phanxuong = await _context.PhanXuong
            .FirstOrDefaultAsync(m => m.Id == id);
        if (phanxuong == null)
        {
            return NotFound();
        }

        return View(phanxuong);
    }

    // POST: PHANXUONGS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var phanxuong = await _context.PhanXuong.FindAsync(id);
        if (phanxuong != null)
        {
            _context.PhanXuong.Remove(phanxuong);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PhanXuongExists(int? id)
    {
        return _context.PhanXuong.Any(e => e.Id == id);
    }
}
