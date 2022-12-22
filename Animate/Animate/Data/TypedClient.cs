namespace Animate.Data
{
    public class TypedClient : ITypedClient
    {
        public HttpClient _httpClient { get; set; }

        public TypedClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }    

        public void DoSomething()
        {
            // Read some properties to prove the intended HttpClient has been injectd in

            Console.WriteLine($"Base address of injected client: {_httpClient.BaseAddress}");
            Console.WriteLine($"Timeout of injected client: {_httpClient.Timeout}");

            // use ParkSquare.Extensions.Http methods on _httpClient to call API endpoints
        }
    }
}
