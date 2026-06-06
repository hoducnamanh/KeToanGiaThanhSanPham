using System;

namespace KeToanGiaThanhSanPham.Services
{
    public interface IAccountingPeriodService
    {
        bool IsPeriodClosed(int year, int month);
    }
}