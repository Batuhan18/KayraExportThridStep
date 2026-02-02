using KayraExportThridStep.Application.CQRS.Handlers;
using KayraExportThridStep.Application.CQRS.Service;
using KayraExportThridStep.Application.Interfaces;
using KayraExportThridStep.Core.Entities;
using KayraExportThridStep.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Swagger JWT desteği
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    //c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    //{
    //    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
    //    Name = "Authorization",
    //    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
    //    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
    //    Scheme = "Bearer"
    //});
    //c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    //{
    //    {
    //        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    //        {
    //            Reference = new Microsoft.OpenApi.Models.OpenApiReference
    //            {
    //                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
    //                Id = "Bearer"
    //            }
    //        },
    //        Array.Empty<string>()
    //    }
    //});
});

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithProperty("ServiceName", "ProductMicroservice")
    .CreateLogger();

builder.Host.UseSerilog();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddScoped<IProductCommandRepository, ProductCommandRepository>();

//Redis
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

// ✅ DOĞRU GENERIC REPOSITORY KAYDI
builder.Services.AddScoped(typeof(IRepository<Product>), typeof(Repository<Product>));

// Command Repository
builder.Services.AddScoped<IProductCommandRepository, ProductCommandRepository>();

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




