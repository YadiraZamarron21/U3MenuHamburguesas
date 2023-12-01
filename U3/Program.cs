using Microsoft.EntityFrameworkCore;
using U3.Models.Entities;
using U3.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<Repository<Clasificacion>>();
builder.Services.AddTransient<MenuRepository>();
builder.Services.AddTransient<ClasificacionRepository>();

builder.Services.AddDbContext<NeatContext>(x => x.UseMySql("server=localhost;user=root;password=root;database=neat", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql")));

builder.Services.AddMvc();
var app = builder.Build();

app.UseFileServer();
app.MapControllerRoute(
    name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );

app.MapDefaultControllerRoute();
app.Run();


