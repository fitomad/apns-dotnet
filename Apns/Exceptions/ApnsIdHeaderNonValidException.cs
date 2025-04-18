namespace Fitomad.Apns.Exceptions;

public class ApnsIdHeaderNonValidException : Exception
{
    private const string ApnsIdHeaderMessage = "The apns-id header value is not valid.";
    
    public ApnsIdHeaderNonValidException() : base(message: ApnsIdHeaderMessage)
    {
    }
}