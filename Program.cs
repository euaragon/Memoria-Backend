// Program.cs de la API

using MemoriaAPI.Models;
using MemoriaAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MemoriaDB");

try
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();
        Console.WriteLine("Conexion exitosa a la base de datos.");
    }
}
catch (Exception ex)
{
    Console.WriteLine("Error al conectar a la base de datos: " + ex.Message);
    builder.Logging.AddConsole(); 
    var logger = builder.Build().Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Error de conexion a la base de datos");
    throw new Exception("Error de conexion a la base de datos: " + ex.Message); 
}


builder.Services.AddDbContext<MemoriaDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MemoriaDB")));


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
