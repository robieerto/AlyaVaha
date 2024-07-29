using Reinforced.Typings.Attributes;

namespace AlyaVaha
{
    [TsInterface]
    public class OperationResult
    {
        public string Message { get; set; }
        public bool Success { get; set; }

        public OperationResult(string message, bool success)
        {
            Message = message;
            Success = success;
        }
    }
}
