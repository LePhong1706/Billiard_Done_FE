using BilliardShop.Application.Interfaces;
using BilliardShop.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BilliardShop.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Services
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IWishlistService, WishlistService>();
        services.AddScoped<IRecentlyViewedService, RecentlyViewedService>();

        return services;
    }
}