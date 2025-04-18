namespace Fitomad.Apns.Exceptions;

public class DuplicatedAuthorizationException : Exception
{
    private const string DuplicatedAuthorizationMessage = "Environment not set";
    
    public DuplicatedAuthorizationException() : base(message: DuplicatedAuthorizationMessage)
    {
    }
}