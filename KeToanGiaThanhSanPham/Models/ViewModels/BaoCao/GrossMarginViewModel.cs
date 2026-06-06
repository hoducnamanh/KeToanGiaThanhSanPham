using System.Collections.Generic;

namespace KeToanGiaThanhSanPham.Models.ViewModels.BaoCao
{
    public class GrossMarginViewModel
    {
        public List<GrossMarginItem> Items { get; set; } = new List<GrossMarginItem>();
        public decimal TotalRevenue { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalGrossProfit => TotalRevenue - TotalCost;
    }

    public class GrossMarginItem
    {
        public int SanPhamId { get; set; }
        public string MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string TenPhanXuong { get; set; }

        // Sales
        public decimal UnitsSold { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Revenue => UnitsSold * UnitPrice;

        // Cost
        public decimal CostPerUnit { get; set; }
        public decimal CostTotal => UnitsSold * CostPerUnit;

        // Result
        public decimal GrossProfit => Revenue - CostTotal;
        public decimal GrossMarginPercent => Revenue != 0 ? (GrossProfit / Revenue) * 100m : 0m;
    }
}