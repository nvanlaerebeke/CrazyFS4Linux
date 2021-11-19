using System.IO.Abstractions;
using Mono.Unix.Native;

namespace CrazyFS.Passthrough.Linux.Interfaces
{
    public interface ILinuxFileInfo : IFileInfo
    {
        string GetRealPath();
    }
}