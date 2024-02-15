using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Novel.API.Services;
using Novle.Application.Common.Interfaces;
using Novle.Application.Services;
using Novle.Domain.Repositories.Base;
using Novle.Infrastructure.Identity;
using Novle.Infrastructure.Repositories.Base;

namespace Novel.API.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        return services;
    } 
    
    public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<NovleDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        return services;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.Scan(scan => scan.FromAssembliesOf(typeof(RepositoryBase<>))
            .AddClasses(c => c.AssignableTo(typeof(RepositoryBase<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblyOf<BaseService>()
            .AddClasses(c => c.AssignableTo<BaseService>())
            .AsSelf()
            .WithScopedLifetime());
        return services;
    }

    public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;

                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<NovleDbContext>()
            .AddDefaultTokenProviders();
        return services;
    }
    
    public static IServiceCollection AddCurrentUser(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddHttpContextAccessor();
        return services;
    }
}