// Program.cs de la API

using MemoriaAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using MemoriaAPI.Models;
using NSwag;
using NSwag.Generation.Processors.Security;
using Microsoft.AspNetCore.Builder;
using MemoriaAPI.Services;


var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://192.168.237.187:5010", "https://192.168.237.187:4435", "https://localhost", "https://webprueba", "https://www.tribcuentasmendoza.gob.ar")
                   .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


var connectionString = builder.Configuration.GetConnectionString("FallosDb");

builder.Services.AddDbContext<MemoriaDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.Configure<ApiConfiguration>(builder.Configuration.GetSection("ApiConfiguration"));


builder.Services.AddControllers();
builder.Services.AddScoped<IFallosService, FallosService>();
builder.Services.AddScoped<IConfiguracionService, ConfiguracionService>();
builder.Services.AddScoped<IHomeService, HomeService>();
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
    Console.WriteLine("❌ Error al conectar a la base de datos: " + ex.Message);
    throw;
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

app.UseAuthorization();

app.MapControllers();

app.Run();