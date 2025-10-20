using Clio.Shared;
using Mercury.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Olympus.Data.Files.Models;

namespace Olympus.Data.Files.Services;

/// <summary>
/// Service responsible for creating and managing ClioClient from Olympus configuration
/// </summary>
public class ClioService : IClioService
{
    private readonly ClioClient _clioClient;

    public ClioService(IOptions<OlympusClioSettings> olympusClioSettings, IConfiguration configuration)
    {
        var settings = olympusClioSettings.Value;
        
        // Create LoggerSettings from OlympusClioSettings
        var loggerSettings = CreateLoggerSettings(settings, configuration);
        
        // Instantiate ClioClient with the configured settings
        _clioClient = new ClioClient(loggerSettings);
    }

    /// <summary>
    /// Logs a message with the specified log level
    /// </summary>
    public void Log(LogLevel level, string message, Exception? exception = null)
    {
        _clioClient.Log(level, message, exception);
    }

    /// <summary>
    /// Logs a trace message
    /// </summary>
    public void Trace(string message)
    {
        _clioClient.Trace(message);
    }

    /// <summary>
    /// Logs a debug message
    /// </summary>
    public void Debug(string message)
    {
        _clioClient.Debug(message);
    }

    /// <summary>
    /// Logs an information message
    /// </summary>
    public void Information(string message)
    {
        _clioClient.Information(message);
    }

    /// <summary>
    /// Logs a warning message
    /// </summary>
    public void Warning(string message)
    {
        _clioClient.Warning(message);
    }

    /// <summary>
    /// Logs an error message
    /// </summary>
    public void Error(string message, Exception? exception = null)
    {
        _clioClient.Error(message, exception);
    }

    /// <summary>
    /// Logs a critical message
    /// </summary>
    public void Critical(string message, Exception? exception = null)
    {
        _clioClient.Critical(message, exception);
    }

    /// <summary>
    /// Gets recent log entries from all active loggers
    /// </summary>
    /// <param name="count">Number of recent entries to retrieve</param>
    /// <returns>Dictionary with logger type as key and list of log entries as value</returns>
    public Dictionary<string, List<string>> GetRecentLogs(int count)
    {
        return _clioClient.GetRecentLogs(count);
    }

    /// <summary>
    /// Gets recent log entries from all active loggers as a flattened list
    /// </summary>
    /// <param name="count">Number of recent entries to retrieve per logger</param>
    /// <returns>Flattened list of all log entries</returns>
    public List<string> GetRecentLogsFlattened(int count)
    {
        return _clioClient.GetRecentLogsFlattened(count);
    }

    /// <summary>
    /// Creates Mercury.Shared.LoggerSettings from OlympusClioSettings
    /// </summary>
    private LoggerSettings CreateLoggerSettings(OlympusClioSettings settings, IConfiguration configuration)
    {
        // Convert string log level to Mercury.Shared.LogLevel enum
        var minimumLogLevel = ConvertToMercuryLogLevel(settings.MinimumLogLevel, configuration);

        // Create and populate Mercury.Shared.LoggerSettings
        var mercuryLoggerSettings = new LoggerSettings
        {
            // Console Logger Settings
            EnableConsoleLogger = settings.EnableConsoleLogger,
            
            // File Logger Settings
            EnableFileLogger = settings.EnableFileLogger,
            LogFilePath = settings.LogFilePath,
            MaxFileSizeInMB = settings.MaxFileSizeInMB,
            MaxLogFiles = settings.MaxLogFiles,
            
            // NATS Logger Settings
            EnableMessageLogger = settings.EnableMessageLogger,
            NatsUrl = settings.NatsUrl,
            LogSubject = settings.LogSubject,
            ConnectionTimeout = settings.ConnectionTimeout,
            
            // General Settings
            ApplicationName = settings.ApplicationName,
            MinimumLogLevel = minimumLogLevel
        };

        return mercuryLoggerSettings;
    }

    /// <summary>
    /// Converts string log level from configuration to Mercury.Shared.LogLevel enum
    /// </summary>
    private LogLevel ConvertToMercuryLogLevel(string logLevelString, IConfiguration configuration)
    {
        var effectiveLogLevel = logLevelString;
        
        if (string.IsNullOrEmpty(effectiveLogLevel))
        {
            // Fallback to Microsoft logging configuration
            effectiveLogLevel = configuration["Logging:LogLevel:Default"] ?? "Information";
        }

        return effectiveLogLevel.ToLowerInvariant() switch
        {
            "trace" => LogLevel.Trace,
            "debug" => LogLevel.Debug,
            "information" => LogLevel.Information,
            "warning" => LogLevel.Warning,
            "error" => LogLevel.Error,
            "critical" => LogLevel.Critical,
            "none" => LogLevel.None,
            _ => LogLevel.Information // Default fallback
        };
    }
}
