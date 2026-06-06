using System;

namespace KeToanGiaThanhSanPham.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string UserId { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Action { get; set; } = ""; // Added, Modified, Deleted
        public string TableName { get; set; } = "";
        public string KeyValues { get; set; } = ""; // JSON
        public string OldValues { get; set; } = ""; // JSON
        public string NewValues { get; set; } = ""; // JSON
        public DateTime TimestampUtc { get; set; }
    }
}