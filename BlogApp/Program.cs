using BlogApp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BlogDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Local"));
});


builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});


app.Run();
