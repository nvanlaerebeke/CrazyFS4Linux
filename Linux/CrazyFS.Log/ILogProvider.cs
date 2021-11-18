using Serilog;

namespace CrazyFS.Log
{
    public interface ILogProvider
    {
        ILogger Get();
    }
}