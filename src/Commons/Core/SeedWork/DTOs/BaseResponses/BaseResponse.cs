using System;

namespace Core.SeedWork.DTOs.BaseResponses
{
    /// <summary>
    /// Base response
    /// </summary>
    public class BaseResponse
    {
        public Guid? Value { get; set; }
        public string Name { get; set; }
    }
}
