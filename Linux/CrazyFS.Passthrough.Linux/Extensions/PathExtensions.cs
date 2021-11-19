using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using CrazyFS.Passthrough.Linux.Interfaces;
using Mono.Unix.Native;

namespace CrazyFS.Passthrough.Linux.Extensions
{
    public static class PathExtensions
    {
        public static void GetExtendedAttribute(this IPath pathInterface, string path, string name, byte[] value, out int bytesWritten)
        {
            if (pathInterface is not ILinuxPath pathWrapper) throw new Exception("IPath is not the linux version");
            pathWrapper.GetExtendedAttribute(path, name, value, out bytesWritten);
        }

        public static string[] ListExtendedAttributes(this IPath pathInterface, string path)
        {
            if (pathInterface is not ILinuxPath pathWrapper) throw new Exception("IPath is not the linux version"); 
            return pathWrapper.ListExtendedAttributes(path);
        }

        public static void RemoveExtendedAttributes(this IPath pathInterface, string path, string name)
        {
            if (pathInterface is not ILinuxPath pathWrapper) throw new Exception("IPath is not the linux version");
            pathWrapper.RemoveExtendedAttributes(path, name);
        }

        public static void SetExtendedAttributes(this IPath pathInterface, string path, string name, byte[] value, XattrFlags flags)
        {
            if (pathInterface is not ILinuxPath pathWrapper) throw new Exception("IPath is not the linux version");
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
            if (pathWrapper is not ILinuxPath obj) throw new Exception("IPath is not the linux version");
            return obj.HasAccess(path, modes);
        }

        public static void Chmod(this IPath pathWrapper, string path, FilePermissions permissions)
        {
            if (pathWrapper is not ILinuxPath obj) throw new Exception("IPath is not the linux version");
            obj.Chmod(path, permissions);
        }

        public static void Chown(this IPath pathWrapper, string path, uint uid, uint gid)
        {
            if (pathWrapper is not ILinuxPath obj) throw new Exception("IPath is not the linux version");
            obj.Chown(path, uid, gid);
        }

        public static void CreateHardLink(this IPath pathWrapper, string from, string to)
        {
            if (pathWrapper is not ILinuxPath obj) throw new Exception("IPath is not the linux version");

            if (!HasAccess(pathWrapper, from, AccessModes.R_OK) || !HasAccess(pathWrapper, Path.GetDirectoryName(to), AccessModes.W_OK)) {
                throw new UnauthorizedAccessException();
            }
            obj.CreateHardLink(from, to);
        }

        public static void CreateSymLink(this IPath pathWrapper, string from, string to)
        {
            if (pathWrapper is not ILinuxPath obj) throw new Exception("IPath is not the linux version");
            obj.CreateSymLink(from, to);
        }
        
        public static IFileSystemInfo GetFromFileSystemInfo(this IPath pathInfoFactory, IFileSystemInfo info)
        {
            if (pathInfoFactory is not ILinuxPath obj) throw new Exception("IPath is not the linux version");
            if ((info.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                return pathInfoFactory.FileSystem.DirectoryInfo.GetFromDirectoryInfo(info as IDirectoryInfo);    
            }
            return pathInfoFactory.FileSystem.FileInfo.GetFromFileInfo(info as IFileInfo);
        }
        
        public static IEnumerable<IFileSystemInfo> GetFromFileSystemInfos(this IPath pathWrapper, IEnumerable<IFileSystemInfo> infos)
        {
            if (pathWrapper is not ILinuxPath obj) throw new Exception("IPath is not the linux version");
            return infos.ToList().ConvertAll(info => pathWrapper.GetFromFileSystemInfo(info));
        }
        
        public static string GetSymlinkTarget(this IPath pathWrapper, string path)
        {
            if (pathWrapper is not ILinuxPath obj) throw new Exception("IPath is not the linux version");
            return obj.GetSymlinkTarget(path);
        }
    }
}