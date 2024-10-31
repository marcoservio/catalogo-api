namespace APICatalogo.Logging;

public class CustomerLogger(string loggerName, CustomLoggerProviderConfiguration loggerConfig) : ILogger
{
    private readonly string _loggerName = loggerName;
    private readonly CustomLoggerProviderConfiguration _loggerConfig = loggerConfig;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == _loggerConfig.LogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string mensagem = $"{logLevel}: {eventId.Id} - {formatter(state, exception)}";

        EscreverTextoNoArquivo(mensagem);
    }

    private void EscreverTextoNoArquivo(string mensagem)
    {
        string caminho = @"C:\Temp\api_log.txt";

        using (StreamWriter stream = new StreamWriter(caminho, true))
        {
            try
            {
                stream.WriteLine(mensagem);
                stream.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
