namespace Fitomad.Apns.Exceptions;

public class AuthorizationNotSetException : Exception
{
    private const string AuthorizationNotSetMessage = "Environment not set";
    
    public AuthorizationNotSetException() : base(message: AuthorizationNotSetMessage)
    {
    }
}