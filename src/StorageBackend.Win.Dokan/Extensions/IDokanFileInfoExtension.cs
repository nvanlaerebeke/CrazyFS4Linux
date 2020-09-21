using DokanNet;
using StorageBackend.IO;

namespace StorageBackend.Win.Dokan {

    public static class IDokanFileInfoExtension {

        public static IFSEntryPointer GetFSEntryPointer(
            this IDokanFileInfo pInfo
        ) {
            if (pInfo?.Context != null) {
                return pInfo.Context as IFSEntryPointer;
            }
            return null;
        }
    }
}