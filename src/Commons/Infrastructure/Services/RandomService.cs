using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IRandomService
    {
        string GenerateRandomString(int length);
        string GenerateRandomAlphabet(int length);
        bool ContainsNumber(string input);
    }

    public class RandomStringService : IRandomService
    {
        private static readonly Random _random = new Random();

        public bool ContainsNumber(string input)
        {
            // Biểu thức chính quy để tìm số có ít nhất 1 chữ số
            string pattern = @"\b\d+\b";
            // Tạo đối tượng Regex với biểu thức chính quy
            Regex regex = new Regex(pattern);
            // Tìm kiếm chuỗi khớp với mẫu trong input
            return regex.IsMatch(input);
        }

        public string GenerateRandomAlphabet(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
