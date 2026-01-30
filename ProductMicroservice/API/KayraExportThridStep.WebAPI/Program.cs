using KayraExportThridStep.Application.CQRS.Handlers;
using KayraExportThridStep.Application.CQRS.Service;
using KayraExportThridStep.Application.Interfaces;
using KayraExportThridStep.Core.Entities;
using KayraExportThridStep.Infrastructure.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(
        builder.Configuration.GetConnectionString("Redis")
    )
);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        typeof(GetProductQueryHandler).Assembly
    );
});


builder.Services.AddScoped(typeof(IRepository<>), sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connStr = config.GetConnectionString("SqlServer");

    return Activator.CreateInstance(
        typeof(Repository<>).MakeGenericType(
            sp.GetType().GenericTypeArguments),
        connStr
    )!;
});

builder.Services.AddScoped<IProductCommandRepository>(sp =>
    new ProductCommandRepository(
        builder.Configuration.GetConnectionString("SqlServer")));


builder.Services.AddScoped<ICacheService, CacheService>();

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
