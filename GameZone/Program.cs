using GameZone.Data;
using GameZone.Services;
using Microsoft.AspNetCore.Identity;
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
builder.Services.AddScoped<IUserGamesServies, UserGamesServies>();
builder.Services.AddSession();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options=>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();




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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
