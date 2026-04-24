using dynamic_form_system.Data;
using dynamic_form_system.Interface;
using dynamic_form_system.Middlewares;
using dynamic_form_system.Repository;
using dynamic_form_system.Services;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// 2. Khai báo Swagger 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//connect db 
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IFormRepository, FormRepository>();
builder.Services.AddScoped<IFormService, FormService>();

var app = builder.Build();
app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// T? ??ng Apply Migration khi app kh?i ??ng (R?t quan tr?ng cho Docker)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        // L?nh này t??ng ???ng v?i 'dotnet ef database update'
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Log l?i n?u database ch?a s?n sàng
        Console.WriteLine($"Could not run migrations: {ex.Message}");
    }
}
app.Run();
