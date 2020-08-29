using CrazyFS.CommandLine;
using Fsp;
using StorageBackend;
using StorageBackend.Win;
using StorageType.Passthrough;
using System;

namespace CrazyFS {
    internal class CrazyFSService : Service {
        private FileSystemHost _Host;

        public CrazyFSService() : base("CrazyFS") {
        }

        protected override void OnStart(string[] Args) {
            try {
                var o = new Input().Get(Args);
                _Host = VolumeManager.Mount(
                    new StorageBackendFactory().CreateWindowsStorageBackend(new PassthroughStorage(o.SourcePath)),
                    o.MountPoint,
                    null,
                    true,
                    o.DebugFlags
                );
                _ = FileSystemHost.SetDebugLogFile(o.LogFile);
            } catch (Exception ex) {
                Log(EVENTLOG_ERROR_TYPE, string.Format("{0}", ex.Message));
                throw;
            }
        }
        protected override void OnStop() {
            _Host.Unmount();
            _Host = null;
        }
    }
}
