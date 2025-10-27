namespace ExpenseTrackerApp.Services.IHttpClient;

public sealed class WebClientOptions
{
    public Uri BaseAddress { get; set; }

    public Dictionary<string, string> CustomHeaders { get; set; }

    public long? MaxResponseContentBufferSize { get; set; }

    public TimeSpan? Timeout { get; set; }
}
