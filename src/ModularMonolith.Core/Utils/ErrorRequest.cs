namespace ModularMonolith.Core.Utils;

public class ErrorRequest
{
    public bool Success { get; set; }
    public IEnumerable<string> Message { get; set; }

    public ErrorRequest() { }

    public ErrorRequest(IEnumerable<string> message)
    {
        Success = false;
        Message = message;
    }
    public ErrorRequest(string message)
    {
        Success = false;
        Message = [message];
    }
}
