namespace KeToanGiaThanhSanPham.Models.ViewModels.TinhGiaThanh
{
    public class KetQuaSanPham
    {
        public int SanPhamId { get; set; }
        public string MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string DonViTinh { get; set; }
        public string TenPhanXuong { get; set; }

        public decimal SoLuongHoanThanh { get; set; }
        public decimal SoLuongDoDangCuoiKy { get; set; }
        public decimal TyLeHoanThanhWIP { get; set; }

        // Chi phí NVL trực tiếp
        public decimal ChiPhiNVLTrucTiep { get; set; }

        // Chi phí SXC được phân bổ
        public decimal ChiPhiSXCPhanBo { get; set; }

        // Tổng chi phí
        public decimal TongChiPhi => ChiPhiNVLTrucTiep + ChiPhiSXCPhanBo;

        // Số lượng quy đổi (thành phẩm + WIP quy đổi)
        public decimal SoLuongQuyDoi { get; set; }

        // Tổng giá thành
        public decimal TongGiaThanh { get; set; }

        // Giá thành đơn vị
        public decimal GiaThanhDonVi => SoLuongHoanThanh > 0 ? TongGiaThanh / SoLuongHoanThanh : 0;

        // Chi tiết NVL
        public List<ChiTietNVL> ChiTietNVLList { get; set; } = new();
    }

    public class ChiTietNVL
    {
        public string MaNVL { get; set; }
        public string TenNVL { get; set; }
        public decimal SoLuongDinhMuc { get; set; }
        public decimal SoLuongThucTe { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien => SoLuongThucTe * DonGia;
    }

    public class KetQuaGiaThanhViewModel
    {
        public KyTinhGiaThanhViewModel KyTinh { get; set; }
        public PhuongPhapWIP PhuongPhapWIP { get; set; }
        public TieuThucPhanBo TieuThucPhanBo { get; set; }
        public decimal TongChiPhiSXC { get; set; }
        public List<KetQuaSanPham> KetQuaDanhSach { get; set; } = new();

        public string TenKy => KyTinh?.LoaiKy switch
        {
            LoaiKy.Thang => $"Tháng {KyTinh.Thang}/{KyTinh.Nam}",
            LoaiKy.Quy => $"Quý {KyTinh.Quy}/{KyTinh.Nam}",
            LoaiKy.Nam => $"Năm {KyTinh.Nam}",
            _ => ""
        };

        public string TenPhuongPhapWIP => PhuongPhapWIP switch
        {
            PhuongPhapWIP.TheoNVLChinh => "Theo NVL chính",
            PhuongPhapWIP.TheoTyLeHoanThanh => "Theo tỷ lệ hoàn thành tương đương",
            _ => ""
        };

        public string TenTieuThuc => TieuThucPhanBo switch
        {
            TieuThucPhanBo.TheoNVLTrucTiep => "Chi phí NVL trực tiếp",
            TieuThucPhanBo.TheoGioCong => "Giờ công lao động",
            TieuThucPhanBo.TheoSoLuongSanXuat => "Số lượng sản xuất",
            _ => ""
        };
    }
}