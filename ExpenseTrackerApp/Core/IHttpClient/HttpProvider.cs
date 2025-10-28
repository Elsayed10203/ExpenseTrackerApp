namespace ExpenseTrackerApp.Services.IHttpClient
{
    public interface IHttpProvider
    {
        public Task<HttpResponseMessage> DeleteAsync(string requestUri);
        public Task<HttpResponseMessage> GetAsync(string requestUri);
        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);

        public Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content);

    }

    public sealed class HttpProvider : IHttpProvider
    {
        private readonly HttpClient client;

        public HttpProvider( )
        {
            client = new HttpClient();
            WebClientOptions options = new WebClientOptions();

            if (options.BaseAddress != null)
                client.BaseAddress = options.BaseAddress;

            if (options.MaxResponseContentBufferSize.HasValue)
                client.MaxResponseContentBufferSize = options.MaxResponseContentBufferSize.Value;

            if (options.Timeout.HasValue)
                client.Timeout = options.Timeout.Value;

            Options = options;
        }

        private WebClientOptions Options { get; }

   
        public Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            var req = new HttpRequestMessage()
            {
                Method = HttpMethod.Delete,
                RequestUri = GetUri(requestUri),
            };

            return SendAsync(req);
        }

        public Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return GetAsync(requestUri, CancellationToken.None);
        }

        public Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken)
        {
            var req = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = GetUri(requestUri),
            };

            return SendAsync(req);
        }
       

        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            var req = new HttpRequestMessage()
            {
                Content = content,
                Method = HttpMethod.Post,
                RequestUri = GetUri(requestUri),
            };

            return SendAsync(req);
        }

 
        public Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)
        {
            var req = new HttpRequestMessage()
            {
                Content = content,
                Method = HttpMethod.Put,
                RequestUri = GetUri(requestUri),
            };

            return SendAsync(req);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
             var customHeaders = Options?.CustomHeaders;
            if (customHeaders != null)
            {
                foreach (var head in customHeaders)
                    client.DefaultRequestHeaders.Add(head.Key, head.Value);
            }
            return await client.SendAsync(request);
        }

        private Uri GetUri(string path)
        {
            var urlStr = client.BaseAddress == null ? path : client.BaseAddress.ToString() + path;
            return new Uri(urlStr);
        }

    }
}
