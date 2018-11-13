using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DemoUITest.Services.Exceptions
{
    public sealed class HttpConnectorException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string ReasonPhrase { get; }
        public HttpResponseHeaders Headers { get; }
        public HttpMethod HttpMethod { get; }
        public Uri Uri => RequestMessage.RequestUri;
        public HttpRequestMessage RequestMessage { get; }

        public HttpContentHeaders ContentHeaders { get; private set; }

        public string Content { get; private set; }

        public bool HasContent => !string.IsNullOrWhiteSpace(Content);

        HttpConnectorException(HttpRequestMessage message, HttpMethod httpMethod,
                                      HttpStatusCode statusCode, string reasonPhrase, HttpResponseHeaders headers) :
            base(CreateMessage(statusCode, reasonPhrase))
        {
            RequestMessage = message;
            HttpMethod = httpMethod;
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            Headers = headers;
        }

#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public static async Task<HttpConnectorException> Create(HttpRequestMessage message, HttpMethod httpMethod, HttpResponseMessage response)
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var exception = new HttpConnectorException(message, httpMethod, response.StatusCode, response.ReasonPhrase, response.Headers);

            if (response.Content == null)
                return exception;

            try
            {
                exception.ContentHeaders = response.Content.Headers;
                exception.Content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                response.Content.Dispose();
            }
            catch
            {
                // NB: We're already handling an exception at this point, 
                // so we want to make sure we don't throw another one 
                // that hides the real error.
            }

            return exception;
        }

        static string CreateMessage(HttpStatusCode statusCode, string reasonPhrase) =>
            $"Response status code does not indicate success: {(int)statusCode} ({reasonPhrase}).";
    }
}

