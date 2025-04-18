namespace Fitomad.Apns.Entities;

public sealed class ApnsResponse
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public ApnsError Error { get; }
    public ApnsGuid Guid { get; }

    private ApnsResponse(bool isSuccess, ApnsError error, ApnsGuid guid)
    {
        IsSuccess = isSuccess;
        Error = error;
        Guid = guid;
    }

    public static ApnsResponse Success(ApnsGuid identifiers) => new(true, ApnsError.None, identifiers);
    public static ApnsResponse Failure(ApnsError error) => new(false, error, null); 
}