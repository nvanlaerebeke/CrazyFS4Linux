using CrazyFS.Linux;
using CrazyFS.Log;
using Serilog.Events;

/*using (RedirectFS fs = new RedirectFS ("/mnt/test/source", "/mnt/test/dest")) {
    fs.Start ();
}*/
LogProvider.LogLevel = LogEventLevel.Debug;

using (CrazyFsFileSystem fs = new ("/mnt/test/source", "/mnt/test/dest")) {
    fs.Start();
}