namespace StorageBackend {
    public static class PathNormalizer {
        public static string GetDriveLetter(string pPath) {
            return System.IO.Path.GetPathRoot(pPath);
        }

        public static string Normalize(string pPath) {
            return pPath.EndsWith("\\") ? pPath.Substring(0, pPath.Length - 1) : pPath;
        }

        public static string ConcatPath(string pPath, string pFileName) {
            return pPath + pFileName;
        }
    }
}
