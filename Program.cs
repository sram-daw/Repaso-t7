using Repaso_t7.Models;
using Repaso_t7.Models.Repository;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton(new Conexion(builder.Configuration.GetConnectionString("ConexionTarea7")));
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (context, next) =>
    {
        var currentThreadCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
        currentThreadCulture.NumberFormat = NumberFormatInfo.InvariantInfo;

        Thread.CurrentThread.CurrentCulture = currentThreadCulture;
        Thread.CurrentThread.CurrentUICulture = currentThreadCulture;

        await next();
    });

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Producto}/{action=Login}");

app.Run();
