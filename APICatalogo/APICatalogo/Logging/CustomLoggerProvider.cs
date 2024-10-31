using System.Collections.Concurrent;

namespace APICatalogo.Logging;

public class CustomLoggerProvider(CustomLoggerProviderConfiguration loggerConfig) : ILoggerProvider
{
    private readonly CustomLoggerProviderConfiguration _loggerConfig = loggerConfig;
    private readonly ConcurrentDictionary<string, CustomerLogger> _loggers = new();

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => new CustomerLogger(name, _loggerConfig));
    }

    public void Dispose()
    {
        _loggers.Clear();
    }
}
