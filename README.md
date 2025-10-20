# Olympus.Data.Files

MCP Server project for file operations using C# .NET 9 with Clio logging integration.

## 📋 Overview

This project provides a Model Context Protocol (MCP) server for file operations, featuring integrated logging through the Clio.Shared library. It demonstrates best practices for integrating Clio logging in .NET 9 applications using dependency injection and configuration management.

## ✨ Features

- **Clio Logging Integration**: Full implementation of Clio.Shared logging system
  - Console logging with color-coded output
  - File logging with automatic rotation
  - NATS message logging support
  - Built-in log retrieval capabilities

- **Configuration Management**: Flexible configuration via appsettings.json
- **Dependency Injection**: Proper service registration and lifetime management
- **Structured Logging**: Consistent logging patterns throughout the application

## 🛠️ Requirements

- .NET 9 SDK
- Clio.Shared library (referenced project)
- Mercury.Shared library (referenced project)

## 📁 Project Structure

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

## ⚙️ Configuration

The application uses `appsettings.json` for configuration. All settings are documented inline.

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

### Configuration Options

| Setting | Type | Default | Description |
|---------|------|---------|-------------|
| `EnableConsoleLogger` | bool | true | Enable console output |
| `EnableFileLogger` | bool | false | Enable file logging |
| `EnableMessageLogger` | bool | false | Enable NATS messaging |
| `LogFilePath` | string | "logs/olympus-data-files.log" | Path to log file |
| `MaxFileSizeInMB` | int | 10 | Maximum log file size before rotation |
| `MaxLogFiles` | int | 5 | Number of rotated files to keep |
| `NatsUrl` | string | "nats://localhost:4222" | NATS server URL |
| `LogSubject` | string | "logs.olympus-data-files" | NATS subject for logs |
| `ConnectionTimeout` | int | 5000 | NATS connection timeout (ms) |
| `ApplicationName` | string | "Olympus Data Files" | Application identifier |
| `MinimumLogLevel` | string | "Information" | Minimum log level to capture |

## 🚀 Build and Run

```bash
# Clone the repository
git clone https://github.com/trooks1017/olympus_data_files.git
cd olympus_data_files

# Build the project
dotnet build

# Run the project
dotnet run
```

## 📝 Logging

### Log Levels

The application supports the following log levels (in order of severity):

- **Trace**: Most verbose, detailed debugging information
- **Debug**: Debugging information
- **Information**: General informational messages
- **Warning**: Warning messages for potentially harmful situations
- **Error**: Error messages for failures
- **Critical**: Critical failures requiring immediate attention

### Using the Logger

The logger is injected through dependency injection:

```csharp
public class MyService
{
    private readonly IClioService _logger;
    
    public MyService(IClioService logger)
    {
        _logger = logger;
    }
    
    public void DoWork()
    {
        _logger.Information("Starting work...");
        _logger.Debug("Processing data");
        _logger.Warning("Potential issue detected");
        _logger.Error("An error occurred", exception);
    }
}
```

### Retrieving Recent Logs

The application can retrieve recent log entries programmatically:

```csharp
// Get recent logs organized by logger type
var recentLogs = logger.GetRecentLogs(count: 10);
foreach (var loggerType in recentLogs.Keys)
{
    Console.WriteLine($"{loggerType}: {recentLogs[loggerType].Count} entries");
}

// Get recent logs as a flattened list
var flattenedLogs = logger.GetRecentLogsFlattened(count: 20);
foreach (var log in flattenedLogs)
{
    Console.WriteLine(log);
}
```

## 📚 Implementation Reference

This project serves as a reference implementation of Clio logging. For detailed information about implementing Clio in your own projects:

### [📖 Clio Implementation Guide](https://github.com/trooks1017/clio_shared/blob/main/IMPLEMENTATION_GUIDE.md)

The guide includes:
- Complete step-by-step implementation instructions
- Architecture and design patterns
- Configuration examples
- Usage best practices
- Troubleshooting tips
- Code templates

## 🏗️ Architecture

### Service Layer Pattern

The application uses a service wrapper pattern around `ClioClient`:

```
IClioService (Interface)
    ↓
ClioService (Implementation)
    ↓
ClioClient (Clio.Shared)
    ↓
├── ConsoleLogger
├── FileLogger
└── MessageLogger
```

This pattern provides:
- **Abstraction**: Application code depends on interfaces, not implementations
- **Encapsulation**: `ClioClient` remains private to the service
- **Testability**: Easy to mock `IClioService` for testing
- **Reusability**: Same pattern works across all projects

## 🔧 Development

### Adding New Services

To add a new service with logging:

1. Create service interface in `Services/`
2. Implement service with `IClioService` injected
3. Register service in `Program.cs`
4. Use logger throughout implementation

Example:

```csharp
public class MyService : IMyService
{
    private readonly IClioService _logger;
    
    public MyService(IClioService logger)
    {
        _logger = logger;
    }
    
    public void Execute()
    {
        _logger.Information($"[{nameof(MyService)}] Executing...");
        // Service implementation
    }
}
```

### Environment-Specific Configuration

Use different settings for different environments:

**appsettings.Development.json**:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug"
    }
  },
  "Clio": {
    "MinimumLogLevel": "Debug"
  }
}
```

**appsettings.Production.json**:
```json
{
  "Clio": {
    "EnableFileLogger": true,
    "EnableMessageLogger": true,
    "MinimumLogLevel": "Information"
  }
}
```

## 🎯 Roadmap

- [x] Initial project setup
- [x] Clio logging integration
- [x] Configuration management
- [x] Dependency injection setup
- [ ] Implement MCP server functionality
- [ ] Add HTTPTransport support
- [ ] Integrate Zeus messaging transport
- [ ] Implement file operation tools
- [ ] Add authentication and authorization
- [ ] Implement file read/write operations
- [ ] Add file listing capabilities
- [ ] Add file deletion capabilities

## 📖 Related Documentation

- [Clio.Shared README](https://github.com/trooks1017/clio_shared/blob/main/README.md)
- [Clio Implementation Guide](https://github.com/trooks1017/clio_shared/blob/main/IMPLEMENTATION_GUIDE.md)
- [Mercury.Shared](https://github.com/trooks1017/Mercury.Shared) (for LogLevel and LoggerSettings)

## 🤝 Contributing

This is a private project for 11Binary. For questions or issues, contact the development team.

## 📄 License

MIT

## 🔗 Related Projects

- **clio_shared**: Logging library
- **Mercury.Shared**: Shared types and settings
- **olympus_data_nosql_couchdb**: Production implementation example

## 📞 Support

For implementation questions, refer to:
1. [Clio Implementation Guide](https://github.com/trooks1017/clio_shared/blob/main/IMPLEMENTATION_GUIDE.md)
2. This project's source code as a working example
3. Development team

---

**Built with ❤️ using Clio.Shared logging**
