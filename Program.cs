using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Olympus.Data.Files.Models;
using Olympus.Data.Files.Services;

// Create the host builder
var builder = Host.CreateApplicationBuilder(args);

// Configure configuration sources
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args);

// Configure options from appsettings.json
builder.Services.Configure<OlympusClioSettings>(
    builder.Configuration.GetSection(OlympusClioSettings.SectionName));

// Register ClioService as singleton
builder.Services.AddSingleton<IClioService, ClioService>();

// Build the host
var host = builder.Build();

// Get logger for program-level logging using Clio
var logger = host.Services.GetRequiredService<IClioService>();

try
{
    logger.Information("==========================================================");
    logger.Information("Olympus.Data.Files MCP Server Starting...");
    logger.Information($"Environment: {builder.Environment.EnvironmentName}");
    logger.Information("==========================================================");

    // Validate configuration on startup
    var clioSettings = host.Services.GetRequiredService<Microsoft.Extensions.Options.IOptions<OlympusClioSettings>>().Value;

    logger.Information("Configuration loaded:");
    logger.Information($"- Application Name: {clioSettings.ApplicationName}");
    logger.Information($"- Console Logger: {clioSettings.EnableConsoleLogger}");
    logger.Information($"- File Logger: {clioSettings.EnableFileLogger}");
    logger.Information($"- Message Logger: {clioSettings.EnableMessageLogger}");
    logger.Information($"- Minimum Log Level: {clioSettings.MinimumLogLevel}");

    if (clioSettings.EnableFileLogger)
    {
        logger.Information($"- Log File Path: {clioSettings.LogFilePath}");
        logger.Information($"- Max File Size: {clioSettings.MaxFileSizeInMB} MB");
        logger.Information($"- Max Log Files: {clioSettings.MaxLogFiles}");
    }

    if (clioSettings.EnableMessageLogger)
    {
        logger.Information($"- NATS URL: {clioSettings.NatsUrl}");
        logger.Information($"- Log Subject: {clioSettings.LogSubject}");
    }

    // Test GetRecentLogs functionality
    logger.Debug("Testing GetRecentLogs functionality...");
    var recentLogs = logger.GetRecentLogs(5);
    logger.Debug($"Retrieved logs from {recentLogs.Count} logger types");
    
    foreach (var loggerType in recentLogs.Keys)
    {
        logger.Debug($"  - {loggerType}: {recentLogs[loggerType].Count} entries");
    }

    // Run the host
    await host.RunAsync();

    logger.Information("==========================================================");
    logger.Information("Olympus.Data.Files MCP Server Stopped");
    logger.Information("==========================================================");

    return 0;
}
catch (OperationCanceledException)
{
    logger.Information("Service was cancelled");
    return 0;
}
catch (Exception ex)
{
    logger.Critical("Host terminated unexpectedly", ex);
    return -1;
}
