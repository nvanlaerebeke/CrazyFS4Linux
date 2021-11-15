using CrazyFS.Log;
using Serilog.Events;

namespace CrazyFS
{
    public class Program
    {
        public static void Main (string[] args)
        {
            /*using (RedirectFS fs = new RedirectFS ("/mnt/test/source", "/mnt/test/dest")) {
                fs.Start ();
            }*/
            LogProvider.LogLevel = LogEventLevel.Debug;
            
            using (CrazyFsFileSystem fs = new ("/mnt/test/source", "/mnt/test/dest")) {
                fs.Start();
            }
        }
    }
}