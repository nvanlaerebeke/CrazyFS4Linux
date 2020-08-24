using System.Collections;

namespace StorageType.Passthrough {
    internal class DirectoryEntryComparer : IComparer {
        public int Compare(object x, object y) {
            return string.Compare(
                (string)((DictionaryEntry)x).Key,
                (string)((DictionaryEntry)y).Key);
        }
    }
}
