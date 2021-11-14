using System.Collections.Generic;
using CrazyFS.Log;
using Serilog;

namespace CrazyFS.FileSystem
{
    class CrazyFSRequest
    {
        private readonly CrazyFSRequestName _name;
        private readonly KeyValuePair<string, string>[] _parameters;
        private static ILogger _logger;

        private static ILogger Logger => _logger ??= new LogProvider().Get();

        public CrazyFSRequest(CrazyFSRequestName name, KeyValuePair<string, string>[] parameters)
        {
            _name = name;
            _parameters = parameters;
            _name = name;
            _parameters = parameters;
        }

        internal CrazyFSRequest Log()
        {
            Logger.Information(
                "\nReceived filesystem request {@Name}\n With parameters: \n {@Parameters}",
                _name,
                _parameters
            );
            return this;
        }

        internal CrazyFSRequest Log(Result result)
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