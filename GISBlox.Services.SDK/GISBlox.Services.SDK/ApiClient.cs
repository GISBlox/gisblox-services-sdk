// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using Microsoft.Extensions.Caching.Memory;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GISBlox.Services.SDK
{
    /// <summary>
    /// Base class for API clients.
    /// </summary>
    public abstract class ApiClient : IDisposable
    {
        /// <summary>
        /// The HTTP client used for API requests.
        /// </summary>
        protected readonly HttpClient HttpClient;

        /// <summary>
        /// The memory cache used for caching responses.
        /// </summary>
        protected readonly IMemoryCache Cache;

        private static readonly JsonSerializerOptions JsonSerializerOptions = new() { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never };

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClient"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client to use for API requests.</param>
        /// <param name="cache">The memory cache to use for caching responses.</param>
        protected ApiClient(HttpClient httpClient, IMemoryCache cache)
        {
            HttpClient = httpClient;
            Cache = cache;

            // Enable response compression
            if (httpClient.DefaultRequestHeaders.AcceptEncoding.Count == 0)
            {
                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            }
            // Always prefer JSON
            if (httpClient.DefaultRequestHeaders.Accept.Count == 0)
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }        
                
        /// <summary>
        /// Sends a GET request to the specified URI.
        /// </summary>
        /// <typeparam name="T">The type of the response content.</typeparam>
        /// <param name="httpClient">The HTTP client to use for the request.</param>
        /// <param name="cache">The memory cache to use for caching responses.</param>
        /// <param name="requestUri">The URI of the resource to retrieve.</param>
        /// <param name="epsg">The EPSG code to use for the request.</param>
        /// <param name="customHeaders">Any custom headers to include in the request.</param>
        /// <param name="cancellationToken">The cancellation token to use for the request.</param>
        /// <returns>The deserialized response content.</returns>
        /// <exception cref="ClientApiException"></exception>
        protected static async Task<T> HttpGet<T>(HttpClient httpClient, IMemoryCache cache, string requestUri, string epsg = null, Dictionary<string, string> customHeaders = null, CancellationToken cancellationToken = default)
        {
            string cacheKey = epsg != null ? $"{requestUri}::epsg={epsg}" : requestUri;
            
            if (!cache.TryGetValue(cacheKey, out string responseContent))
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
                
                if (epsg != null) request.Headers.Add("X-EPSG", epsg);
                SetRequestHeaderValues(request, customHeaders);
                
                var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await GetErrorContentAsync(response);
                    throw new ClientApiException(!string.IsNullOrEmpty(errorContent) ? errorContent : response.ReasonPhrase, response.StatusCode);
                }
                
                responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(responseContent))
                {
                    cache.Set(cacheKey, responseContent);
                }
            }
            return JsonSerializer.Deserialize<T>(responseContent);
        }        

        /// <summary>
        /// Sends a POST request to the specified URI.
        /// </summary>
        /// <typeparam name="TBody">The type of the request body.</typeparam>
        /// <param name="httpClient">The HTTP client to use for the request.</param>
        /// <param name="requestUri">The URI of the resource to create or update.</param>
        /// <param name="body">The request body content.</param>
        /// <param name="epsg">The EPSG code to use for the request.</param>
        /// <param name="customHeaders">Any custom headers to include in the request.</param>
        /// <param name="cancellationToken">The cancellation token to use for the request.</param>
        /// <returns></returns>
        /// <exception cref="ClientApiException"></exception>
        protected static async Task HttpPost<TBody>(HttpClient httpClient, string requestUri, TBody body, string epsg = null, Dictionary<string, string> customHeaders = null, CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            
            if (epsg != null) request.Headers.Add("X-EPSG", epsg);
            SetRequestHeaderValues(request, customHeaders);

            request.Content = new StringContent(JsonSerializer.Serialize(body, JsonSerializerOptions), Encoding.UTF8, "application/json");
            var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await GetErrorContentAsync(response);
                throw new ClientApiException(!string.IsNullOrEmpty(errorContent) ? errorContent : response.ReasonPhrase, response.StatusCode);
            }
        }

        /// <summary>
        /// Sends a POST request to the specified URI.
        /// </summary>
        /// <typeparam name="TBody">The type of the request body.</typeparam>
        /// <typeparam name="TResult">The type of the response content.</typeparam>
        /// <param name="httpClient">The HTTP client to use for the request.</param>
        /// <param name="requestUri">The URI of the resource to create or update.</param>
        /// <param name="body">The request body content.</param>
        /// <param name="epsg">The EPSG code to use for the request.</param>
        /// <param name="customHeaders">Any custom headers to include in the request.</param>
        /// <param name="cancellationToken">The cancellation token to use for the request.</param>
        /// <returns>The deserialized response content.</returns>
        /// <exception cref="ClientApiException"></exception>
        protected static async Task<TResult> HttpPost<TBody, TResult>(HttpClient httpClient, string requestUri, TBody body, string epsg = null, Dictionary<string, string> customHeaders = null, CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            
            if (epsg != null) request.Headers.Add("X-EPSG", epsg);
            SetRequestHeaderValues(request, customHeaders);
            
            request.Content = new StringContent(JsonSerializer.Serialize(body, JsonSerializerOptions), Encoding.UTF8, "application/json");
            var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await GetErrorContentAsync(response);
                throw new ClientApiException(!string.IsNullOrEmpty(errorContent) ? errorContent : response.ReasonPhrase, response.StatusCode);
            }
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            return JsonSerializer.Deserialize<TResult>(responseContent);
        }

        /// <summary>
        /// Sends a POST request with a plain text body (text/plain; charset=utf-8).
        /// </summary>
        /// <param name="httpClient">The HTTP client to use for the request.</param>
        /// <param name="requestUri">The URI of the resource to create or update.</param>
        /// <param name="textBody">The plain text request body content.</param>        
        /// <param name="customHeaders">Any custom headers to include in the request.</param>
        /// <param name="cancellationToken">The cancellation token to use for the request.</param>
        /// <exception cref="ClientApiException"></exception>
        protected static async Task HttpPost(HttpClient httpClient, string requestUri, string textBody, Dictionary<string, string> customHeaders = null, CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
                        
            SetRequestHeaderValues(request, customHeaders);

            request.Content = new StringContent(textBody ?? string.Empty, Encoding.UTF8, "text/plain");
            var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await GetErrorContentAsync(response);
                throw new ClientApiException(!string.IsNullOrEmpty(errorContent) ? errorContent : response.ReasonPhrase, response.StatusCode);
            }
        }

        /// <summary>
        /// Sends a POST request with a plain text body (text/plain; charset=utf-8) and returns a response.
        /// </summary>
        /// <typeparam name="TResult">The type of the response content.</typeparam>
        /// <param name="httpClient">The HTTP client to use for the request.</param>
        /// <param name="requestUri">The URI of the resource to create or update.</param>
        /// <param name="textBody">The plain text request body content.</param>        
        /// <param name="customHeaders">Any custom headers to include in the request.</param>
        /// <param name="cancellationToken">The cancellation token to use for the request.</param>
        /// <returns>The response content.</returns>
        /// <exception cref="ClientApiException"></exception>
        protected static async Task<string> HttpPost<TResult>(HttpClient httpClient, string requestUri, string textBody, Dictionary<string, string> customHeaders = null, CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
                        
            SetRequestHeaderValues(request, customHeaders);

            request.Content = new StringContent(textBody ?? string.Empty, Encoding.UTF8, "text/plain");
            var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await GetErrorContentAsync(response);
                throw new ClientApiException(!string.IsNullOrEmpty(errorContent) ? errorContent : response.ReasonPhrase, response.StatusCode);
            }
            return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);            
        }

        /// <summary>
        /// Sets the request header values.
        /// </summary>
        /// <param name="request">The HTTP request message.</param>
        /// <param name="headers">The headers to set.</param>
        protected static void SetRequestHeaderValues(HttpRequestMessage request, Dictionary<string, string> headers)
        {
            if (headers == null) return;
            foreach (var kvp in headers)
            {
                if (request.Headers.Contains(kvp.Key))
                {
                    request.Headers.Remove(kvp.Key);
                }
                request.Headers.Add(kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        /// Gets the error content from the HTTP response.
        /// </summary>
        /// <param name="response">The HTTP response message.</param>
        /// <returns>The error content as a string.</returns>
        protected static async Task<string> GetErrorContentAsync(HttpResponseMessage response)
        {
            try { return await response.Content.ReadAsStringAsync().ConfigureAwait(false); }
            catch { return response.ReasonPhrase; }
        }

        /// <summary>
        /// Disposes the resources used by the API client.
        /// </summary>
        /// <param name="disposing">Indicates whether the method is being called from Dispose (true) or from a finalizer (false).</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                HttpClient?.Dispose();
            }
        }

        /// <summary>
        /// Disposes the resources used by the API client.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
