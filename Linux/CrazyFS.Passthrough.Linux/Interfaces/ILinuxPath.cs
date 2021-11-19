using System.IO.Abstractions;
using Mono.Unix.Native;

namespace CrazyFS.Passthrough.Linux.Interfaces
{
    public interface ILinuxPath: IPath
    {
        void Chmod(string path, FilePermissions permissions);
        void Chown(string path, uint uid, uint gid);
        void CreateHardLink(string from, string to);
        void CreateSymLink(string from, string to);
        void GetExtendedAttribute(string path, string name, byte[] value, out int bytesWritten);
        bool HasAccess(string path, AccessModes modes);
        string[] ListExtendedAttributes(string path);
        string GetSymlinkTarget(string path);
        void RemoveExtendedAttributes(string path, string name);
        void SetExtendedAttributes(string path, string name, byte[] value, XattrFlags flags);
    }
}