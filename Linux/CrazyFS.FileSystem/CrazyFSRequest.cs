using System.Collections.Generic;
using CrazyFS.Log;
using Serilog;

namespace CrazyFS.FileSystem
{
    public class CrazyFsRequest
    {
        private readonly CrazyFsRequestName _name;
        private readonly KeyValuePair<string, string>[] _parameters;
        private static ILogger _logger;

        private static ILogger Logger => _logger ??= new LogProvider().Get();

        public CrazyFsRequest(CrazyFsRequestName name, KeyValuePair<string, string>[] parameters)
        {
            _name = name;
            _parameters = parameters;
            _name = name;
            _parameters = parameters;
        }

        public CrazyFsRequest Log()
        {
            Logger.Information(
                "\nReceived filesystem request {@Name}\n With parameters: \n {@Parameters}",
                _name,
                _parameters
            );
            return this;
        }

        public CrazyFsRequest Log(Result result)
        {
            Logger.Information(
                "Result for {@Name}\n is {@Result} with native code {@NativeCode}",
                _name,
                result.Status,
                result.NativeCode
            );
            return this;
        }
    }
}