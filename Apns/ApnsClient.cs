using Apns.Entities;
using Apns.Entities.Notification;

namespace Apns;

public interface IApnsClient
{
   Task<ApnsResponse> SendAsync(Notification notification, string deviceToken); 
}

public class ApnsClient: IApnsClient
{
    private HttpClient _httpClient;
    
    internal ApnsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<ApnsResponse> SendAsync(Notification notification, string deviceToken)
    {
        throw new NotImplementedException();
    }
}