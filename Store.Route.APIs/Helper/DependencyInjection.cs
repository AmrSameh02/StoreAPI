using Microsoft.EntityFrameworkCore;
using Store.Route.Core.Services.Contract;
using Store.Route.Core;
using Store.Route.Repository;
using Store.Route.Repository.Data.Contexts;
using Store.Route.Service.Services.Products;
using Store.Route.Core.Mapping.Products;
using Microsoft.AspNetCore.Mvc;
using Store.Route.APIs.Errors;
using Store.Route.Core.Repositories.Contract;
using Store.Route.Repository.Repositories;
using StackExchange.Redis;
using Store.Route.Core.Mapping.Baskets;
using Store.Route.Service.Services.Caches;
using Store.Route.Repository.Identity.Contexts;
using Store.Route.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Store.Route.Service.Services.Tokens;
using Store.Route.Service.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Store.Route.Core.Mapping.Auth;
using Store.Route.Core.Mapping.Orders;
using Store.Route.Service.Services.Orders;
using Store.Route.Service.Services.Baskets;
using Store.Route.Service.Services.Payment;

namespace Store.Route.APIs.Helper
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBuiltInService();
            services.AddSwaggerService();
            services.AddDbContextService(configuration);
            services.AddUserDefinedService();
            services.AddAutoMapperService(configuration);
            services.ConfigureInvalidModelStateResponseService();
            services.AddRedisService(configuration);
            services.AddIdentityService();
            services.AddAuthenticationService(configuration);
            return services;
        }

        private static IServiceCollection AddBuiltInService(this IServiceCollection services)
        {
            services.AddControllers();  
            return services;
        }

        private static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }

        private static IServiceCollection AddDbContextService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });

            return services;
        }

        private static IServiceCollection AddUserDefinedService(this IServiceCollection services)
        {
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPaymentService, PaymentService>();
            return services;
        }

        private static IServiceCollection AddAutoMapperService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(m => m.AddProfile(new OrderProfile(configuration)));
            services.AddAutoMapper(m => m.AddProfile(new ProductProfile(configuration)));
            services.AddAutoMapper(m => m.AddProfile(new BasketProfile()));
            services.AddAutoMapper(m => m.AddProfile(new AuthProfile()));
            return services;
        }

        private static IServiceCollection ConfigureInvalidModelStateResponseService(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (ActionContext) =>
                {
                    var errors = ActionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                   .SelectMany(p => p.Value.Errors)
                   .Select(p => p.ErrorMessage)
                   .ToArray();

                    var response = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response);
                };
            });
            return services;
        }
    
        private static IServiceCollection AddRedisService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connection = configuration.GetConnectionString("Redis");

                return ConnectionMultiplexer.Connect(connection);
            });

            return services;
        }

        private static IServiceCollection AddIdentityService(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();

            return services;
        }


        private static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:key"]))
                };
            });

            return services;
        }


    }
}
