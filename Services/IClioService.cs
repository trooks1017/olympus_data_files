using Mercury.Shared;

namespace Olympus.Data.Files.Services;

/// <summary>
/// Service responsible for managing Clio logging
/// </summary>
public interface IClioService
{
    /// <summary>
    /// Logs a message with the specified log level
    /// </summary>
    void Log(LogLevel level, string message, Exception? exception = null);
    
    /// <summary>
    /// Logs a trace message
    /// </summary>
    void Trace(string message);
    
    /// <summary>
    /// Logs a debug message
    /// </summary>
    void Debug(string message);
    
    /// <summary>
    /// Logs an information message
    /// </summary>
    void Information(string message);
    
    /// <summary>
    /// Logs a warning message
    /// </summary>
    void Warning(string message);
    
    /// <summary>
    /// Logs an error message
    /// </summary>
    void Error(string message, Exception? exception = null);
    
    /// <summary>
    /// Logs a critical message
    /// </summary>
    void Critical(string message, Exception? exception = null);
    
    /// <summary>
    /// Gets recent log entries from all active loggers
    /// </summary>
    /// <param name="count">Number of recent entries to retrieve</param>
    /// <returns>Dictionary with logger type as key and list of log entries as value</returns>
    Dictionary<string, List<string>> GetRecentLogs(int count);
    
    /// <summary>
    /// Gets recent log entries from all active loggers as a flattened list
    /// </summary>
    /// <param name="count">Number of recent entries to retrieve per logger</param>
    /// <returns>Flattened list of all log entries</returns>
    List<string> GetRecentLogsFlattened(int count);
}
