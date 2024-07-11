using Microsoft.Extensions.Options;
using SimpleProxy.Settings;

namespace SimpleProxy.Infrastructure;

public class Router(IOptions<RouterSettings> routerSettings)
{
    private List<Route> Routes = routerSettings.Value.Routes;
    
    public async Task<HttpRequestMessage> CreateRoutedRequest(
        HttpContext context,
        StringContent jsonContent)
    {
        var requestPath = context.Request.Path.ToString();
        var baseUrl = "/" + requestPath.Split('/')[3];
        
        var routedUrl = GetRoutedUrl(baseUrl, requestPath);
        
        var newRequest = new HttpRequestMessage(
            new HttpMethod(context.Request.Method), 
            routedUrl);
        newRequest.Content = jsonContent;
        
        return newRequest;
    }

    private string GetRoutedUrl(string baseUrl, string requestPath) =>
        Routes
            .First(route => route.BaseUrl == baseUrl)
            .Destination.Url + requestPath;
}