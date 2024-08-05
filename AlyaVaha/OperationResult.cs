using Reinforced.Typings.Attributes;

namespace AlyaVaha
{
    [TsInterface]
    public class OperationResult
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public string? Data { get; set; }

        public OperationResult(string message, bool success, string? data = null)
        {
            Message = message;
            Success = success;
            Data = data;
        }
    }
}
