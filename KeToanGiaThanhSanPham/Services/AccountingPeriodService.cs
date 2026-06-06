using KeToanGiaThanhSanPham.Data;
using System.Linq;

namespace KeToanGiaThanhSanPham.Services
{
    public class AccountingPeriodService : IAccountingPeriodService
    {
        private readonly ApplicationDbContext _db;
        public AccountingPeriodService(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool IsPeriodClosed(int year, int month)
        {
            return _db.AccountingPeriods.Any(p => p.Year == year && p.Month == month && p.IsClosed);
        }
    }
}