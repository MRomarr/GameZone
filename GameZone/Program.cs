using GameZone.Data;
using GameZone.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connctionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("no conntecion string was found");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connctionString));
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICategoriesServics, CategoriesService>();
builder.Services.AddScoped<IDevicesService, DevicesService>();
builder.Services.AddScoped<IGamesServices, GamesServices>();
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
