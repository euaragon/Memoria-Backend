// Program.cs de la API

using MemoriaAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using MemoriaAPI.Models;
using NSwag;
using NSwag.Generation.Processors.Security;
using Serilog;
using MemoriaAPI.Services;
using MemoriaAPI.Service;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyOrigin()
            //policy.WithOrigins("https://localhost", "https://webprueba", "https://www.tribcuentasmendoza.gob.ar")
                   .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});



// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/startup-log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();


builder.Host.UseSerilog();

var connectionString = builder.Configuration.GetConnectionString("FallosDb");

builder.Services.AddDbContext<MemoriaDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.Configure<ApiConfiguration>(builder.Configuration.GetSection("ApiConfiguration"));


builder.Services.AddControllers();
builder.Services.AddScoped<IFallosService, FallosService>();
builder.Services.AddScoped<IConfiguracionService, ConfiguracionService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<ISeccionService, SeccionService>();
builder.Services.AddScoped<IPaginaService, PaginaService>();
builder.Services.AddScoped<IContenidoService, ContenidoService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "Fallos Memoria 2024";
    config.Version = "v1";
    config.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = OpenApiSecurityApiKeyLocation.Header,
        Description = "Introduce el token JWT con el formato: Bearer {token}"
    });

    config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateLifetime = true
    };
});




var app = builder.Build();


try
{
    using (var scope = app.Services.CreateScope())
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            logger.LogInformation("✅ Conexión exitosa a la base de datos.");
        }
    }
}
catch (Exception ex)
{
    Log.Fatal(ex, "❌ La aplicación no pudo iniciarse correctamente.");
    return;
}
finally
{
    Log.CloseAndFlush();
}



app.UseHttpsRedirection();

app.UseStaticFiles();


app.UseRouting();

app.MapGet("/", () => Results.Redirect("/MemoriaAPI/swagger/index.html"));

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())   //Linea para que la app funcione en desarrollo y produccion. 
{
    app.UseOpenApi(); // Sirve /swagger/v1/swagger.json
    app.UseSwaggerUi();
}



app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();