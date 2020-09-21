using DokanNet;

namespace StorageBackend.Win.Dokan {

    public static class FileAccessExtension {
        private const FileAccess DataAccess = FileAccess.ReadData | FileAccess.WriteData | FileAccess.AppendData | FileAccess.Execute | FileAccess.GenericExecute | FileAccess.GenericWrite | FileAccess.GenericRead;
        private const FileAccess DataWriteAccess = FileAccess.WriteData | FileAccess.AppendData | FileAccess.Delete | FileAccess.GenericWrite;

        public static System.IO.FileAccess GetFileAccess(
            this FileAccess pAccess
        ) {
            if ((pAccess & DataAccess) == 0) {
                return System.IO.FileAccess.ReadWrite;
            }
            if ((pAccess & DataWriteAccess) == 0) {
                return System.IO.FileAccess.Read;
            }
            return System.IO.FileAccess.Read;
        }
    }
}