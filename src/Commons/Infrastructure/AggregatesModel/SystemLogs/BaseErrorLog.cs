namespace Infrastructure.AggregatesModel.SystemLogs
{
    public class BaseErrorLog
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// MessageTemplate
        /// </summary>
        public string MessageTemplate { get; set; }

        /// <summary>
        /// Level
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// CreatedDate
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Exception
        /// </summary>
        public string Exception { get; set; }
        /// <summary>
        /// UserName
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// UserName
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// UserName
        /// </summary>
        public string StatusCode { get; set; }
        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; set; }
    }
}
