using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Helpers
{
    public static class HMACSHA256Helper
    {
        public static string CreateSignature(string message, string secret)
        {
            var encoding = new UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);

            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashMessage = hmacsha256.ComputeHash(messageBytes);
                return BitConverter.ToString(hashMessage).Replace("-", "").ToLower();
            }
        }

        public static string CreateSignature(Dictionary<string, string> parameters, string secret)
        {
            var sortedParameters = parameters
                .Where(p => p.Key.StartsWith("vpc_") || p.Key.StartsWith("user_"))
                .OrderBy(p => p.Key, StringComparer.Ordinal)
                .ToList();

            var dataString = string.Join("&", sortedParameters.Select(p => $"{p.Key}={p.Value}"));

            return CreateSignature(dataString, secret);
        }

        public static string sha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("X2")); // Chuyển đổi sang chữ cái in hoa
            }
            return hash.ToString();
        }

        static byte[] StringToByteArray(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        static string SortQueryString(string queryString)
        {
            var queryParams = HttpUtility.ParseQueryString(queryString);

            var parameters = queryParams.AllKeys
            .Where(key => key.StartsWith("vpc_") || key.StartsWith("user_"))
            .Select(key => new { Key = key, Value = queryParams[key] });

            var sortedQueryString = string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"));
            return sortedQueryString;
        }

        public static string GetHash(string text, string key)
        {
            //ASCIIEncoding encoding = new ASCIIEncoding();

            //byte[] textBytes = encoding.GetBytes(text);
            //byte[] keyBytes = encoding.GetBytes(key);

            //byte[] hashBytes;

            //using (HMACSHA256 hash = new HMACSHA256(keyBytes))
            //    hashBytes = hash.ComputeHash(textBytes);

            //return BitConverter.ToString(hashBytes).Replace("-", "").ToUpper(); 
            byte[] keyBytes = StringToByteArray(key);

            text = SortQueryString(text);

            using (var hmacSha256 = new HMACSHA256(keyBytes))
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(text);
                byte[] hashBytes = hmacSha256.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public static string ComputeHmacSha256Hash(string data, string key)
        {
            //using (var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            //{
            //    var hashBytes = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(data));
            //    return BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
            //}
            // Chuyển đổi khóa thành mảng byte
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            // Sắp xếp chuỗi truy vấn
            data = SortQueryString(data);

            // Tạo đối tượng HMACSHA256 với khóa
            using (var hmacSha256 = new HMACSHA256(keyBytes))
            {
                // Chuyển đổi dữ liệu đầu vào thành mảng byte
                byte[] inputBytes = Encoding.UTF8.GetBytes(data);

                // Tính toán mã băm HMACSHA256
                byte[] hashBytes = hmacSha256.ComputeHash(inputBytes);

                // Chuyển đổi mảng byte thành chuỗi hex
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
