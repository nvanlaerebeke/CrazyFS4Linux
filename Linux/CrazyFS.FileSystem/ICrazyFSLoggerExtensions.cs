using CrazyFS.Log;
using Serilog;

namespace CrazyFS.FileSystem;

internal static class ILoggerExtensions
{
    public static void Information(this ILogger logger, CrazyFSRequest msg)
    {
        logger.Information(
            "\nReceived filesystem request {@Name}\n With parameters: \n {@Parameters}", 
            msg.Name, 
            msg.Parameters
        );
    }
    
    public static void Debug(this ILogger logger, CrazyFSRequest msg)
    {
        logger.Debug(
            "\nReceived filesystem request {@Name}\n With parameters: \n {@Parameters}", 
            msg.Name, 
            msg.Parameters
        );    }
    
    public static void Error(this ILogger logger, CrazyFSRequest msg)
    {
        logger.Error(
            "\nReceived filesystem request {@Name}\n With parameters: \n {@Parameters}", 
            msg.Name, 
            msg.Parameters
        );    }
}