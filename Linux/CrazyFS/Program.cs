using CrazyFS.Linux;
using CrazyFS.Log;
using Serilog.Events;

/*using (RedirectFS fs = new RedirectFS ("/mnt/test/source", "/mnt/test/dest")) {
    fs.Start ();
}*/

LogProvider.LogLevel = LogEventLevel.Debug;

var source = "/mnt/test/source";
var source_enc = "/mnt/test/source_enc";
var dest = "/mnt/test/dest";

using (CrazyFsFileSystem fs = new(source_enc, dest))
{
    fs.Start();
}