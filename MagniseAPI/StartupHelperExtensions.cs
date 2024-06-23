using MagniseAPI.DbContexts;
using MagniseAPI.Helpers;
using MagniseAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;


namespace MagniseAPI
{
    internal static class StartupHelperExtensions
    {
        public static WebApplication ConfigureServices(
            this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<MarketContext>(options =>
                options.UseSqlite(
                    builder.Configuration["ConnectionStrings:MarketDBConnectionString"], options =>
                    {
                        options.UseRelationalNulls();
                        options.CommandTimeout(30);
                        options.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                    }));

            var configuration = builder.Configuration;
            builder.Services.Configure<FintatechApiOptions>(configuration.GetSection("FintatechApi"));

            builder.Services.AddScoped<IPricesRepository, PricesRepository>();

            builder.Services.AddScoped<IAssetsRepository, AssetsRepository>();

            builder.Services.AddScoped<IWebSocketService, WebSocketService>();

            builder.Services.AddScoped<IHistoricalService, HistoricalService>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpClient<TokenService>();

            builder.Services.AddHostedService<DataProviderBackgroundService>();

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }

        public static async Task ResetDatabaseAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetService<MarketContext>();
                    if (context != null)
                    {
                        await context.Database.EnsureDeletedAsync();
                        await context.Database.MigrateAsync();
                    }
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }
        }
    }
}
