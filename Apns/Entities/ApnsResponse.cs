namespace Apns.Entities;

public sealed class ApnsResponse
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public ApnsError Error { get; }

    private ApnsResponse(bool isSuccess, ApnsError error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static ApnsResponse Success() => new(true, ApnsError.None);
    public static ApnsResponse Failure(ApnsError error) => new(false, error); 
}