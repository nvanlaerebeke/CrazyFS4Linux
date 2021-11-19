using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using CrazyFS.Passthrough.Linux.Interfaces;

namespace CrazyFS.Passthrough.Linux.Extensions
{
    public static class FileInfoFactoryExtensions
    {
        public static IFileInfo GetFromFileInfo(this IFileInfoFactory fileInfoFactory, IFileInfo info)
        {
            if (fileInfoFactory is not ILinuxFileInfoFactory file) throw new Exception("IFileInfoFactory is not the linux version");
            return file.FromFileInfo(info);
        }

        public static IEnumerable<IFileInfo> GetFromFileInfos(this IFileInfoFactory fileInfoFactory,
            IEnumerable<IFileInfo> infos)
        {
            return infos.ToList().ConvertAll(info => fileInfoFactory.GetFromFileInfo(info));
        }
    }
}