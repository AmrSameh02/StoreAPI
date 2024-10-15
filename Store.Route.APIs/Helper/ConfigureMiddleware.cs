using Store.Route.APIs.Middlewares;
using Store.Route.Repository.Data.Contexts;
using Store.Route.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Store.Route.Repository.Identity.Contexts;
using Store.Route.Repository.Identity;
using Microsoft.AspNetCore.Identity;
using Store.Route.Core.Entities.Identity;

namespace Store.Route.APIs.Helper
{
    public static class ConfigureMiddleware
    {
        public static async Task<WebApplication> ConfigureMiddlewareAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<StoreDbContext>();
            var identityContext = services.GetRequiredService<StoreIdentityDbContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                await context.Database.MigrateAsync();
                await StoreDbContextSeed.SeedAsync(context);
                await identityContext.Database.MigrateAsync();
                await StoreIdentityDbContextSeed.SeedAppUserAsync(userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();

                logger.LogError(ex, "there are problems during apply migrations");
            }


            app.UseMiddleware<ExceptionMiddleware>(); //configure user defined middleware

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            return app;
        } 
    }
}
