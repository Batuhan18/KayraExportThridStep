using KayraExportThridStep.Application.CQRS.Handlers;
using KayraExportThridStep.Application.CQRS.Service;
using KayraExportThridStep.Application.Interfaces;
using KayraExportThridStep.Infrastructure.Services;
using MediatR;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(
        builder.Configuration.GetConnectionString("Redis")
    )
);

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetProductQueryHandler).Assembly);
});

// Generic Repository
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

// Command Repository
builder.Services.AddScoped<IProductCommandRepository>(sp =>
    new ProductCommandRepository(
        builder.Configuration.GetConnectionString("SqlServer")
    )
);

// Cache
builder.Services.AddScoped<ICacheService, CacheService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
