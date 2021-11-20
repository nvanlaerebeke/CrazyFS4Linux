using System.IO.Abstractions;
using CrazyFS.Passthrough.Linux.Interfaces;
using Mono.Unix.Native;

namespace CrazyFS.FileSystem.Encrypted.Linux.Interfaces
{
    public interface ILinuxEncDirectoryWrapper : ILinuxDirectory
    {
        new void CreateDirectory(string path, FilePermissions mode);
    }
}