using CrazyFS.Passthrough.Linux.Interfaces;

namespace CrazyFS.FileSystem.Encrypted.Linux.Interfaces
{
    public interface ILinuxEncFileSystemInfo : ILinuxFileSystemInfo
    {
        string GetEncryptedName();  
    } 
}