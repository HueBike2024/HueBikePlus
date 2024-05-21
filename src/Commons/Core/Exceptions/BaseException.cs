using System;
using System.Globalization;

namespace Core.Exceptions
{
    public class BaseException : Exception
    {
        public const string ErrorCode = "error_code";
        public BaseException()
        {

        }
        public BaseException(string message) : base(message)
        {
        }

        public BaseException(string message, params object[] args) : base(string.Format(CultureInfo.CurrentCulture,
            message, args))
        {
        }

        public BaseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public BaseException(string message, int code) : base(message)
        {
            Data.Add(ErrorCode, code);
        }
    }
}
