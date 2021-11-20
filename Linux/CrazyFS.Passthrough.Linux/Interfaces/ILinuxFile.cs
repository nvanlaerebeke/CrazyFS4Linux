using System.IO.Abstractions;
using Mono.Unix.Native;

namespace CrazyFS.Passthrough.Linux.Interfaces
{
    public interface ILinuxFile : IFile
    {
        void CreateSpecialFile(string path, FilePermissions mode, ulong rdev);
    }
}