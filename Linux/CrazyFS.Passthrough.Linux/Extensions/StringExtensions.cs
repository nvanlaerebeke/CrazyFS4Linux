using System.IO;

// ReSharper disable once CheckNamespace
namespace CrazyFS.Passthrough.Linux
{
    public static class StringExtensions
    {
        public static string GetPath(this string path, string source)
        {
            return Path.Combine(source, path.Trim('/'));
        }
        
        public static string GetMountedPath(this string path, string source, string destination)
        {
            return path.StartsWith(source) ? Path.Combine(destination, path.Substring(source.Length).Trim('/')) : path;
        }
        
        public static string GetRealPath(this string path, string source, string destination)
        {
            return path.StartsWith(destination) ? Path.Combine(source, path.Substring(destination.Length).Trim('/')) : path;
        }
    }
}