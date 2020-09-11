using System.Collections;

namespace StorageType.Passthrough.IO {

    internal class DirectoryEntryComparer : IComparer {

        public int Compare(object x, object y) => string.Compare((string)((DictionaryEntry)x).Key, (string)((DictionaryEntry)y).Key);
    }
}