using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Settings
{
    public class InternalLogSetting
    {
        /// <summary>
        /// Nếu false => Không log
        /// </summary>
        public bool Enable { get; set; } = false;

        /// <summary>
        /// Chỉ cần pass qua 1 config thì sẽ được phép log
        /// </summary>
        public List<LogSetting> LogSettings { get; set; } = new List<LogSetting>();
    }

    public class LogSetting
    {
        /// <summary>
        /// Ưu tiên số 1
        /// Nếu empty => không chặn method
        /// </summary>
        public List<string> DenyHeaders { get; set; } = new List<string>();

        /// <summary>
        /// Ưu tiên số 2
        /// Nếu empty => cho phép all method
        /// </summary>
        public List<string> AllowHeaders { get; set; } = new List<string>();

        /// <summary>
        /// Ưu tiên số 3
        /// HttpMethod: GET/POST/PUT/DELETE/TRACE/HEAD/OPTIONS/PATCH
        /// Nếu empty => không chặn method
        /// </summary>
        public List<string> DenyMethods { get; set; } = new List<string>();

        /// <summary>
        /// Ưu tiên số 4
        /// HttpMethod: GET/POST/PUT/DELETE/TRACE/HEAD/OPTIONS/PATCH
        /// Nếu empty => cho phép all method
        /// </summary>
        public List<string> AllowMethods { get; set; } = new List<string>();

        /// <summary>
        /// Ưu tiên số 5
        /// StatusCode: 200, 400, 500 ...
        /// Nếu empty => không chặn all 
        /// </summary>
        public List<int> DenyStatusCodes { get; set; } = new List<int>();

        /// <summary>
        /// Ưu tiên số 6
        /// StatusCode: 200, 400, 500 ...
        /// Nếu empty => cho phép all
        /// </summary>
        public List<int> AllowStatusCodes { get; set; } = new List<int>();

        /// <summary>
        /// Ưu tiên số 7
        /// URL contain
        /// Nếu empty => không chặn all 
        /// </summary>
        public List<string> DenyPaths { get; set; } = new List<string>();

        /// <summary>
        /// Ưu tiên số 8
        /// URL contain
        /// Nếu empty => cho phép all
        /// </summary>
        public List<string> AllowPaths { get; set; } = new List<string>();
    }
}
