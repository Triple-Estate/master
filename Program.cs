using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using G8CozyMVC.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using G8CozyMVC.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add configuration from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddDbContext<G8CozyMVCContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("G8CozyMVCContext") ?? throw new InvalidOperationException("Connection string 'G8CozyMVCContext' not found.")));

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.Cookie.Name = "MyApp.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(10000);
    options.Cookie.IsEssential = true;

    // Set SameSite=None for all paths
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always; // Set to true if serving over HTTPS
});

builder.Services.AddLogging(logging =>
{
    logging.AddConsole(); // Add console logging
    // Add additional logging configurations as needed
});

// Register EmailSender
builder.Services.AddSingleton<IEmailSender, EmailSender>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


