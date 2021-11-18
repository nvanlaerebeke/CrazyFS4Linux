using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CrazyFS.Encryption
{
    public class FileCrypto
    {
        private const string _password = "myPassword";
        private static readonly byte[] _salt = Encoding.ASCII.GetBytes("42kb$2fs$@#GE$^%gdhf;!M807c5o666");
        private static byte[] _IV;
        private static readonly string _unEncryptedFile = @"/tmp/test/cleartext.yaml";
        private static readonly string _encryptedFile = @"/tmp/test/encrypted.yaml";
        private static readonly string _decrytedFile = @"/tmp/test/decrypted.yaml";

        public static void Encrypt()
        {
            using var sourceStream = new FileStream(_unEncryptedFile, FileMode.OpenOrCreate);
            using var destStream = new FileStream(_encryptedFile, FileMode.OpenOrCreate);
            using var key = new Rfc2898DeriveBytes(_password, _salt);
            using var aes = Aes.Create();
            
            aes.Padding = PaddingMode.ISO10126;
            aes.Key = key.GetBytes(aes.KeySize / 8);
            _IV = aes.IV;
            
            using var cryptStream = new CryptoStream(destStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

            var content = new byte[sourceStream.Length];
            sourceStream.Read(content);
            cryptStream.Write(content);
        }

        public static void Decrypt()
        {
            using var sourceStream = new FileStream(_encryptedFile, FileMode.Open);
            using var destStream = new FileStream(_decrytedFile, FileMode.OpenOrCreate);
            using var aes = Aes.Create();
            using var key = new Rfc2898DeriveBytes(_password, _salt);

            aes.IV = _IV;
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.Padding = PaddingMode.ISO10126;

            using var cryptStream = new CryptoStream(sourceStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            var content = new List<byte>();
            var reading = true;
            do
            {
                var buffer = new byte[4096];
                var bytesRead = cryptStream.Read(buffer);
                if (bytesRead == buffer.Length)
                {
                    content.AddRange(buffer);
                }
                else
                {
                    content.AddRange(buffer[..bytesRead]);
                    reading = false;
                }
            } while (reading);

            destStream.Write(content.ToArray());
        }
    }
}