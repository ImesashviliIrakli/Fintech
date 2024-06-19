using System.Text.Json;

namespace Shared.Exceptions;

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public required string Message { get; set; }
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}