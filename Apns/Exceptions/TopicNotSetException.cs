namespace Fitomad.Apns.Exceptions;

public class TopicNotSetException : Exception
{
    private const string TopicMessage = "Topic not set and it's mandatory to set the `apns-topic` header";
    
    public TopicNotSetException() : base(message: TopicMessage)
    {
    }
}