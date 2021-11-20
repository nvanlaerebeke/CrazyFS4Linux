using CrazyFS.Encryption;
using Serilog.Parsing;

namespace CrazyFS.FileSystem.Encrypted.Linux.Extensions
{
    public static class StringExtensions
    {
        public static string Decrypt(this string value, IEncryption encryption)
        {
            return encryption.DecryptString(value);
        }
        public static string Encrypt(this string value, IEncryption encryption)
        {
            return encryption.EncryptString(value);
        }

        public static string GetRelative(this string value, string basePath)
        {
            return value.StartsWith(basePath) ? value.Substring(basePath.Length) : value;
        }
    }
}