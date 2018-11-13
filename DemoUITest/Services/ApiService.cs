using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using DemoUITest.Services.Exceptions;
using Xamarin.Forms;

namespace DemoUITest.Services
{
    public enum HttpMethodEnum
    {
        Get, Post, Delete, Put
    }

    public interface IHttpConnector
    {
        Task<T> Get<T>(string endpoint, CancellationToken cancellationToken) where T : class;

        Task<TOutput> Post<TOutput>(string endpoint, CancellationToken cancellationToken) where TOutput : class;

        Task<TOutput> Post<TInput, TOutput>(string endpoint, TInput body, CancellationToken cancellationToken) where TInput : class where TOutput : class;

        Task<TOutput> PostFormUrlEncoded<TOutput>(string url, IEnumerable<KeyValuePair<string, string>> postData, CancellationToken cancellationToken);

        Task<byte[]> GetResponseAsByteAsync(string endpoint, CancellationToken cancellationToken);
    }

    public class HttpConnector : IHttpConnector, IDisposable
    {
        readonly HttpClient _client = null;

        public HttpConnector()
        {
            _client = new HttpClient(DependencyService.Get<HttpClientHandler>());
            _client.Timeout = TimeSpan.FromMinutes(5);
        }

        string GetBearer()
        {
            try
            {
                if (_client != null && _client.DefaultRequestHeaders != null && _client.DefaultRequestHeaders.Authorization != null)
                    return _client.DefaultRequestHeaders.Authorization.ToString();
                return "";
            }
            catch
            {
                return "";
            }
        }

        #region GET method

        Task<TOutput> IHttpConnector.Get<TOutput>(string endpoint, CancellationToken cancellationToken)
        {
            return CallWebApiAsync<TOutput>(endpoint, HttpMethodEnum.Get, cancellationToken);
        }
        #endregion

        #region POST method

        Task<TOutput> IHttpConnector.Post<TInput, TOutput>(string endpoint, TInput body, CancellationToken cancellationToken)
        {
            return CallWebApiAsync<TInput, TOutput>(body, endpoint, HttpMethodEnum.Post, cancellationToken);
        }
        #endregion


        async Task<TOutput> CallWebApiAsync<TInput, TOutput>(TInput input,
                                                             string endpoint,
                                                             HttpMethodEnum method,
                                                             CancellationToken cancellationToken) where TInput : class
        {
            return await Task.Run(async () =>
            {
                HttpContent content = null;

                if (input != null)
                {
                    content = await SerializeContent<TInput>(input, endpoint);
                }

                var startupUri = new Uri(endpoint);
                HttpResponseMessage response = null;
                switch (method)
                {
                    case HttpMethodEnum.Get:
                        response = await _client.GetAsync(endpoint, cancellationToken);
                        break;
                    case HttpMethodEnum.Post:
                        response = await _client.PostAsync(endpoint, content, cancellationToken);
                        break;
                    case HttpMethodEnum.Put:
                        response = await _client.PutAsync(endpoint, content, cancellationToken);
                        break;
                    case HttpMethodEnum.Delete:
                        response = await _client.DeleteAsync(endpoint, cancellationToken);
                        break;
                    default:
                        break;
                }
                return await ManageBackendResponse<TOutput>(endpoint, response);
            }, cancellationToken);
        }

        async Task<TOutput> CallWebApiAsync<TOutput>(string endpoint, HttpMethodEnum method, CancellationToken cancellationToken)
        {
            return await Task.Run(async () =>
            {
                HttpContent content = null;

#if DEBUG
                System.Diagnostics.Debug.WriteLine($"{endpoint} Request {method}");
#endif
                var startupUri = new Uri(endpoint);

                HttpResponseMessage response = null;
                switch (method)
                {
                    case HttpMethodEnum.Get:
                        response = await _client.GetAsync(endpoint, cancellationToken);
                        break;
                    case HttpMethodEnum.Post:
                        response = await _client.PostAsync(endpoint, content, cancellationToken);
                        break;
                    case HttpMethodEnum.Put:
                        response = await _client.PutAsync(endpoint, content, cancellationToken);
                        break;
                    case HttpMethodEnum.Delete:
                        response = await _client.DeleteAsync(endpoint, cancellationToken);
                        break;
                    default:
                        break;
                }

                return await ManageBackendResponse<TOutput>(endpoint, response);
            }, cancellationToken);
        }

