using System;
using System.IO;
using System.IO.Abstractions;
using Mono.Unix;
using Mono.Unix.Native;

// ReSharper disable once CheckNamespace
namespace CrazyFS.Passthrough.Linux
{
    public static class PathExtensions
    {
        public static void GetExtendedAttribute(this IPath pathInterface, string path, string name, byte[] value, out int bytesWritten)
        {
            if (pathInterface is not LinuxPathWrapper pathWrapper) throw new Exception("IPath is not the linux version");
            pathWrapper.GetExtendedAttribute(path, name, value, out bytesWritten);
        }

        public static string[] ListExtendedAttributes(this IPath pathInterface, string path)
        {
            if (pathInterface is LinuxPathWrapper pathWrapper)
            {
                return pathWrapper.ListExtendedAttributes(path);
            }

            throw new Exception("IPath is not the linux version");
        }

        public static void RemoveExtendedAttributes(this IPath pathInterface, string path, string name)
        {
            if (pathInterface is not LinuxPathWrapper pathWrapper)
            {
                throw new Exception("IPath is not the linux version");
            }
            pathWrapper.RemoveExtendedAttributes(path, name);
        }

        public static void SetExtendedAttributes(this IPath pathInterface, string path, string name, byte[] value, XattrFlags flags)
        {
            if (pathInterface is not LinuxPathWrapper pathWrapper)
            {
                throw new Exception("IPath is not the linux version");
            }
            pathWrapper.SetExtendedAttributes(path, name, value, flags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathWrapper"></param>
        /// <param name="path"></param>
        /// <param name="modes"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static bool HasAccess(this IPath pathWrapper, string path, AccessModes modes)
        {
            if (pathWrapper is not LinuxPathWrapper obj)
            {
                throw new Exception("IPath is not the linux version");
            }
            return obj.HasAccess(path, modes);
        }

        public static void Chmod(this IPath pathWrapper, string path, FilePermissions permissions)
        {
            if (pathWrapper is not LinuxPathWrapper obj) throw new Exception("IPath is not the linux version");
            obj.Chmod(path, permissions);
        }

        public static void Chown(this IPath pathWrapper, string path, uint uid, uint gid)
        {
            if (pathWrapper is not LinuxPathWrapper obj) throw new Exception("IPath is not the linux version");
            obj.Chown(path, uid, gid);
        }

        public static void CreateHardLink(this IPath pathWrapper, string from, string to)
        {
            if (!HasAccess(pathWrapper, from, AccessModes.R_OK) || !HasAccess(pathWrapper, Path.GetDirectoryName(to), AccessModes.W_OK))
            {
                throw new UnauthorizedAccessException();
            }

            if (Syscall.link(pathWrapper.GetFullPath(from), pathWrapper.GetFullPath(to)) == -1)
            {
                throw new Exception();
            }
        }

        public static void CreateSymlink(this IPath pathWrapper, string from, string to)
        {
            if (pathWrapper is not LinuxPathWrapper obj)
            {
                throw new Exception("IPath is not the linux version");
            }
            obj.CreateSymlink(from, to);
        }

        public static string GetSymlinkTarget(this IPath pathWrapper, string path)
        {
            return new UnixFileInfo(pathWrapper.GetFullPath(path)).IsSymbolicLink ? pathWrapper.GetRelativePath(pathWrapper.GetFullPath("/"), UnixPath.GetRealPath(path)) : pathWrapper.GetRelativePath(pathWrapper.GetFullPath("/"), path);
        }
    }
}