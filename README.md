# Olympus.Data.Files

MCP Server project for file operations using C# .NET 9 with Clio logging integration.

## Features

- **Clio Logging**: Integrated logging system with support for console, file, and message (NATS) loggers
- **Configuration**: Flexible configuration via appsettings.json
- **Dependency Injection**: Built on Microsoft.Extensions.Hosting for proper service management

## Requirements

- .NET 9 SDK
- Clio.Shared library (referenced project)
- Mercury.Shared library (referenced project)

## Project Structure

```
olympus_data_files/
├── Models/
│   └── OlympusClioSettings.cs    # Clio logging configuration model
├── Services/
│   ├── IClioService.cs           # Clio service interface
│   └── ClioService.cs            # Clio service implementation
├── Program.cs                     # Main application entry point
├── appsettings.json              # Application configuration
├── appsettings.Development.json  # Development configuration
├── Olympus.Data.Files.csproj     # Project file
└── README.md                     # This file
```

## Configuration

The application uses `appsettings.json` for configuration. Key settings include:

### Clio Logging Configuration

```json
{
  "Clio": {
    "EnableConsoleLogger": true,
    "EnableFileLogger": false,
    "EnableMessageLogger": false,
    "LogFilePath": "logs/olympus-data-files.log",
    "MaxFileSizeInMB": 10,
    "MaxLogFiles": 5,
    "NatsUrl": "nats://localhost:4222",
    "LogSubject": "logs.olympus-data-files",
    "ConnectionTimeout": 5000,
    "ApplicationName": "Olympus Data Files",
    "MinimumLogLevel": "Information"
  }
}
```

## Build and Run

```bash
# Build the project
dotnet build

# Run the project
dotnet run
```

## Logging

The application uses Clio.Shared for logging with three types of loggers:

1. **Console Logger**: Outputs to console with color-coded log levels
2. **File Logger**: Writes to log files with automatic rotation
3. **Message Logger**: Publishes logs to NATS message bus

### Log Levels

- Trace
- Debug
- Information
- Warning
- Error
- Critical

### Retrieving Recent Logs

The application can retrieve recent log entries programmatically:

```csharp
// Get recent logs organized by logger type
var recentLogs = logger.GetRecentLogs(count: 10);

// Get recent logs as a flattened list
var flattenedLogs = logger.GetRecentLogsFlattened(count: 10);
```

## Next Steps

- [ ] Implement MCP server functionality
- [ ] Add HTTPTransport support
- [ ] Integrate Zeus messaging transport
- [ ] Implement file operation tools

## License

MIT
