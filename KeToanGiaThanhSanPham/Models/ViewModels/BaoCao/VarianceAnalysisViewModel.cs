using System.Collections.Generic;

namespace KeToanGiaThanhSanPham.Models.ViewModels.BaoCao
{
    public class VarianceAnalysisViewModel
    {
        public List<VarianceProduct> Products { get; set; } = new List<VarianceProduct>();
        public decimal TotalStandard { get; set; }
        public decimal TotalActual { get; set; }
        public decimal TotalVariance => TotalActual - TotalStandard;
    }

    public class VarianceProduct
    {
        public int SanPhamId { get; set; }
        public string MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string TenPhanXuong { get; set; }
        public decimal StandardUnitCost { get; set; }
        public decimal ActualUnitCost { get; set; }
        public decimal VarianceAmount => ActualUnitCost - StandardUnitCost;
        public decimal VariancePercent => StandardUnitCost != 0 ? (VarianceAmount / StandardUnitCost) * 100m : 0m;

        public List<VarianceNVLItem> NVLDetails { get; set; } = new List<VarianceNVLItem>();
    }

    public class VarianceNVLItem
    {
        public string TenNVL { get; set; }
        public decimal StandardQtyPerUnit { get; set; }
        public decimal StandardUnitPrice { get; set; }
        public decimal StandardCost => StandardQtyPerUnit * StandardUnitPrice;

        // For demo we use actual unit price from warehouse issue slips (ChiPhiNVL)
        public decimal ActualUnitPrice { get; set; }
        public decimal ActualCost => StandardQtyPerUnit * ActualUnitPrice;

        public decimal VarianceCost => ActualCost - StandardCost;
    }
}