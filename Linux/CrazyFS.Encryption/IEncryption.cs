using System;

namespace CrazyFS.Encryption
{
    public interface IEncryption: IDisposable
    {
        byte[] Encrypt(byte[] data);
        byte[] Encrypt(string data);
        byte[] Decrypt(byte[] data);
        string DecryptString(byte[] data);
        byte[] GetInitializationVector();
    }
}