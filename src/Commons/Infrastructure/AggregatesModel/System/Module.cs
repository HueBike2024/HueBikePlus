using Core.Models.Base;

namespace Infrastructure.AggregatesModel.System
{
    public class Module : BaseEntity
    {
        public string Name { get; set; }
        public string Segment { get; set; }
        public bool IsActive { get; set; }
        public string Message { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsLog { get; set; }
        public string StatusCodes { get; set; }     // 200,400,500,...

        public Module()
        {

        }

        public Module(string name, string segment, bool isActive, string message, DateTime? startDate, DateTime? endDate, bool isLog = false, string statusCodes = null)
        {
            Name = name;
            Segment = segment;
            IsActive = isActive;
            Message = message;
            StartDate = startDate;
            EndDate = endDate;
            IsLog = isLog;
            StatusCodes = statusCodes;
        }

        public static void Update(ref Module module, string name, string segment, bool isActive, string message, DateTime? startDate, DateTime? endDate, bool isLog = false, string statusCodes = null) 
        {
            module.Name = name;
            module.Segment = segment;
            module.IsActive = isActive;
            module.Message = message;
            module.StartDate = startDate;
            module.EndDate = endDate;
            module.IsLog = isLog;
            module.StatusCodes = statusCodes;
        }
    }
}
