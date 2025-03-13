using CourseManagerApp.Entities;
using CourseManagerApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
string connString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<CourseDBContext>(options => options.UseSqlServer(connString));
builder.Services.AddSingleton<IEmail, Email>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
