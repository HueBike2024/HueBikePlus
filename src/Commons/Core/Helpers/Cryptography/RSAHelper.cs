using System.Security.Cryptography;
using System.Text;

namespace Core.Helpers.Cryptography
{
    public static class RSAHelper
    {
        // ***** Asymmetric encryption *****
        public static (RSAParameters publicKey, RSAParameters privateKey) GenerateKeys(int keyLength)
        {
            using var rsa = RSA.Create();
            rsa.KeySize = keyLength;

            return (
                publicKey: rsa.ExportParameters(includePrivateParameters: false),
                privateKey: rsa.ExportParameters(includePrivateParameters: true)
            );
        }

        public static (string publicKeyXmlStr, string privateKeyXmlStr) GenerateKeysStr(int? keyLength = null)
        {
            using var rsa = RSA.Create();
            
            if (keyLength.HasValue)
                rsa.KeySize = keyLength.Value;

            var publicKeyStr = rsa.ToXmlString(false);

            var privateKeyStr = rsa.ToXmlString(true);

            return (publicKeyStr, privateKeyStr);
        }

        public static string EncryptRSA(string plainText, string privateKeyXmlStr)
        {
            using var rsa = RSA.Create();
            rsa.FromXmlString(privateKeyXmlStr);

            using Aes aesAlgorithm = Aes.Create();
            aesAlgorithm.KeySize = 256;
            aesAlgorithm.GenerateKey();
            aesAlgorithm.GenerateIV();

            ICryptoTransform encryptor = aesAlgorithm.CreateEncryptor();
            byte[] encryptedData;
            using(MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) 
                { 
                    using(StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                    encryptedData = ms.ToArray();
                }
            }
            string encryptedText = Convert.ToBase64String(encryptedData);

            string base64Key = Convert.ToBase64String(aesAlgorithm.Key) + "." + Convert.ToBase64String(aesAlgorithm.IV);
            byte[] plainKey = Encoding.UTF8.GetBytes(base64Key);
            byte[] dataKey = rsa.Encrypt(plainKey, RSAEncryptionPadding.OaepSHA256);
            string encryptedKey = Convert.ToBase64String(dataKey);

            encryptedKey = encryptedKey.TrimEnd('=').Replace('+', '-').Replace('/', '_');
            encryptedText = encryptedText.TrimEnd('=').Replace('+', '-').Replace('/', '_');
            string result = $"{encryptedKey}.{encryptedText}";

            return result ;
        }

        public static string DecryptRSA(string cipherText, string privateKeyXmlStr)
        {
            using var rsa = RSA.Create();
            rsa.FromXmlString(privateKeyXmlStr);

            string[] items = cipherText.Split('.');
            string encryptedKey = items[0];
            string encryptedText = items[1];

            encryptedKey = encryptedKey.Replace('_', '/').Replace('-', '+');
            switch(encryptedKey.Length % 4)
            {
                case 2:
                    encryptedKey += "==";
                    break;
                case 3:
                    encryptedKey += "=";
                    break;
            }
            encryptedText = encryptedText.Replace('_', '/').Replace('-', '+');
            switch (encryptedText.Length % 4)
            {
                case 2:
                    encryptedText += "==";
                    break;
                case 3:
                    encryptedText += "=";
                    break;
            }

            byte[] cipherKey = Convert.FromBase64String(encryptedKey);
            byte[] dataKey = rsa.Decrypt(cipherKey, RSAEncryptionPadding.OaepSHA256);
            string base64Key = Encoding.UTF8.GetString(dataKey);

            items = base64Key.Split(".");
            string key = items[0];
            string iv = items[1];

            using Aes aesAlgorithm = Aes.Create();
            aesAlgorithm.Key = Convert.FromBase64String(key);
            aesAlgorithm.IV = Convert.FromBase64String(iv);
            ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor();

            byte[] cipherData = Convert.FromBase64String(encryptedText);
            using (MemoryStream ms = new MemoryStream(cipherData))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }

        public static byte[] EncryptRSA(byte[] data, RSAParameters publicKey)
        {
            using var rsa = RSA.Create();
            rsa.ImportParameters(publicKey);

            var result = rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
            return result;
        }

        public static byte[] DecryptRSA(byte[] data, RSAParameters privateKey)
        {
            using var rsa = RSA.Create();
            rsa.ImportParameters(privateKey);
            return rsa.Decrypt(data, RSAEncryptionPadding.OaepSHA256);
        }
        // ***** End Asymmetric encryption *****
    }
}
