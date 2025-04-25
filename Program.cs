// Program.cs de la API

using MemoriaAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;


var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("https://192.168.237.187:4435")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


var connectionString = builder.Configuration.GetConnectionString("FallosDb");

builder.Services.AddDbContext<MemoriaDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins); 

app.UseAuthorization();

app.MapControllers();

app.Run();