using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CrazyFS.Encryption
{
    public class ByteCrypto : IEncryption
    {
        private readonly string _password;

        private readonly byte[] _salt;

        private byte[] _iv;

        private ICryptoTransform _decryptor;

        private ICryptoTransform _encryptor;
        
        /// <summary>
        /// Setup the class that will handle all the cryptographic stuff
        ///
        /// Note that when passing an empty byte array to initialization vector a new one will be generated
        /// If not stored data encrypted with this initialization vector will not be able to decrypted anymore
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="iv">The initialization vecor used by the AES crypto class</param>
        public ByteCrypto(string password, string salt, byte[] iv)
        {
            _password = password;
            _salt = Encoding.UTF8.GetBytes(salt);
            _iv = iv;
        }

        public void Dispose()
        {
            _encryptor?.Dispose();
            _decryptor?.Dispose();
        }

        public byte[] Encrypt(byte[] data)
        {
            return PerformCryptography(data, GetEncryptor());
        }

        public byte[] Decrypt(byte[] data)
        {
            return PerformCryptography(data, GetDecryptor());
        }

        public byte[] Encrypt(string data)
        {
            return PerformCryptography(Encoding.UTF8.GetBytes(data), GetEncryptor());
        }

        public string DecryptString(byte[] data)
        {
            return Encoding.UTF8.GetString(PerformCryptography(data, GetDecryptor()));
        }
        
        public byte[] GetInitializationVector()
        {
            GetAes();
            return _iv;
        }

        private ICryptoTransform GetEncryptor()
        {
            if (_encryptor != null) return _encryptor;
            var aes = GetAes();
            _encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            aes.Dispose();
            return _encryptor;
        }

        private ICryptoTransform GetDecryptor()
        {
            if (_decryptor != null) return _decryptor;
            var aes = GetAes();
            _decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            aes.Dispose();
            return _decryptor;
        }

        private Aes GetAes()
        {
            var aes = Aes.Create();
            using var key = new Rfc2898DeriveBytes(_password, _salt);

            if (_iv.Length != 0) aes.IV = _iv;
            aes.Padding = PaddingMode.ISO10126;
            aes.Key = key.GetBytes(aes.KeySize / 8);
            if (_iv.Length != 0) _iv = aes.IV;

            return aes;
        }

        private static byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using var ms = new MemoryStream();
            using var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write);

            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();

            return ms.ToArray();
        }
    }
}