using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using OrderService;
using OrderService.Data;
using OrderService.Models;
using OrderService.Repositories;
using OrderService.Services;
using Shared.Helpers;
using Shared.Middleware;
using Shared.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.OperationFilter<CustomHeaderParameter>();
});

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection"));
});

builder.Services.AddHostedService<RabbitMqConsumer>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("RabbitMqConnection");
    var queueName = builder.Configuration.GetConnectionString("QueueName");
    var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();

    return new RabbitMqConsumer(connectionString, queueName, scopeFactory);
});

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService.Services.OrderService>();
builder.Services.AddHttpClient<IIdentityService, IdentityService>();
builder.Services.AddScoped<ApiKeyAuthFilter>();

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("Fixed", opt =>
    {
        opt.Window = TimeSpan.FromSeconds(3);
        opt.PermitLimit = 3;
    });
});


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.Run();
