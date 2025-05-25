using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.EntityFrameworkCore;
using VMS.Helper;
using VMS.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
//Adding DataContext Services
//builder.Services.AddDbContext<VmsDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("VMSContext")));

builder.Services.AddDbContext<VmsDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("VMSContext"),
    sqlServerOptions => sqlServerOptions.CommandTimeout((int)TimeSpan.FromMinutes(180).TotalSeconds))
);

builder.Services.AddRazorPages();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var provider = builder.Services.BuildServiceProvider();
IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
IWebHostEnvironment environment = provider.GetRequiredService<IWebHostEnvironment>();
IHttpContextAccessor accessor = provider.GetRequiredService<IHttpContextAccessor>();
builder.Services.AddSingleton(configuration);

//Added for session state
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(120);
});
//services.AddSingleton<IConfiguration>(configRoot);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ISenderEmail, EmailService>();

VmsDbContext dbContext = provider.GetRequiredService<VmsDbContext>();

// register the static class
utilityHelper.configure(configuration);
utilityHelper.environment(environment);
utilityHelper.tbsDataContext(dbContext);
utilityHelper.sessionExtension(accessor);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.UseFileServer(enableDirectoryBrowsing: false);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();