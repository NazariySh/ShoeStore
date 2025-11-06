using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ShoeStore.Domain.Repositories;
using ShoeStore.Domain.Settings;
using ShoeStore.Infrastructure.Data;
using ShoeStore.Infrastructure.Repositories;
using ShoeStore.Infrastructure.Services;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShoeStore.Application.Interfaces;
using CloudinaryDotNet;
using ShoeStore.Infrastructure.Data.Initializers;

namespace ShoeStore.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions(configuration);

        services.AddDatabase(configuration);
        services.AddImageStorage(configuration);

        services.AddAuthentication(configuration);
        services.AddAuthorization();

        services.AddScoped<IInitializer, DbInitializer>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ITokenProvider, JwtTokenProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IImageService, ImageService>();

        return services;
    }

    private static void AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CloudinarySettings>(configuration.GetSection(nameof(CloudinarySettings)));
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ShoeStoreDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }

    private static void AddImageStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(_ =>
        {
            var config = configuration.GetSection(nameof(CloudinarySettings)).Get<CloudinarySettings>();
            ArgumentNullException.ThrowIfNull(config);
            return new Cloudinary(new Account(config.CloudName, config.ApiKey, config.ApiSecret));
        });
    }

    private static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
        ArgumentNullException.ThrowIfNull(jwtSettings);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
            };
        });
    }
}