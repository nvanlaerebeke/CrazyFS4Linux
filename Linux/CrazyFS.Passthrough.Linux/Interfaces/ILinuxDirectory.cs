using System.IO.Abstractions;
using Mono.Unix.Native;

namespace CrazyFS.Passthrough.Linux.Interfaces
{
    public interface ILinuxDirectory : IDirectory
    {
        void CreateDirectory(string path, FilePermissions mode);
    }
}