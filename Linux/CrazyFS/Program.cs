using System.Text;
using CrazyFS.Linux;
using CrazyFS.Log;
using Serilog.Events;

/*using (RedirectFS fs = new RedirectFS ("/mnt/test/source", "/mnt/test/dest")) {
    fs.Start ();
}*/

string _password = "myPassword"; 
byte[] _salt = Encoding.ASCII.GetBytes("42kb$2fs$@#GE$^%gdhf;!M807c5o666");
byte[] _IV;

LogProvider.LogLevel = LogEventLevel.Debug;



using (CrazyFsFileSystem fs = new ("/mnt/test/source", "/mnt/test/dest")) {
    fs.Start();
}

