using System.Security.Cryptography;
using System.Text;

namespace Core.Helpers.Cryptography
{
    public static class SHACryptographyHelper
    {
        public static string SHA256Encrypt(string clearText)
        {
            string salt = GenerateSalt();

            return HashSHA256Data(clearText, salt);
        }

        public static bool ComparePasswords(string pasword, string hashPassword)
        {
            string salt = hashPassword.Split(".").First();
            return hashPassword.Equals(HashSHA256Data(pasword, salt));
        }

        public static string GenerateSalt()
        {
            int saltLength = 32; // Độ dài của salt (32 byte trở lên)
            byte[] saltBytes = new byte[saltLength];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            return Convert.ToBase64String(saltBytes);
        }

        public static string HashSHA256Data(string data, string salt)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] saltBytes = Convert.FromBase64String(salt);

            byte[] combinedBytes = new byte[dataBytes.Length + saltBytes.Length];
            Buffer.BlockCopy(dataBytes, 0, combinedBytes, 0, dataBytes.Length);
            Buffer.BlockCopy(saltBytes, 0, combinedBytes, dataBytes.Length, saltBytes.Length);

            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                return $"{salt}.{Convert.ToBase64String(hashBytes)}";
            }
        }
    }
}
