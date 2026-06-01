
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KeToanGiaThanhSanPham.Models;
using KeToanGiaThanhSanPham.Data;

public class NguyenVatLieusController : Controller
{
    private readonly ApplicationDbContext _context;

    public NguyenVatLieusController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: NGUYENVATLIEUS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.NguyenVatLieu.ToListAsync());
    }

    // GET: NGUYENVATLIEUS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var nguyenvatlieu = await _context.NguyenVatLieu
            .FirstOrDefaultAsync(m => m.Id == id);
        if (nguyenvatlieu == null)
        {
            return NotFound();
        }

        return View(nguyenvatlieu);
    }

    // GET: NGUYENVATLIEUS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: NGUYENVATLIEUS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,MaNguyenVatLieu,TenNguyenVatLieu,DonViTinh,DonGia")] NguyenVatLieu nguyenvatlieu)
    {
        if (ModelState.IsValid)
        {
            _context.Add(nguyenvatlieu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(nguyenvatlieu);
    }

    // GET: NGUYENVATLIEUS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var nguyenvatlieu = await _context.NguyenVatLieu.FindAsync(id);
        if (nguyenvatlieu == null)
        {
            return NotFound();
        }
        return View(nguyenvatlieu);
    }

    // POST: NGUYENVATLIEUS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,MaNguyenVatLieu,TenNguyenVatLieu,DonViTinh,DonGia")] NguyenVatLieu nguyenvatlieu)
    {
        if (id != nguyenvatlieu.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(nguyenvatlieu);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NguyenVatLieuExists(nguyenvatlieu.Id))
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
        return View(nguyenvatlieu);
    }

    // GET: NGUYENVATLIEUS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var nguyenvatlieu = await _context.NguyenVatLieu
            .FirstOrDefaultAsync(m => m.Id == id);
        if (nguyenvatlieu == null)
        {
            return NotFound();
        }

        return View(nguyenvatlieu);
    }

    // POST: NGUYENVATLIEUS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var nguyenvatlieu = await _context.NguyenVatLieu.FindAsync(id);
        if (nguyenvatlieu != null)
        {
            _context.NguyenVatLieu.Remove(nguyenvatlieu);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool NguyenVatLieuExists(int? id)
    {
        return _context.NguyenVatLieu.Any(e => e.Id == id);
    }
}
