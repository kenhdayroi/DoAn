using Microsoft.EntityFrameworkCore;
using LuxStay.Models;
using LuxStay.Dao;
using Microsoft.AspNetCore.Mvc;  // Thay thế System.Web.Mvc
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký các dịch vụ
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<DataProvider>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddTransient<SendMailDao>();
builder.Services.AddScoped<BookingTourDao>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var app = builder.Build();

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Sử dụng session trước authentication/authorization
app.UseSession();

// Đảm bảo rằng authentication và authorization được sử dụng đúng thứ tự
app.UseAuthentication(); // Nếu bạn có cấu hình xác thực
app.UseAuthorization();

// Định tuyến cho các controller
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.Run();
