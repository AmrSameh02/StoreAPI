using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Store.Route.APIs.Errors;
using Store.Route.APIs.Helper;
using Store.Route.APIs.Middlewares;
using Store.Route.Core;
using Store.Route.Core.Mapping.Products;
using Store.Route.Core.Services.Contract;
using Store.Route.Repository;
using Store.Route.Repository.Data;
using Store.Route.Repository.Data.Contexts;
using Store.Route.Service.Services.Products;

namespace Store.Route.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDependency(builder.Configuration);

            var app = builder.Build();

            await app.ConfigureMiddlewareAsync();
            

            app.Run();
        }
    }
}
