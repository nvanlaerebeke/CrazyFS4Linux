using StorageBackend.FileSystem;

namespace StorageBackend.Volume {

    public interface IVolume {

        FileSystemFeatures GetFeatures();

        IVolumeInfo GetVolumeInfo();
    }
}