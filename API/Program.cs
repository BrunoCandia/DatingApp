using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContextPool<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(configurePolicy =>
{
    configurePolicy
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins("http://localhost:4200", "https://localhost:4200");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
