using Data;
using Microsoft.EntityFrameworkCore;
using Service;
using Middleware;
using Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
	});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Entity Framework Core con Base de Datos en Memoria
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseInMemoryDatabase("TaskAPIDb"));

// Inyección de Dependencias
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITaskService, TaskService>();

// Logging
builder.Services.AddLogging();

var app = builder.Build();

// Configurar el pipeline HTTP

// Middleware de manejo global de excepciones
app.UseGlobalExceptionHandler();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();

	// Seed data inicial
	using var scope = app.Services.CreateScope();
	var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
	context.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();