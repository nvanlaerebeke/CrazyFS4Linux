using DokanNet;
using DokanNet.Logging;
using System.IO;
using System.Linq;
using static DokanNet.FormatProviders;
using FileAccess = DokanNet.FileAccess;

namespace StorageBackend.Win.Dokan {

    internal class Log {
        private static readonly ConsoleLogger logger = new ConsoleLogger("[Mirror] ");

        public static NtStatus Trace(string method, string fileName, IDokanFileInfo info, NtStatus result, params object[] parameters) {
#if TRACE
            var extraParameters = parameters != null && parameters.Length > 0
                ? ", " + string.Join(", ", parameters.Select(x => string.Format(DefaultFormatProvider, "{0}", x)))
                : string.Empty;

            logger.Debug(DokanFormat($"{method}('{fileName}', {info}{extraParameters}) -> {result}"));
#endif

            return result;
        }

        public static NtStatus Trace(string method, string fileName, IDokanFileInfo info, FileAccess access, FileShare share, FileMode mode, FileOptions options, FileAttributes attributes, NtStatus result) {
#if TRACE
            logger.Debug(
                DokanFormat(
                    $"{method}('{fileName}', {info}, [{access}], [{share}], [{mode}], [{options}], [{attributes}]) -> {result}"));
#endif

            return result;
        }
    }
}