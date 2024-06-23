namespace Shared.Exceptions;

public class ExpiryDateException : Exception
{
    public ExpiryDateException(string message) : base(message) { }
}