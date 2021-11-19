using Mono.Unix.Native;

namespace CrazyFS.Passthrough.Linux.Interfaces
{
    public interface ILinuxFile
    {
        void CreateSpecialFile(string path, FilePermissions mode, ulong rdev);
    }
}