using System.Text;

namespace SimpleProxy.Infrastructure;

public class RequestSerializer
{
    public async Task<string> SerializeRequest(HttpContext context)
    {
        string requestContent;
        await using Stream receiveStream = context.Request.Body;
        using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
        {
            requestContent = await readStream.ReadToEndAsync();
        }

        return requestContent;
    }
}