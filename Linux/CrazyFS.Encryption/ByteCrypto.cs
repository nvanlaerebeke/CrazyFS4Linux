using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CrazyFS.Encryption
{
    public class ByteCrypto : IDisposable
    {
        private readonly string _password;

        private readonly byte[] _salt;

        private readonly byte[] _iv;

        private ICryptoTransform _encryptor;

        private ICryptoTransform _decryptor;
        
        /*private static string _password = "myPassword";
        private static readonly byte[] _salt = Encoding.ASCII.GetBytes("42kb$2fs$@#GE$^%gdhf;!M807c5o666");
        private static byte[] _IV;*/
        
        public ByteCrypto(string password, string salt, byte[] iv)
        {
            _password = password;
            _salt = Encoding.UTF8.GetBytes(salt);
            _iv = iv;
        }
        
        public byte[] Encrypt(byte[] data)
        {
            return PerformCryptography(data, GetEncrypter());
        }

        public byte[] Decrypt(byte[] data)
        {
            return PerformCryptography(data, GetDecryptor());
        }

        private ICryptoTransform GetEncrypter()
        {
            if (_encryptor != null) return _encryptor;
            
            using var key = new Rfc2898DeriveBytes(_password, _salt);
            using var aes = Aes.Create();

            aes.IV = _iv;
            aes.Padding = PaddingMode.ISO10126;
            aes.Key = key.GetBytes(aes.KeySize / 8);
            
            _encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            return _encryptor;
        }

        private ICryptoTransform GetDecryptor()
        {
            if (_decryptor != null) return _decryptor;
            
            using var aes = Aes.Create();
            using var key = new Rfc2898DeriveBytes(_password, _salt);

            aes.IV = _iv;
            aes.Padding = PaddingMode.ISO10126;
            aes.Key = key.GetBytes(aes.KeySize / 8);
            _decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            return _decryptor;
        }
        
        private static byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using var ms = new MemoryStream();
            using var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write);

            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();

            return ms.ToArray();
        }

        public void Dispose()
        {
            _encryptor.Dispose();
            _decryptor.Dispose();
        }
    }
}