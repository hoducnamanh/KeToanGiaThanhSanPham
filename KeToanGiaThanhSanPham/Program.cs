using KeToanGiaThanhSanPham.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KeToanGiaThanhSanPham.Services;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IAccountingPeriodService, AccountingPeriodService>();

// 1. Cấu hình Identity: Nới lỏng chính sách mật khẩu để cho phép đặt "admin123"
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// 2. Bắt buộc đăng nhập toàn cục (Bất cứ URL nào cũng tự động chuyển hướng về trang Đăng nhập)


builder.Services.AddControllersWithViews();

var app = builder.Build();
// Tự động chạy Migration khi khởi tạo App
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>(); // Thay bằng tên DbContext của bạn
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

// 3. Khởi tạo dữ liệu (Seed Roles & Admin Account)
using (var scope = app.Services.CreateScope())
{

    var services = scope.ServiceProvider;
    var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userMgr = services.GetRequiredService<UserManager<IdentityUser>>();

    // Khởi tạo các nhóm quyền
    string[] roles = new[] { "DataEntry", "ChiefAccountant", "Director" };
    foreach (var role in roles)
    {
        if (!roleMgr.RoleExistsAsync(role).GetAwaiter().GetResult())
            roleMgr.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
    }

    // Khởi tạo tài khoản admin mặc định
    // Sử dụng admin@ketoan.vn để tương thích với field <input type="email"> mặc định của Identity UI
    var adminEmail = "admin@ketoan.vn";
    var adminPassword = "admin123";

    var adminUser = userMgr.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();
    if (adminUser == null)
    {
        adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
        var result = userMgr.CreateAsync(adminUser, adminPassword).GetAwaiter().GetResult();
        if (result.Succeeded)
        {
            userMgr.AddToRoleAsync(adminUser, "Director").GetAwaiter().GetResult();
        }
    }
}

// Configure HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// 4. Bỏ qua xác thực cho các tệp tĩnh (CSS, JS, hình ảnh) để màn hình đăng nhập không bị vỡ giao diện
app.MapStaticAssets().AllowAnonymous();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages().WithStaticAssets();

app.Run();
