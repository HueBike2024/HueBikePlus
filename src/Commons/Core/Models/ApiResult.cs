namespace Core.Models
{
    public class ApiResult<T>
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        //public T ResultObj { get; set; }
    }
}
