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

        /// <summary>
        ///     Setup the class that will handle all the cryptographic stuff
        ///     Note that when passing an empty byte array to initialization vector a new one will be generated
        ///     If not stored data encrypted with this initialization vector will not be able to decrypted anymore
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        public ByteCrypto(string password, byte[] salt)
        {
            _password = password;
            _salt = salt;
        }

        public byte[] Encrypt(byte[] data)
        {
            var encrypted = PerformCryptography(data, GetEncryptor(out var iv));
            var result = new byte[encrypted.Length + 16];
            Array.Copy(iv, result, 16);
            Array.Copy(encrypted, 0, result, 16, encrypted.Length);
            return result;
        }

        public byte[] Decrypt(byte[] data)
        {
            return PerformCryptography(data[16..], GetDecryptor(data[..16]));
        }

        public string DecryptString(string value)
        {
            var source = Convert.FromBase64String(value.Replace('_', '/'));
            return Encoding.UTF8.GetString(PerformCryptography(source[16..], GetDecryptor(source[..16])));
        }

        public string EncryptString(string value)
        {
            var encrypted = PerformCryptography(Encoding.UTF8.GetBytes(value), GetEncryptor(out var iv));
            var result = new byte[16 + encrypted.Length];
            Array.Copy(iv, result, 16);
            Array.Copy(encrypted, 0, result, 16, encrypted.Length);
            return Convert.ToBase64String(result).Replace('/', '_');;
        }

        private ICryptoTransform GetEncryptor(out byte[] iv)
        {
            var aes = GetAes();
            iv = aes.IV;
            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            aes.Dispose();
            return encryptor;
        }

        private ICryptoTransform GetDecryptor(byte[] iv)
        {
            var aes = GetAes(iv);
            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            aes.Dispose();
            return decryptor;
        }

        private Aes GetAes()
        {
            return GetAes(Array.Empty<byte>());
        }

        private Aes GetAes(byte[] iv)
        {
            var aes = Aes.Create();
            using var key = new Rfc2898DeriveBytes(_password, _salt);

            if (iv.Length == 16) aes.IV = iv;

            aes.Padding = PaddingMode.ISO10126;
            aes.Key = key.GetBytes(aes.KeySize / 8);

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