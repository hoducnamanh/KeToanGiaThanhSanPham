using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace KeToanGiaThanhSanPham.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "system";
        }

        public string GetUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system";
        }
    }
}