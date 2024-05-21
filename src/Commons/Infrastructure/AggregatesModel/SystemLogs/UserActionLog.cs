namespace Infrastructure.AggregatesModel.SystemLogs
{
    public class UserActionLog
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// TimeStamp
        /// </summary>
        public DateTime TimeStamp { get; set; }
    }
}
