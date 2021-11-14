using Serilog;

namespace CrazyFS.Log;

public class LogProvider
{
    private static  ILogger _logger;
    
    public ILogger Get()
    {
        if (_logger == null)
        {
            _logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();    
        }
        return _logger;
    }
}