using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KeToanGiaThanhSanPham.Models;
using KeToanGiaThanhSanPham.Services;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace KeToanGiaThanhSanPham.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly ICurrentUserService _currentUserService;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService)
            : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<DinhMucKyThuat> DinhMucKyThuat { get; set; } = default!;
        public DbSet<PhanXuong> PhanXuong { get; set; } = default!;
        public DbSet<NguyenVatLieu> NguyenVatLieu { get; set; } = default!;
        public DbSet<SanPham> SanPham { get; set; } = default!;

        // New
        public DbSet<AuditLog> AuditLogs { get; set; } = default!;
        public DbSet<AccountingPeriod> AccountingPeriods { get; set; } = default!;

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is not AuditLog && e.State != EntityState.Unchanged && e.State != EntityState.Detached)
                .ToList();

            foreach (var entry in entries)
            {
                var audit = new AuditLog
                {
                    UserId = _currentUserService.GetUserId(),
                    UserName = _currentUserService.GetUserName(),
                    Action = entry.State.ToString(),
                    TableName = entry.Entity.GetType().Name,
                    TimestampUtc = DateTime.UtcNow
                };

                try
                {
                    var pk = entry.Properties
                        .Where(p => p.Metadata.IsPrimaryKey())
                        .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue);
                    audit.KeyValues = JsonSerializer.Serialize(pk);
                }
                catch
                {
                    audit.KeyValues = "";
                }

                if (entry.State == EntityState.Added)
                {
                    audit.NewValues = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                }
                else if (entry.State == EntityState.Deleted)
                {
                    audit.OldValues = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
                }
                else if (entry.State == EntityState.Modified)
                {
                    audit.OldValues = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
                    audit.NewValues = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                }

                Entry(audit).State = EntityState.Added;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
