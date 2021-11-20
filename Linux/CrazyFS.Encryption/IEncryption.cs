using System;

namespace CrazyFS.Encryption
{
    public interface IEncryption
    {
        byte[] Encrypt(byte[] data);
        byte[] Decrypt(byte[] data);

        string DecryptString(string value);
        string EncryptString(string value);
    }
}