        async Task<TOutput> ManageBackendResponse<TOutput>(string endpoint, HttpResponseMessage response)
        {
            if (response == null)
            {
                throw new Exception("Null response received");
            }

            var task = response.Content?.ReadAsStringAsync();
            var responseText = await (task ?? Task.FromResult<string>(null));
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"{endpoint}");
            System.Diagnostics.Debug.WriteLine($"Auth headers: {GetBearer()}");
            System.Diagnostics.Debug.WriteLine($"Resonse Status: {response.StatusCode}");
            System.Diagnostics.Debug.WriteLine($"Backend Response: {responseText}");
#endif
            if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotModified)
            {
                if (typeof(TOutput) == typeof(string))
                    return (TOutput)Convert.ChangeType(responseText, typeof(string));
                var result = JsonConvert.DeserializeObject<TOutput>(responseText, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                return result;
            }
            else if (response.StatusCode == HttpStatusCode.NotModified)
            {
                return default(TOutput);
            }
            else
            {
                var ex = await HttpConnectorException.Create(response.RequestMessage, response.RequestMessage.Method, response)
                                            .ConfigureAwait(false);
                throw ex;
            }
        }


        async Task<string> ManageBackendResponse(string endpoint, HttpResponseMessage response)
        {
            if (response == null)
            {
                throw new Exception("Null response received");
            }

            var task = response.Content?.ReadAsStringAsync();
            var responseText = await (task ?? Task.FromResult<string>(null));
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"{endpoint}");
            System.Diagnostics.Debug.WriteLine($"Resonse Status: {response.StatusCode}");
            System.Diagnostics.Debug.WriteLine($"Backend Response: {responseText}");
            System.Diagnostics.Debug.WriteLine($"Auth headers: {GetBearer()}");
#endif
            if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotModified)
            {
                return responseText;
            }
            else
            {
                var ex = await HttpConnectorException.Create(response.RequestMessage, response.RequestMessage.Method, response)
                                           .ConfigureAwait(false);
                throw ex;
            }
        }

        #region Serialization
        private Task<HttpContent> SerializeContent<T>(T body, string endpoint)
            where T : class
        {
            if (body == null)
            {
                return Task.FromResult<HttpContent>(new StringContent(string.Empty));
            }

            if (body.GetType() == typeof(byte[]))
            {
                return Task.FromResult<HttpContent>(new ByteArrayContent(body as byte[]));
            }
            else
            {
                var jsonSerialized = JsonConvert.SerializeObject(body);

#if DEBUG
                System.Diagnostics.Debug.WriteLine($"{endpoint}");
                System.Diagnostics.Debug.WriteLine($"Request Body: {jsonSerialized}");
                System.Diagnostics.Debug.WriteLine($"Auth headers: {GetBearer()}");
#endif
                var jsonContent = new StringContent(jsonSerialized);
                jsonContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                return Task.FromResult<HttpContent>(jsonContent);
            }
        }

        #endregion

        public void Dispose()
        {
            _client?.Dispose();
        }

        public Task<byte[]> GetResponseAsByteAsync(string endpoint, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                _client.DefaultRequestHeaders.Date = DateTime.UtcNow;
                return await _client.GetByteArrayAsync(endpoint);
            }, cancellationToken);
        }

        Task<byte[]> IHttpConnector.GetResponseAsByteAsync(string endpoint, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                _client.DefaultRequestHeaders.Date = DateTime.UtcNow;
                return await _client.GetByteArrayAsync(endpoint);
            }, cancellationToken);
        }

        Task<TOutput> IHttpConnector.Post<TOutput>(string endpoint, CancellationToken cancellationToken)
        {
            return CallWebApiAsync<TOutput>(endpoint, HttpMethodEnum.Post, cancellationToken);
        }

        Task<TOutput> IHttpConnector.PostFormUrlEncoded<TOutput>(string url, IEnumerable<KeyValuePair<string, string>> postData, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                using (var httpClient = new HttpClient())
                {
                    using (var content = new FormUrlEncodedContent(postData))
                    {
                        content.Headers.Clear();
                        content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                        HttpResponseMessage response = await httpClient.PostAsync(url, content, cancellationToken);

                        return await ManageBackendResponse<TOutput>(url, response);
                    }
                }
            }, cancellationToken);
        }
    }
}
