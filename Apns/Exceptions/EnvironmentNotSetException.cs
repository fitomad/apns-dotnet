namespace Fitomad.Apns.Exceptions;

public class EnvironmentNotSetException : Exception
{
    private const string EnvironmentMessage = "Environment not set";
    
    public EnvironmentNotSetException() : base(message: EnvironmentMessage)
    {
    }
}