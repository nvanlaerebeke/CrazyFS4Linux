using System.IO.Abstractions;

namespace CrazyFS.Passthrough.Linux.Interfaces
{
    public interface ILinuxDirectoryInfoFactory : IDirectoryInfoFactory
    {
        IDirectoryInfo FromDirectoryInfo(IDirectoryInfo info);
    }
}