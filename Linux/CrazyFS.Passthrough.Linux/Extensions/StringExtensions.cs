using System.IO;

// ReSharper disable once CheckNamespace
namespace CrazyFS.FileSystem
{
    public static class StringExtensions
    {
        public static string GetPath(this string path, string source)
        {
            return Path.Combine(source, path.Trim('/'));
        }
        
        public static string GetMountedPath(this string path, string source, string destination)
        {
            if (path.StartsWith(source))
            {
                return Path.Combine(destination, path.Substring(source.Length).Trim('/'));
            }
            return path;
        }
        
        public static string GetRealPath(this string path, string source, string destination)
        {
            if (path.StartsWith(destination))
            {
                return Path.Combine(source, path.Substring(destination.Length).Trim('/'));
            }
            return path;
        }
    }
}