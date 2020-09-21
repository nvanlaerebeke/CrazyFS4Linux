using StorageBackend.FileSystem;
using StorageBackend.Volume;
using System.IO;
using System.IO.Abstractions;

namespace StorageType.Passthrough {

    internal class VolumeManager : IVolume {
        private readonly string Source;

        public VolumeManager(string pSource) => Source = pSource;

        public FileSystemFeatures GetFeatures()
            =>
                FileSystemFeatures.CasePreservedNames |
                FileSystemFeatures.CaseSensitiveSearch |
                FileSystemFeatures.PersistentAcls |
                FileSystemFeatures.SupportsRemoteStorage |
                FileSystemFeatures.UnicodeOnDisk;

        public IVolumeInfo GetVolumeInfo() =>
            new VolumeInfo(new DriveInfoWrapper(new FileSystem(), new DriveInfo(Source)), "MyLabel");
    }
}