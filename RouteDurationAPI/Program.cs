using Microsoft.EntityFrameworkCore;
using RouteDurationAPI.DB;
using RouteDurationAPI.Services;



var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
// Dodaj us�ugi Swaggera
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(); 

builder.Services.AddOpenApi();
builder.Services.AddScoped<IRouteDurationService, RouteDurationService>();
// Konfiguracja po��czenia z baz� danych
var connectionString = builder.Configuration.GetConnectionString("CS");
builder.Services.AddDbContext<RouteDurationContext>(options =>
    options.UseSqlServer(connectionString));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
