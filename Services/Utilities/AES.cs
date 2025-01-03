using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Services.Utilities
{
    public class AES
    {
        public static string Decrypt(string encryptedString)
        {
            try
            {
                // Get the bytes of the string
                var bytesToBeDecrypted = Convert.FromBase64String(encryptedString);
                var passwordBytes = Encoding.UTF8.GetBytes("SaltBytes");
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                var bytesDecrypted = AESDecrypt(bytesToBeDecrypted, passwordBytes);
                return Encoding.UTF8.GetString(bytesDecrypted);
            }
            catch (Exception e)
            {
                throw new Exception("Decrypt Encrypted Characters Failed!", e);
            }
        }


        public static string Encrypt(string password)
        {
            try
            {
                // Get the bytes of the string
                byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(password);
                byte[] passwordBytes = Encoding.UTF8.GetBytes("SaltBytes");

                // Hash the password with SHA256
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

                return Convert.ToBase64String(bytesEncrypted);

            }
            catch (Exception e)
            {
                throw new Exception("Encrypt Characters Failed!", e);
            }
        }

        private static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            // The salt bytes must be at least 8 bytes.
            var saltBytes = new byte[] {1, 2, 3, 4, 5, 6, 7, 8};

            using var ms = new MemoryStream();
            using var aes = new RijndaelManaged {KeySize = 256, BlockSize = 128};

            var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);

            aes.Mode = CipherMode.CBC;

            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                cs.Close();
            }

            var encryptedBytes = ms.ToArray();

            return encryptedBytes;
        }

        private static byte[] AESDecrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            var saltBytes = new byte[] {1, 2, 3, 4, 5, 6, 7, 8};

            using var ms = new MemoryStream();
            using var aes = new RijndaelManaged {KeySize = 256, BlockSize = 128};

            var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);

            aes.Mode = CipherMode.CBC;

            using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                cs.Close();
            }

            var decryptedBytes = ms.ToArray();

            return decryptedBytes;
        }
    }
}