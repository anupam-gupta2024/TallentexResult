using System.Net;

namespace TallentexResult.Servies
{
    public class HttpService
    {
        public static async Task<string> requestAsync(string url_)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;  // Specifies the Transport Layer Security (TLS) 1.2 security protocol
            using (var httpClient = new HttpClient())
            {
                // Use the await keyword to perform a non-blocking GET request
                string result = await httpClient.GetStringAsync(url_).ConfigureAwait(false);    // use ConfigureAwait(false) in library code to avoid potential deadlocks and improve performance

                // Return the result when the request is completed
                return result;
            }
        }
    }
}
