using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace Juto
{
    public static class Crypto
    {
        public static string key = "abc123he";

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="s">string to encrypt</param>
        /// <param name="encryption">which encryption to use.</param>
        /// <returns></returns>
        public static string Encrypt(string s, Encryption encryption)
        {
            CryptoHelper c = new CryptoHelper();
            switch (encryption)
            {
                case Encryption.none:
                    return s;
                case Encryption.AES:
                    return c.AesEncrypt(s, key);
                case Encryption.base64:
                    return c.Base64Encrypt(s);
                case Encryption.base64Reversed:
                    return c.Base64ReverseEncrypt(s);
                case Encryption.Salsa20:
                    return c.salsa20Encrypt(s, key);
                case Encryption.XOR:
                    return c.XorEncryptDecrypt(s, key);
                default:
                    return s;
            }
        }

        /// <summary>
        /// Decrypts a string
        /// </summary>
        /// <param name="s">string to decrypt</param>
        /// <param name="encryption">encryption to use</param>
        /// <returns></returns>
        public static string Decrypt(string s, Encryption encryption)
        {
            CryptoHelper c = new CryptoHelper();
            switch (encryption)
            {
                case Encryption.none:
                    return s;
                case Encryption.AES:
                    return c.AesDecrypt(s, key);
                case Encryption.base64:
                    return c.Base64Decrypt(s);
                case Encryption.base64Reversed:
                    return c.Base64ReverseDecrypt(s);
                case Encryption.Salsa20:
                    return c.salsa20Decrypt(s, key);
                case Encryption.XOR:
                    return c.XorEncryptDecrypt(s, key);
                default:
                    return s;
            }
        }

        public enum Encryption
        {
            none,
            AES,
            base64,
            base64Reversed,
            Salsa20,
            XOR
        };

        public class CryptoHelper
        {


            #region base64

            public string Base64Encrypt(string s)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(s);

                return Convert.ToBase64String(bytes);
            }

            public string Base64Decrypt(string s)
            {
                byte[] bytes = Convert.FromBase64String(s);

                return Encoding.UTF8.GetString(bytes);
            }

            #endregion


            #region base64Reverse

            public string Base64ReverseEncrypt(string s)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(s);

                string r = Convert.ToBase64String(bytes);

                r.Remove(r.Length - 2, 2);

                return Misc.Misc.Reverse(r);
            }

            public string Base64ReverseDecrypt(string s)
            {
                s.Insert(0, "==");
                s = Misc.Misc.Reverse(s);
                byte[] bytes = Convert.FromBase64String(s);

                return Encoding.UTF8.GetString(bytes);
            }

            #endregion

            #region salsa20

            public string salsa20Encrypt(string s, string key)
            {
                throw new Exception("Salsa20 encryption not implemented!");
            }

            public string salsa20Decrypt(string s, string key)
            {
                throw new Exception("Salsa20 encryption not implemented!");
            }

            #endregion

            #region XOR

            public string XorEncryptDecrypt(string s, string key)
            {
                int intKey = key.GetHashCode();
                StringBuilder szInputStringBuild = new StringBuilder(s);
                StringBuilder szOutStringBuild = new StringBuilder(s.Length);
                char Textch;
                for (int iCount = 0; iCount < s.Length; iCount++)
                {
                    Textch = szInputStringBuild[iCount];
                    Textch = (char)(Textch ^ intKey);
                    szOutStringBuild.Append(Textch);
                }
                return szOutStringBuild.ToString();
            }

            #endregion

            #region AES
            public string AesEncrypt(string clearText, string EncryptionKey)
            {
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return clearText;
            }
            public string AesDecrypt(string cipherText, string EncryptionKey)
            {
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            #endregion
        }
    }

}
