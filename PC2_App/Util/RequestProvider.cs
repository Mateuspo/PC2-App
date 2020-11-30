using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PC2_App.Util
{
    public class RequestProvider 
    {
        private readonly int _timeoutSeconds = 60;
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly string _mediaType = "application/json";
        
        public async Task DeleteAsync(string uri, string token = "", TimeSpan? timeout = null)
        {
            using (var httpClient = CreateHttpClient(token, timeout))
            {
                await httpClient.DeleteAsync(uri).ConfigureAwait(continueOnCapturedContext: false);
            }
        }
        
        public async Task<TResult> GetAsync<TResult>(string uri, string token = "", TimeSpan? timeout = null)
        {
            using (var httpClient = CreateHttpClient(token, timeout))
            {
                var response = await httpClient.GetAsync(uri).ConfigureAwait(continueOnCapturedContext: false);
                
                await HandleResponse(response).ConfigureAwait(continueOnCapturedContext: false);

                string serialized = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
                var responseLegal = await Task.Run(() => JsonConvert.DeserializeObject<Response>(serialized)).ConfigureAwait(continueOnCapturedContext: false);

                TResult result = JsonConvert.DeserializeObject<TResult>(responseLegal.model.ToString());
                return result;
            }
        }
        
        public async Task<TResult> PostAsync<TResult>(string uri, object data, string token = "", TimeSpan? timeout = null)
        {
            using (var httpClient = CreateHttpClient(token, timeout))
            {
                var content = new StringContent(JsonConvert.SerializeObject(data), _encoding);
                string serialized = string.Empty;
                content.Headers.ContentType = new MediaTypeHeaderValue(_mediaType);
                try
                {
                    var response = await httpClient.PostAsync(uri, content).ConfigureAwait(continueOnCapturedContext: false);

                    await HandleResponse(response).ConfigureAwait(continueOnCapturedContext: false);

                    serialized = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);

                }
                catch (Exception ex)
                {
                    serialized = string.Empty;
                }

                var responseLegal = await Task.Run(() => JsonConvert.DeserializeObject<Response>(serialized)).ConfigureAwait(continueOnCapturedContext: false);

                TResult result = JsonConvert.DeserializeObject<TResult>(responseLegal.model.ToString());
                return result;
            }
        }        
             
        public async Task<TResult> PutAsync<TResult>(string uri, object data, string token = "", TimeSpan? timeout = null)
        {
            using (var httpClient = CreateHttpClient(token, timeout))
            {
                var content = new StringContent(JsonConvert.SerializeObject(data), _encoding);
                content.Headers.ContentType = new MediaTypeHeaderValue(_mediaType);
                
                var response = await httpClient.PutAsync(uri, content).ConfigureAwait(continueOnCapturedContext: false);
                
                await HandleResponse(response).ConfigureAwait(continueOnCapturedContext: false);
                
                string serialized = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
                var responseLegal = await Task.Run(() => JsonConvert.DeserializeObject<Response>(serialized)).ConfigureAwait(continueOnCapturedContext: false);

                TResult result = JsonConvert.DeserializeObject<TResult>(responseLegal.model.ToString());
                return result;
            }
        }
        
        private HttpClient CreateHttpClient(string token, TimeSpan? timeout)
        {
            var handler = new HttpClientHandler { CookieContainer = new CookieContainer() };
            var httpClient = new HttpClient(handler) { Timeout = timeout ?? TimeSpan.FromSeconds(_timeoutSeconds) };
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));
            
            if (!string.IsNullOrWhiteSpace(token))
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            return httpClient;
        }
        
        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                
                if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new ServiceAuthenticationException(response.StatusCode, content);
                
                throw new RequestException(response.StatusCode, content);
            }
        }

        private class Response
        {
            public string message { get; set; }
            public bool didError { get; set; }
            public string errorMessage { get; set; }
            public object model { get; set; }
        }

    [Serializable]
        private class ServiceAuthenticationException : Exception
        {
            private HttpStatusCode statusCode;
            private string content;

            public ServiceAuthenticationException()
            {
            }

            public ServiceAuthenticationException(string message) : base(message)
            {
            }

            public ServiceAuthenticationException(HttpStatusCode statusCode, string content)
            {
                this.statusCode = statusCode;
                this.content = content;
            }

            public ServiceAuthenticationException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected ServiceAuthenticationException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        [Serializable]
        private class RequestException : Exception
        {
            private HttpStatusCode statusCode;
            private string content;

            public RequestException()
            {
            }

            public RequestException(string message) : base(message)
            {
            }

            public RequestException(HttpStatusCode statusCode, string content)
            {
                this.statusCode = statusCode;
                this.content = content;
            }

            public RequestException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected RequestException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }
}
