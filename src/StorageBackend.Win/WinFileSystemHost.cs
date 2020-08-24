using Fsp;

namespace StorageBackend.Win {
    public class WinFileSystemHost : FileSystemHost { //, IFileSystemHost {
        public WinFileSystemHost(FileSystemBase FileSystem) : base(FileSystem) {
        }
    }
}
