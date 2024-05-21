//------------------------------------------------
// Author:                      Nhan Phan
//
// Copyright 2021
//------------------------------------------------

using System.Security.Cryptography;
using System.Text;

namespace Core.Helpers
{
    public static class CryptographyHelper
    {
        private const string EncryptionKey = "BASE-NET8-PLATFORM@";

        /// <summary>
        /// Encrypt.
        /// </summary>
        /// <param name="clearText">clear text</param>
        /// <returns>encrypted text</returns>
        public static string Encrypt(string clearText)
        {
            var result = clearText;
            try
            {
                var clearBytes = Encoding.Unicode.GetBytes(clearText);
                using var encryptor = Aes.Create();
                var pdb = new Rfc2898DeriveBytes(EncryptionKey,
                    new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using var ms = new MemoryStream();
                using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(),
                    CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                result = Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception)
            {
                result = string.Empty;
            }
            return result;
        }

        /// <summary>
        /// Decrypt
        /// </summary>
        /// <param name="cipherText">cipher text</param>
        /// <returns>clear text</returns>
        public static string Decrypt(string cipherText)
        {
            var result = string.Empty;
            try
            {
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using var encryptor = Aes.Create();
                var pdb = new Rfc2898DeriveBytes(EncryptionKey,
                    new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using var ms = new MemoryStream();
                using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(),
                    CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                result = Encoding.Unicode.GetString(ms.ToArray());
            }
            catch (Exception)
            {
                result = string.Empty;
            }

            return result;
        }

        public static string CreateRandomPassword(int length = 8)
        {
            // Tạo một chuỗi ký tự, số cho phép trong mật khẩu
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();

            //Chọn một ký tự ngẫu nhiên tại một thời điểm từ chuỗi và tạo một mảng ký tự
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }

        //public static string Md5(string s)
        //{
        //    try
        //    {
        //        MD5 md5 = MD5CryptoServiceProvider.Create();
        //        byte[] dataMd5 = md5.ComputeHash(Encoding.Default.GetBytes(s));
        //        StringBuilder sb = new StringBuilder();
        //        for (int i = 0; i < dataMd5.Length; i++)
        //            sb.AppendFormat("{0:x2}", dataMd5[i]);
        //        return sb.ToString();
        //    }
        //    catch
        //    {
        //        return "";
        //    }
        //}

    }
}
