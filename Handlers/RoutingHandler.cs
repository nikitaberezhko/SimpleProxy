using System.Text;
using SimpleProxy.Infrastructure;

namespace SimpleProxy.Handlers;

public class RoutingHandler(
    IHttpClientFactory factory,
    RequestSerializer requestSerializer, 
    Router router)
{
    public async Task Routing(HttpContext context)
    {
        var client = factory.CreateClient();

        var requestContent = await requestSerializer.SerializeRequest(context);
        
        using StringContent jsonContent = new(
            requestContent,
            Encoding.UTF8,
            "application/json");

        var routedRequest = await router.CreateRoutedRequest(context, jsonContent);
        
        using var response = await client.SendAsync(routedRequest);

        await SendClientResponse(context, response);
    }

    private async Task SendClientResponse(HttpContext context,
        HttpResponseMessage serviceResponse)
    {
        var jsonResponse = await serviceResponse.Content.ReadAsStringAsync();
        context.Response.StatusCode = (int)serviceResponse.StatusCode;
        foreach (var header in serviceResponse.Headers)
        {
            context.Response.Headers.Append(header.Key, header.Value.ToString());
        }
        
        context.Response.ContentType = "application/json";
        await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
    }
}