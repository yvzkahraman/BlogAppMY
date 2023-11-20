using BlogApp.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.Cookie.HttpOnly = true;
        opt.Cookie.SameSite = SameSiteMode.Strict;
        opt.Cookie.Name = "LoginCookie";
        opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    });


builder.Services.AddCors(option =>
{
    option.AddPolicy("default", conf =>
    {
        conf.WithOrigins(new string[]
        {
            "http://127.0.0.1:5500"
        }).AllowAnyMethod().AllowAnyHeader();
    });
});


builder.Services.AddDbContext<BlogDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Local"));
});


builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("default");

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(name: "area", pattern: "{Area}/{Controller=Category}/{Action=Index}/{id?}");
app.MapDefaultControllerRoute();


app.Run();
