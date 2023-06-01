using catalogServiceAPI.Models;
using catalogServiceAPI.Services;
using NLog;
using NLog.Web;

// Set up NLog logger using configuration from app settings
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    // Create a new WebApplicationBuilder instance
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    

    builder.Services.AddEndpointsApiExplorer();
    
    // Add Swagger generation to the services collection
    builder.Services.AddSwaggerGen();

    // Register the ItemsRepository as a singleton service
    builder.Services.AddSingleton<IItemsRepository, ItemsRepository>();

    // Register the ItemToAuctionRepository as a singleton service
    builder.Services.AddSingleton<IItemToAuctionRepository, ItemToAuctionRepository>();

    // Clear any existing logging providers
    builder.Logging.ClearProviders();

    // Use NLog for logging
    builder.Host.UseNLog();

    // Build the application
    var app = builder.Build();

    // Enable Swagger and SwaggerUI
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    // Enable authorization
    app.UseAuthorization();

    // Map the controllers to routes
    app.MapControllers();

    
    app.Run();
}
catch (System.Exception ex)
{
    
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
   
    NLog.LogManager.Shutdown();
}
