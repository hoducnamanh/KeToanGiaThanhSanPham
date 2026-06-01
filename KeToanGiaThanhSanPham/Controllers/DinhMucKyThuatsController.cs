
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KeToanGiaThanhSanPham.Models;
using KeToanGiaThanhSanPham.Data;

public class DinhMucKyThuatsController : Controller
{
    private readonly ApplicationDbContext _context;

    public DinhMucKyThuatsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: DINHMUCKYTHUATS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.DinhMucKyThuat.ToListAsync());
    }

    // GET: DINHMUCKYTHUATS/Details/5
    public async Task<IActionResult> Details(int? dinhmuckythuatid)
    {
        if (dinhmuckythuatid == null)
        {
            return NotFound();
        }

        var dinhmuckythuat = await _context.DinhMucKyThuat
            .FirstOrDefaultAsync(m => m.DinhMucKyThuatId == dinhmuckythuatid);
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
    public async Task<IActionResult> Create([Bind("DinhMucKyThuatId,SanPhamId,SanPham,NguyenVatLieuId,NguyenVatLieu,SoLuongDinhMuc")] DinhMucKyThuat dinhmuckythuat)
    {
        if (ModelState.IsValid)
        {
            _context.Add(dinhmuckythuat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(dinhmuckythuat);
    }

    // GET: DINHMUCKYTHUATS/Edit/5
    public async Task<IActionResult> Edit(int? dinhmuckythuatid)
    {
        if (dinhmuckythuatid == null)
        {
            return NotFound();
        }

        var dinhmuckythuat = await _context.DinhMucKyThuat.FindAsync(dinhmuckythuatid);
        if (dinhmuckythuat == null)
        {
            return NotFound();
        }
        return View(dinhmuckythuat);
    }

    // POST: DINHMUCKYTHUATS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? dinhmuckythuatid, [Bind("DinhMucKyThuatId,SanPhamId,SanPham,NguyenVatLieuId,NguyenVatLieu,SoLuongDinhMuc")] DinhMucKyThuat dinhmuckythuat)
    {
        if (dinhmuckythuatid != dinhmuckythuat.DinhMucKyThuatId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(dinhmuckythuat);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DinhMucKyThuatExists(dinhmuckythuat.DinhMucKyThuatId))
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
        return View(dinhmuckythuat);
    }

    // GET: DINHMUCKYTHUATS/Delete/5
    public async Task<IActionResult> Delete(int? dinhmuckythuatid)
    {
        if (dinhmuckythuatid == null)
        {
            return NotFound();
        }

        var dinhmuckythuat = await _context.DinhMucKyThuat
            .FirstOrDefaultAsync(m => m.DinhMucKyThuatId == dinhmuckythuatid);
        if (dinhmuckythuat == null)
        {
            return NotFound();
        }

        return View(dinhmuckythuat);
    }

    // POST: DINHMUCKYTHUATS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? dinhmuckythuatid)
    {
        var dinhmuckythuat = await _context.DinhMucKyThuat.FindAsync(dinhmuckythuatid);
        if (dinhmuckythuat != null)
        {
            _context.DinhMucKyThuat.Remove(dinhmuckythuat);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DinhMucKyThuatExists(int? dinhmuckythuatid)
    {
        return _context.DinhMucKyThuat.Any(e => e.DinhMucKyThuatId == dinhmuckythuatid);
    }
}
