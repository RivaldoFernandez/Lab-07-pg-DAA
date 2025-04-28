// using System.Text;
// using Lab_07_Roman_Qquelcca.Middleware;
// using Lab_07_Roman_Qquelcca.Models;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens;
//
// var builder = WebApplication.CreateBuilder(args);
//
// // Agregar servicios al contenedor
// builder.Services.AddControllersWithViews();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
//
// // Configuración de DbContext con MySQL
// builder.Services.AddDbContext<MiddlewareDbContext>(options =>
//     options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));
//
// // Configuración de autenticación JWT
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters()
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidIssuer = builder.Configuration["Jwt:Issuer"],
//             ValidAudience = builder.Configuration["Jwt:Audience"],
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
//         };
//     });
//
// // Configuración de autorización
// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("Admin", policy => policy.RequireClaim("Role", "admin"));
// });
//
// var app = builder.Build();
//
// // Configuración del pipeline HTTP
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Home/Error");
//     app.UseHsts();
// }
//
// app.UseHttpsRedirection();
//
// // Middleware de manejo de errores (DEBE SER EL PRIMERO PERSONALIZADO)
// app.UseMiddleware<ErrorHandlingMiddleware>();
//
// // Habilitar Swagger solo en Desarrollo
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI(c =>
//     {
//         c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
//         c.RoutePrefix = string.Empty;
//     });
// }
//
// app.UseRouting();
//
// // Middleware de autenticación y autorización
// app.UseAuthentication();
// app.UseAuthorization();
//
// // Middleware de validación de parámetros
// app.UseMiddleware<ParameterValidationMiddleware>();
//
// // Middleware de control de acceso por roles
// app.UseMiddleware<RoleBasedAccessMiddleware>();
//
// // Mapear controladores
// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapControllers();
// });
//
// // Configuración de la ruta por defecto (MVC)
// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");
//
// app.Run();



using Lab_07_Roman_Qquelcca.Middleware;
using Lab_07_Roman_Qquelcca.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de DbContext con MySQL
builder.Services.AddDbContext<MiddlewareDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de autenticación por Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login"; // Redirige a esta ruta si no está autenticado
        options.AccessDeniedPath = "/access-denied"; // Redirige si no tiene permisos
    });

// Configuración de autorización con roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("admin"));
    options.AddPolicy("Vendedor", policy => policy.RequireRole("vendedor"));
});

var app = builder.Build();

// Configuración del pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Middleware de manejo de errores personalizado
app.UseMiddleware<ErrorHandlingMiddleware>();

// Habilitar Swagger solo en Desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseRouting();

// Middleware de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Middleware de validación de parámetros
app.UseMiddleware<ParameterValidationMiddleware>();

// Middleware de control de acceso por roles
app.UseMiddleware<RoleBasedAccessMiddleware>();

// Mapear controladores
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Configuración de la ruta por defecto (MVC)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
