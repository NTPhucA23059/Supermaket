using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Supermaket.Data;
using Supermaket.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<OnlineSupermarketContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLSupermaket"));
});


builder.Services.AddSingleton(x => new PaypalClient(
        builder.Configuration["PaypalOptions:AppId"],
        builder.Configuration["PaypalOptions:AppSecret"],
        builder.Configuration["PaypalOptions:Mode"]
));

builder.Services.AddAutoMapper(typeof(AutoMapperAdmin));
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure Authentication and Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login"; 
        options.LogoutPath = "/User/Logout";
        options.AccessDeniedPath = "/AccessDenied"; 
    });




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();


app.UseAuthentication();
app.UseAuthorization();





app.Run();
