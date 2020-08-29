namespace CrazyFS.CommandLine {
    public class Options : StorageBackend.Options {
        public bool ShowHelp;
        public uint DebugFlags;
        public string LogFile;
        public string MountPoint;
        public string UNCPrefix;
    }
}