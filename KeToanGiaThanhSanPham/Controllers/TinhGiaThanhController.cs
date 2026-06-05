using KeToanGiaThanhSanPham.Data;
using KeToanGiaThanhSanPham.Models;
using KeToanGiaThanhSanPham.Models.ViewModels.TinhGiaThanh;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KeToanGiaThanhSanPham.Controllers
{
    public class TinhGiaThanhController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TinhGiaThanhController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Bước 1: Khai báo kỳ tính giá thành
        [HttpGet]
        public IActionResult Index()
        {
            var vm = new KyTinhGiaThanhViewModel
            {
                LoaiKy = LoaiKy.Thang,
                Thang = DateTime.Now.Month,
                Nam = DateTime.Now.Year
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KhaiBaoKy(KyTinhGiaThanhViewModel model)
        {
            if (model.LoaiKy == LoaiKy.Thang && (model.Thang == null || model.Thang < 1 || model.Thang > 12))
                ModelState.AddModelError("Thang", "Vui lòng chọn tháng hợp lệ");
            if (model.LoaiKy == LoaiKy.Quy && (model.Quy == null || model.Quy < 1 || model.Quy > 4))
                ModelState.AddModelError("Quy", "Vui lòng chọn quý hợp lệ");

            if (!ModelState.IsValid)
                return View("Index", model);

            // Lấy danh sách sản phẩm để nhập WIP
            var sanPhams = await _context.SanPham
                .Include(s => s.PhanXuong)
                .Include(s => s.DinhMucKyThuatCollection)
                    .ThenInclude(d => d.NguyenVatLieu)
                .ToListAsync();

            var wipVm = new WIPViewModel
            {
                KyTinh = model,
                PhuongPhap = PhuongPhapWIP.TheoNVLChinh,
                DanhSachWIP = sanPhams.Select(s => new WIPItem
                {
                    SanPhamId = s.Id,
                    MaSanPham = s.MaSanPham,
                    TenSanPham = s.TenSanPham,
                    DonViTinh = s.DonViTinh,
                    SoLuongDoDang = 0,
                    TyLeHoanThanh = 100
                }).ToList()
            };

            return View("NhapWIP", wipVm);
        }

        // Bước 2: Đánh giá sản phẩm dở dang
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NhapWIP(WIPViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Lấy danh sách sản phẩm để xây dựng tiêu thức
            var sanPhams = await _context.SanPham.ToListAsync();

            var phanBoVm = new PhanBoChiPhiViewModel
            {
                KyTinh = model.KyTinh,
                WIPData = model,
                TieuThuc = TieuThucPhanBo.TheoNVLTrucTiep,
                ChiTietTieuThuc = sanPhams.Select(s => new SanPhamTieuThuc
                {
                    SanPhamId = s.Id,
                    TenSanPham = s.TenSanPham,
                    GioCongMoiSP = 0,
                    SoLuongSanXuat = 0
                }).ToList()
            };

            return View("NhapPhanBoChiPhi", phanBoVm);
        }

        // Bước 3: Tiêu thức phân bổ và chi phí chung
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NhapPhanBoChiPhi(PhanBoChiPhiViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var ketQua = await TinhToanGiaThanh(model);
            return View("KetQua", ketQua);
        }

        // Thuật toán tính giá thành
        private async Task<KetQuaGiaThanhViewModel> TinhToanGiaThanh(PhanBoChiPhiViewModel model)
        {
            var sanPhams = await _context.SanPham
                .Include(s => s.PhanXuong)
                .Include(s => s.DinhMucKyThuatCollection)
                    .ThenInclude(d => d.NguyenVatLieu)
                .ToListAsync();

            decimal tongChiPhiSXC = model.DanhSachChiPhi.Sum(c => c.SoTien);

            var ketQuaList = new List<KetQuaSanPham>();

            foreach (var sp in sanPhams)
            {
                var wipItem = model.WIPData.DanhSachWIP
                    .FirstOrDefault(w => w.SanPhamId == sp.Id);

                decimal slDoDang = wipItem?.SoLuongDoDang ?? 0;
                decimal tyLeHT = (wipItem?.TyLeHoanThanh ?? 100) / 100m;

                // Chi tiết NVL
                var chiTietNVLList = sp.DinhMucKyThuatCollection.Select(d => new ChiTietNVL
                {
                    MaNVL = d.NguyenVatLieu?.MaNguyenVatLieu ?? "",
                    TenNVL = d.NguyenVatLieu?.TenNguyenVatLieu ?? "",
                    SoLuongDinhMuc = d.SoLuongDinhMuc,
                    DonGia = d.NguyenVatLieu?.DonGia ?? 0,
                    SoLuongThucTe = d.SoLuongDinhMuc // Dùng định mức như số thực tế (demo)
                }).ToList();

                // --- Tính số lượng quy đổi ---
                decimal slQuyDoi;
                decimal chiPhiNVLWIP;

                // Giả định sản lượng hoàn thành = Nhập từ model nếu có,
                // demo: dùng 100 sp làm sản lượng hoàn thành mẫu
                decimal slHoanThanh = 100;

                if (model.WIPData.PhuongPhap == PhuongPhapWIP.TheoNVLChinh)
                {
                    // SL quy đổi = SL hoàn thành + SL dở dang (NVL đã xuất đủ)
                    slQuyDoi = slHoanThanh + slDoDang;

                    // Chi phí NVL cho WIP = NVL đầy đủ (100%)
                    chiPhiNVLWIP = chiTietNVLList.Sum(n => n.ThanhTien) * slDoDang;
                }
                else // TheoTyLeHoanThanh
                {
                    // SL quy đổi = SL hoàn thành + SL dở dang * tỷ lệ HT
                    slQuyDoi = slHoanThanh + slDoDang * tyLeHT;

                    // Chi phí NVL cho WIP = NVL * tỷ lệ HT
                    chiPhiNVLWIP = chiTietNVLList.Sum(n => n.ThanhTien) * slDoDang * tyLeHT;
                }

                // Tổng chi phí NVL trực tiếp cho kỳ
                decimal donGiaNVLMoiSP = chiTietNVLList.Sum(n => n.ThanhTien);
                decimal chiPhiNVLTrucTiep = donGiaNVLMoiSP * slQuyDoi;

                ketQuaList.Add(new KetQuaSanPham
                {
                    SanPhamId = sp.Id,
                    MaSanPham = sp.MaSanPham,
                    TenSanPham = sp.TenSanPham,
                    DonViTinh = sp.DonViTinh,
                    TenPhanXuong = sp.PhanXuong?.TenPhanXuong ?? "N/A",
                    SoLuongHoanThanh = slHoanThanh,
                    SoLuongDoDangCuoiKy = slDoDang,
                    TyLeHoanThanhWIP = (wipItem?.TyLeHoanThanh ?? 100),
                    SoLuongQuyDoi = slQuyDoi,
                    ChiPhiNVLTrucTiep = chiPhiNVLTrucTiep,
                    ChiTietNVLList = chiTietNVLList
                });
            }

            // --- Phân bổ chi phí SXC ---
            PhanBoChiPhiSXC(ketQuaList, tongChiPhiSXC, model);

            // --- Tính tổng giá thành ---
            foreach (var kq in ketQuaList)
            {
                // Tổng giá thành = Chi phí NVL thành phẩm + Chi phí SXC phân bổ
                decimal chiPhiNVLThanhPham = kq.SoLuongHoanThanh > 0 && kq.SoLuongQuyDoi > 0
                    ? kq.ChiPhiNVLTrucTiep * (kq.SoLuongHoanThanh / kq.SoLuongQuyDoi)
                    : 0;
                kq.TongGiaThanh = chiPhiNVLThanhPham + kq.ChiPhiSXCPhanBo;
            }

            return new KetQuaGiaThanhViewModel
            {
                KyTinh = model.KyTinh,
                PhuongPhapWIP = model.WIPData.PhuongPhap,
                TieuThucPhanBo = model.TieuThuc,
                TongChiPhiSXC = tongChiPhiSXC,
                KetQuaDanhSach = ketQuaList
            };
        }

        private void PhanBoChiPhiSXC(
            List<KetQuaSanPham> ketQuaList,
            decimal tongChiPhiSXC,
            PhanBoChiPhiViewModel model)
        {
            if (tongChiPhiSXC == 0 || !ketQuaList.Any()) return;

            switch (model.TieuThuc)
            {
                case TieuThucPhanBo.TheoNVLTrucTiep:
                    {
                        decimal tongNVL = ketQuaList.Sum(k => k.ChiPhiNVLTrucTiep);
                        if (tongNVL == 0) break;
                        foreach (var kq in ketQuaList)
                            kq.ChiPhiSXCPhanBo = tongChiPhiSXC * (kq.ChiPhiNVLTrucTiep / tongNVL);
                        break;
                    }

                case TieuThucPhanBo.TheoGioCong:
                    {
                        var chiTiet = model.ChiTietTieuThuc;
                        decimal tongGioCong = chiTiet.Sum(c =>
                        {
                            var kq = ketQuaList.FirstOrDefault(k => k.SanPhamId == c.SanPhamId);
                            return c.GioCongMoiSP * (kq?.SoLuongQuyDoi ?? 0);
                        });
                        if (tongGioCong == 0) break;
                        foreach (var kq in ketQuaList)
                        {
                            var ct = chiTiet.FirstOrDefault(c => c.SanPhamId == kq.SanPhamId);
                            decimal gioCong = (ct?.GioCongMoiSP ?? 0) * kq.SoLuongQuyDoi;
                            kq.ChiPhiSXCPhanBo = tongChiPhiSXC * (gioCong / tongGioCong);
                        }
                        break;
                    }

                case TieuThucPhanBo.TheoSoLuongSanXuat:
                    {
                        var chiTiet = model.ChiTietTieuThuc;
                        decimal tongSL = chiTiet.Sum(c => c.SoLuongSanXuat);
                        if (tongSL == 0) // fallback dùng SL quy đổi
                        {
                            tongSL = ketQuaList.Sum(k => k.SoLuongQuyDoi);
                            if (tongSL == 0) break;
                            foreach (var kq in ketQuaList)
                                kq.ChiPhiSXCPhanBo = tongChiPhiSXC * (kq.SoLuongQuyDoi / tongSL);
                        }
                        else
                        {
                            foreach (var kq in ketQuaList)
                            {
                                var ct = chiTiet.FirstOrDefault(c => c.SanPhamId == kq.SanPhamId);
                                kq.ChiPhiSXCPhanBo = tongChiPhiSXC * ((ct?.SoLuongSanXuat ?? 0) / tongSL);
                            }
                        }
                        break;
                    }
            }
        }
    }
}