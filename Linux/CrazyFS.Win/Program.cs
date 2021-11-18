using CrazyFS.Win;
using DokanNet;

CrazyFSFileSystem fs = new(@"C:\", @"N:\");
fs.Mount(@"N:\", DokanOptions.DebugMode | DokanOptions.EnableNotificationAPI, /*threadCount=*/5);