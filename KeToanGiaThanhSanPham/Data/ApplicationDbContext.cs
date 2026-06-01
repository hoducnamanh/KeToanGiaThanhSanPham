using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KeToanGiaThanhSanPham.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
    public DbSet<KeToanGiaThanhSanPham.Models.DinhMucKyThuat> DinhMucKyThuat { get; set; } = default!;
    public DbSet<KeToanGiaThanhSanPham.Models.PhanXuong> PhanXuong { get; set; } = default!;
    public DbSet<KeToanGiaThanhSanPham.Models.NguyenVatLieu> NguyenVatLieu { get; set; } = default!;
    public DbSet<KeToanGiaThanhSanPham.Models.SanPham> SanPham { get; set; } = default!;
    }
}
