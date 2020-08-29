using CrazyFS.CommandLine;
using Fsp;
using StorageBackend;
using System;

namespace CrazyFS {
    internal class CrazyFSService : Service {
        private readonly Options Options;
        private readonly IFileSystem FileSystem;

        public CrazyFSService(IFileSystem pFileSystem, Options pOptions) : base("CrazyFS") {
            FileSystem = pFileSystem;
            Options = pOptions;
        }

        protected override void OnStart(string[] Args) {
            try {
                FileSystem.Mount(Options.MountPoint, null, true, Options.DebugFlags, Options.LogFile);
            } catch (Exception ex) {
                Log(EVENTLOG_ERROR_TYPE, string.Format("{0}", ex.Message));
                throw;
            }
        }

        protected override void OnStop() {
            FileSystem.UnMount();
        }
    }
}
