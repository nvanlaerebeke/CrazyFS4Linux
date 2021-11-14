using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection.Metadata;
using CrazyFS.Linux;
using Mono.Unix.Native;
using Mono.Unix;

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
                return '/' + Path.Combine(path.Substring(source.Length), destination.Trim('/'));
            }
            return path;
        }
    }
}