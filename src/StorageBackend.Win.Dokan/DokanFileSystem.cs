using DokanNet;
using System.Collections.Generic;

namespace StorageBackend.Win.Dokan {

    public class DokanFileSystem<T> : IVolumeActions where T : IStorageType, new() {
        private readonly IDokanOperations Backend;
        private readonly List<char> MountPoints = new List<char>();

        public DokanFileSystem(string pSource) => Backend = new DokanBackend<T>(pSource);

        public void Mount(string pMountPoint, byte[] pSecurityDescriptor, bool pSynchronized, uint pDebugLog, string pLogFile) {
            Backend.Mount(pMountPoint, DokanOptions.FixedDrive);
            MountPoints.Add(pMountPoint[0]);
        }

        public void UnMount()
        => MountPoints.ForEach(m => {
            _ = DokanNet.Dokan.Unmount(m);
            _ = MountPoints.Remove(m);
        });
    }
}