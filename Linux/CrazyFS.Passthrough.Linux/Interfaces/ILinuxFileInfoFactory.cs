using System.IO.Abstractions;

namespace CrazyFS.Passthrough.Linux.Interfaces
{
    public interface ILinuxFileInfoFactory : IFileInfoFactory
    {
        IFileInfo FromFileInfo(IFileInfo info);
    }
}