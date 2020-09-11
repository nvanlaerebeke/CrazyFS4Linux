using CrazyFS.CommandLine;
using StorageBackend;
using StorageType.Passthrough;

namespace CrazyFS {

    internal class CrazyFSStart {

        internal int Start(string[] pArgs) {
            var o = new Input().Get(pArgs);
            return new CrazyFSService(new StorageBackendFactory().CreateWindowsStorageBackend<PassthroughStorage>(o.SourcePath), o).Run();
        }
    }
}