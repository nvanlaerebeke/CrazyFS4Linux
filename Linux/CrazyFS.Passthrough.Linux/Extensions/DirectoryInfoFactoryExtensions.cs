using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using CrazyFS.Passthrough.Linux.Interfaces;

namespace CrazyFS.Passthrough.Linux.Extensions
{
    public static class DirectoryInfoFactoryExtensions
    {
        public static IDirectoryInfo GetFromDirectoryInfo(this IDirectoryInfoFactory directoryInfoFactory, IDirectoryInfo info)
        {
            if (directoryInfoFactory is not ILinuxDirectoryInfoFactory dir) throw new Exception("IDirectoryInfoFactory is not the linux version");
            return dir.FromDirectoryInfo(info);
        }

        public static IEnumerable<IDirectoryInfo> GetFromDirectoryInfos(this IDirectoryInfoFactory directoryInfoFactory,
            IEnumerable<IDirectoryInfo> infos)
        {
            return infos.ToList().ConvertAll(info => directoryInfoFactory.GetFromDirectoryInfo(info));
        }
    }
}