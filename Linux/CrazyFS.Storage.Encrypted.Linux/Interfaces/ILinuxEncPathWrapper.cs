using CrazyFS.Passthrough.Linux.Interfaces;

namespace CrazyFS.FileSystem.Encrypted.Linux.Interfaces
{
    public interface ILinuxEncPathWrapper: ILinuxPath
    {
        string GetDecryptedPath(string path);
        string GetEncryptedPath(string path);
    }
}