using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ShoeStore.Application.Interfaces.Services;
using ShoeStore.Application.Interfaces.Services.Auth;
using ShoeStore.Application.Interfaces.Services.Carts;
using ShoeStore.Application.Interfaces.Services.Orders;
using ShoeStore.Application.Interfaces.Services.Shoes;
using ShoeStore.Application.Interfaces.Services.Users;
using ShoeStore.Application.Services;
using ShoeStore.Application.Services.Auth;
using ShoeStore.Application.Services.Carts;
using ShoeStore.Application.Services.Orders;
using ShoeStore.Application.Services.Shoes;
using ShoeStore.Application.Services.Users;
using ShoeStore.Application.Validators.Shoes;

namespace ShoeStore.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();

        services.AddHttpContextAccessor();

        services.AddAutoMapper(currentAssemblies);

        services.AddValidatorsFromAssemblyContaining<ShoeCreateDtoValidator>();
        services.AddScoped<IValidationService, ValidationService>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IShoeService, ShoeService>();

        services.AddScoped<IDeliveryMethodService, DeliveryMethodService>();
        services.AddScoped<IOrderService, OrderService>();

        services.AddScoped<ICartService, CartService>();

        return services;
    }
}