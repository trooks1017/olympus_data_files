namespace Olympus.Data.Files.Models;

/// <summary>
/// Configuration settings for Clio logging in Olympus Data Files
/// </summary>
public class OlympusClioSettings
{
    public const string SectionName = "Clio";
    
    // Console Logger Settings
    public bool EnableConsoleLogger { get; set; } = true;
    
    // File Logger Settings
    public bool EnableFileLogger { get; set; } = false;
    public string LogFilePath { get; set; } = "logs/olympus-data-files.log";
    public int MaxFileSizeInMB { get; set; } = 10;
    public int MaxLogFiles { get; set; } = 5;
    
    // NATS Logger Settings
    public bool EnableMessageLogger { get; set; } = false;
    public string NatsUrl { get; set; } = "nats://localhost:4222";
    public string LogSubject { get; set; } = "logs.olympus-data-files";
    public int ConnectionTimeout { get; set; } = 5000; // milliseconds
    
    // General Settings
    public string ApplicationName { get; set; } = "Olympus Data Files";
    public string MinimumLogLevel { get; set; } = "Information";
}
