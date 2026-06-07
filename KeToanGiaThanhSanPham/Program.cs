using KeToanGiaThanhSanPham.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KeToanGiaThanhSanPham.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IAccountingPeriodService, AccountingPeriodService>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed roles and an initial admin user (synchronous calls)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userMgr = services.GetRequiredService<UserManager<IdentityUser>>();

    string[] roles = new[] { "DataEntry", "ChiefAccountant", "Director" };
    foreach (var role in roles)
    {
        if (!roleMgr.RoleExistsAsync(role).GetAwaiter().GetResult())
            roleMgr.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
    }

    var adminEmail = builder.Configuration["Seed:AdminEmail"] ?? "admin@local";
    var adminPassword = builder.Configuration["Seed:AdminPassword"] ?? "Admin123!";
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

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();

// The following code should be inside a controller/action, not at the top level
/*
int year = DateTime.Now.Year; // or obtain from form (Thang/Nam)
int month = DateTime.Now.Month;
if (_accountingPeriodService.IsPeriodClosed(year, month))
{
    TempData["Error"] = "Kỳ kế toán đã chốt. Không thể thay đổi dữ liệu cho tháng này.";
    return RedirectToAction("Index");
}

[Authorize(Roles = "DataEntry,ChiefAccountant,Director")]
public class ChiPhiNVLController : Controller { ... }

[Authorize(Roles = "ChiefAccountant,Director")]
[HttpPost]
public IActionResult Approve(int id) { ... }
*/
