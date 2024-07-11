namespace SimpleProxy.Infrastructure;

public class Destination
{
    public string Url { get; set; }
    
    public bool RequiresAuthentication { get; set; }
}