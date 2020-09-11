using System.IO;
using System.Text.RegularExpressions;

namespace StorageBackend {

    public static class PathNormalizer {
        private static readonly Regex MultipleSlashes = new Regex(@"/{2,}");

        public static string GetDriveLetter(string pPath) {
            return Path.GetPathRoot(pPath);
        }

        public static string ConcatPath(string pPath, string pFileName) => NormalizeAbsolutePath(Path.Combine(pPath, NormalizeRelativePath(pFileName)));

        private static string NormalizeAbsolutePath(string path) {
            path = path.Replace(@"\", @"/");
            path = MultipleSlashes.Replace(path, @"/");
            if (Path.DirectorySeparatorChar == '\\') {
                path = path.Replace('/', Path.DirectorySeparatorChar);
            }

            path = path.TrimEnd(new char[] { Path.DirectorySeparatorChar });

            if (!path.IsNormalized()) {
                path = path.Normalize();
            }
            return path;
        }

        private static string NormalizeRelativePath(string pPath) {
            if (string.IsNullOrEmpty(pPath)) { return string.Empty; }

            pPath = @pPath.Replace(@"\", @"/");
            pPath = MultipleSlashes.Replace(pPath, @"/");
            if (Path.DirectorySeparatorChar == '\\') {
                pPath = pPath.Replace('/', Path.DirectorySeparatorChar);
            }
            pPath = pPath.Trim(new char[] { Path.DirectorySeparatorChar });

            if (!pPath.IsNormalized()) {
                pPath = pPath.Normalize();
            }
            return pPath;
        }
    }
}