using System;

namespace KeToanGiaThanhSanPham.Models
{
    public class AccountingPeriod
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; } // 1..12
        public bool IsClosed { get; set; }
        public string ClosedByUserId { get; set; } = "";
        public string ClosedByUserName { get; set; } = "";
        public DateTime? ClosedAtUtc { get; set; }
    }
}