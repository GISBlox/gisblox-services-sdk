// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using Microsoft.Extensions.Caching.Memory;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

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

      /// <summary>
      /// Provides the default options used for JSON serialization and deserialization with web defaults.
      /// </summary>
      private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web)
      {
         DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never
      };

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

         if (!cache.TryGetValue(cacheKey, out T cachedResult))
         {
            using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            if (epsg != null) request.Headers.Add("X-EPSG", epsg);
            SetRequestHeaderValues(request, customHeaders);

            using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            await EnsureSuccessStatusCodeAsync(response, cancellationToken);

            cachedResult = await response.Content.ReadFromJsonAsync<T>(JsonSerializerOptions, cancellationToken).ConfigureAwait(false);
            if (cachedResult is not null)
            {
               cache.Set(cacheKey, cachedResult);
            }
         }
         return cachedResult;
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

         request.Content = JsonContent.Create(body, options: JsonSerializerOptions);
         using var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

         await EnsureSuccessStatusCodeAsync(response, cancellationToken);
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

         request.Content = JsonContent.Create(body, options: JsonSerializerOptions);
         using var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

         await EnsureSuccessStatusCodeAsync(response, cancellationToken);

         return await response.Content.ReadFromJsonAsync<TResult>(JsonSerializerOptions, cancellationToken).ConfigureAwait(false);
      }

      /// <summary>
      /// Sends a DELETE request to the specified URI.
      /// </summary>
      /// <param name="httpClient">The HTTP client to use for the request.</param>
      /// <param name="requestUri">The URI of the resource to delete.</param> 
      /// <param name="customHeaders">Any custom headers to include in the request.</param>
      /// <param name="cancellationToken">The cancellation token to use for the request.</param>
      /// <returns>A task representing the asynchronous delete operation.</returns>
      /// <exception cref="ClientApiException">Thrown when the server returns an error response.</exception>
      protected static async Task HttpDelete(HttpClient httpClient, string requestUri, Dictionary<string, string> customHeaders = null, CancellationToken cancellationToken = default)
      {
         using var request = new HttpRequestMessage(HttpMethod.Delete, requestUri);

         SetRequestHeaderValues(request, customHeaders);

         using var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

         await EnsureSuccessStatusCodeAsync(response, cancellationToken);
      }

      /// <summary>
      /// Sends a DELETE request to the specified URI and returns a deserialized response.
      /// </summary>
      /// <typeparam name="TResult">The type of the response content.</typeparam>
      /// <param name="httpClient">The HTTP client to use for the request.</param>
      /// <param name="requestUri">The URI of the resource to delete.</param> 
      /// <param name="customHeaders">Any custom headers to include in the request.</param>
      /// <param name="cancellationToken">The cancellation token to use for the request.</param>
      /// <returns>The deserialized response content.</returns>
      /// <exception cref="ClientApiException">Thrown when the server returns an error response.</exception>
      protected static async Task<TResult> HttpDelete<TResult>(HttpClient httpClient, string requestUri, Dictionary<string, string> customHeaders = null, CancellationToken cancellationToken = default)
      {
         using var request = new HttpRequestMessage(HttpMethod.Delete, requestUri);

         SetRequestHeaderValues(request, customHeaders);

         using var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

         await EnsureSuccessStatusCodeAsync(response, cancellationToken);

         return await response.Content.ReadFromJsonAsync<TResult>(JsonSerializerOptions, cancellationToken).ConfigureAwait(false);
      }

      /// <summary>
      /// Sends a POST request with a JSON string body.
      /// </summary>
      /// <param name="httpClient">The HTTP client to use for the request.</param>
      /// <param name="requestUri">The URI of the resource to create or update.</param>
      /// <param name="jsonContent">The JSON string to use as the request body.</param>
      /// <param name="customHeaders">Any custom headers to include in the request.</param>
      /// <param name="cancellationToken">The cancellation token to use for the request.</param>
      /// <returns>A task representing the asynchronous operation.</returns>
      /// <exception cref="ClientApiException"></exception>
      protected static async Task HttpPost(HttpClient httpClient, string requestUri, string jsonContent, Dictionary<string, string> customHeaders = null, CancellationToken cancellationToken = default)
      {
         using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

         SetRequestHeaderValues(request, customHeaders);

         // Directly use the string as JSON content without re-serializing
         var content = new StringContent(jsonContent ?? string.Empty, Encoding.UTF8, "application/json");
         request.Content = content;

         using var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

         await EnsureSuccessStatusCodeAsync(response, cancellationToken);
      }

      /// <summary>
      /// Sends a POST request with a JSON string body and returns a deserialized response.
      /// </summary>
      /// <typeparam name="TResult">The type of the response content.</typeparam>
      /// <param name="httpClient">The HTTP client to use for the request.</param>
      /// <param name="requestUri">The URI of the resource to create or update.</param>
      /// <param name="jsonContent">The JSON string to use as the request body.</param>
      /// <param name="customHeaders">Any custom headers to include in the request.</param>
      /// <param name="cancellationToken">The cancellation token to use for the request.</param>
      /// <returns>The deserialized response content.</returns>
      /// <exception cref="ClientApiException"></exception>
      protected static async Task<TResult> HttpPost<TResult>(HttpClient httpClient, string requestUri, string jsonContent, Dictionary<string, string> customHeaders = null, CancellationToken cancellationToken = default)
      {
         using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

         SetRequestHeaderValues(request, customHeaders);

         // Directly use the string as JSON content without re-serializing
         var content = new StringContent(jsonContent ?? string.Empty, Encoding.UTF8, "application/json");
         request.Content = content;

         using var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

         await EnsureSuccessStatusCodeAsync(response, cancellationToken);

         return await response.Content.ReadFromJsonAsync<TResult>(JsonSerializerOptions, cancellationToken).ConfigureAwait(false);
      }

      /// <summary>
      /// Sends a POST request with a stream as the request body.
      /// </summary>
      /// <param name="httpClient">The HTTP client to use for the request.</param>
      /// <param name="requestUri">The URI of the resource to create or update.</param>
      /// <param name="streamContent">The stream content to use as the request body.</param>
      /// <param name="contentType">The media type of the content stream. Defaults to application/octet-stream.</param>
      /// <param name="customHeaders">Any custom headers to include in the request.</param>
      /// <param name="cancellationToken">The cancellation token to use for the request.</param>
      /// <returns>A task representing the asynchronous operation.</returns>
      /// <exception cref="ClientApiException"></exception>
      /// <exception cref="ArgumentNullException">Thrown when streamContent is null.</exception>
      protected static async Task HttpPost(HttpClient httpClient, string requestUri, Stream streamContent, string contentType = "application/octet-stream", Dictionary<string, string> customHeaders = null, CancellationToken cancellationToken = default)
      {
         ArgumentNullException.ThrowIfNull(streamContent);

         using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
         SetRequestHeaderValues(request, customHeaders);

         HttpContent content;
         if (streamContent is MemoryStream ms && ms.TryGetBuffer(out ArraySegment<byte> buffer) && buffer.Array != null)
         {
            // Respect current position; send remaining bytes only
            int remaining = (int)(ms.Length - ms.Position);
            int start = buffer.Offset + (int)ms.Position;
            content = new ByteArrayContent(buffer.Array, start, remaining);
         }
         else
         {
            // Fallback: copy the stream content into a byte[]
            byte[] contentBytes = await ReadStreamBytes(streamContent, cancellationToken);
            content = new ByteArrayContent(contentBytes);
         }
         content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
         request.Content = content;

         using var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

         await EnsureSuccessStatusCodeAsync(response, cancellationToken);
      }

      /// <summary>
      /// Sends a POST request with a stream as the request body and returns the deserialized response.
      /// </summary>
      /// <typeparam name="TResult">The type of the response content.</typeparam>
      /// <param name="httpClient">The HTTP client to use for the request.</param>
      /// <param name="requestUri">The URI of the resource to create or update.</param>
      /// <param name="streamContent">The stream content to use as the request body.</param>
      /// <param name="contentType">The media type of the content stream. Defaults to application/octet-stream.</param>
      /// <param name="customHeaders">Any custom headers to include in the request.</param>
      /// <param name="cancellationToken">The cancellation token to use for the request.</param>
      /// <returns>The deserialized response content.</returns>
      /// <exception cref="ClientApiException"></exception>
      /// <exception cref="ArgumentNullException">Thrown when streamContent is null.</exception>
      protected static async Task<TResult> HttpPost<TResult>(HttpClient httpClient, string requestUri, Stream streamContent, string contentType = "application/octet-stream", Dictionary<string, string> customHeaders = null, CancellationToken cancellationToken = default)
      {
         ArgumentNullException.ThrowIfNull(streamContent);

         using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
         SetRequestHeaderValues(request, customHeaders);

         HttpContent content;
         if (streamContent is MemoryStream ms && ms.TryGetBuffer(out ArraySegment<byte> buffer) && buffer.Array != null)
         {
            // Respect current position; send remaining bytes only
            int remaining = (int)(ms.Length - ms.Position);
            int start = buffer.Offset + (int)ms.Position;
            content = new ByteArrayContent(buffer.Array, start, remaining);
         }
         else
         {
            // Fallback: copy the stream content into a byte[]
            byte[] contentBytes = await ReadStreamBytes(streamContent, cancellationToken);
            content = new ByteArrayContent(contentBytes);
         }
         content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
         request.Content = content;

         using var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

         await EnsureSuccessStatusCodeAsync(response, cancellationToken);

         return await response.Content.ReadFromJsonAsync<TResult>(JsonSerializerOptions, cancellationToken).ConfigureAwait(false);
      }

      /// <summary>
      /// Downloads the content of a JSON file from the specified URI.
      /// </summary>
      /// <param name="httpClient">The HTTP client to use for the request.</param>
      /// <param name="cache">The memory cache to use for caching responses.</param>
      /// <param name="requestUri">The URI of the JSON file to download.</param>
      /// <param name="useCache">Whether to use cache for the downloaded file.</param>
      /// <param name="customHeaders">Any custom headers to include in the request.</param>
      /// <param name="cancellationToken">The cancellation token to use for the request.</param>
      /// <returns>The JSON file content as a string.</returns>
      /// <exception cref="ClientApiException"></exception>
      protected static async Task<string> DownloadJsonFileData(HttpClient httpClient, IMemoryCache cache, string requestUri, bool useCache = true, Dictionary<string, string> customHeaders = null, CancellationToken cancellationToken = default)
      {
         if (useCache && cache.TryGetValue(requestUri, out string cachedContent))
         {
            return cachedContent;
         }

         using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

         // Set Accept header explicitly for JSON files
         request.Headers.Accept.Clear();
         request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

         SetRequestHeaderValues(request, customHeaders);

         using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

         try
         {
            await EnsureSuccessStatusCodeAsync(response, cancellationToken);
         }
         catch (ClientApiException ex)
         {
            throw new ClientApiException($"Failed to download JSON file. {ex.Message}", ex.StatusCode);
         }

         var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

         if (useCache && !string.IsNullOrEmpty(responseContent))
         {
            cache.Set(requestUri, responseContent);
         }
         return responseContent;
      }

      /// <summary>
      /// Downloads a file from the specified URI and saves it to disk.
      /// </summary>
      /// <param name="httpClient">The HTTP client to use for the request.</param>
      /// <param name="requestUri">The URI of the file to download.</param>
      /// <param name="filePath">The path where the file should be saved.</param>
      /// <param name="contentType">The media type to accept (e.g., application/json, application/octet-stream). Defaults to application/json.</param>
      /// <param name="customHeaders">Any custom headers to include in the request.</param>
      /// <param name="cancellationToken">The cancellation token to use for the request.</param> 
      /// <exception cref="ClientApiException"></exception>
      protected static async Task DownloadFileToDisk(HttpClient httpClient, string requestUri, string filePath, string contentType = "application/json", Dictionary<string, string> customHeaders = null, CancellationToken cancellationToken = default)
      {
         using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

         // Set Accept header explicitly for the requested content type
         request.Headers.Accept.Clear();
         request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

         SetRequestHeaderValues(request, customHeaders);

         using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

         try
         {
            await EnsureSuccessStatusCodeAsync(response, cancellationToken);
         }
         catch (ClientApiException ex)
         {
            throw new ClientApiException($"Failed to download file. {ex.Message}", ex.StatusCode);
         }

         // Ensure directory exists
         string directory = Path.GetDirectoryName(filePath);
         if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
         {
            Directory.CreateDirectory(directory);
         }

         // Write the file
         using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
         {
            await response.Content.CopyToAsync(fileStream, cancellationToken);
         }
      }

      /// <summary>
      /// Reads the entire content of a stream into a byte array.
      /// </summary>
      /// <param name="streamContent">The stream to read from.</param>
      /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
      /// <returns>A byte array containing the entire content of the stream.</returns>
      protected static async Task<byte[]> ReadStreamBytes(Stream streamContent, CancellationToken cancellationToken)
      {
         // Read the entire stream into a byte array to ensure we have the full content
         // regardless of what happens to the original stream after this method returns
         if (streamContent is MemoryStream ms && ms.TryGetBuffer(out ArraySegment<byte> buffer) && buffer.Array != null)
         {
            // Respect current position; copy only the remaining bytes
            int remaining = (int)(ms.Length - ms.Position);

            if (remaining == buffer.Count && ms.Position ==0 && buffer.Offset ==0)
            {
               // Common case: entire buffer contains the content - return a copy to avoid exposing internal buffer
               var copy = new byte[buffer.Count];
               Buffer.BlockCopy(buffer.Array, buffer.Offset, copy,0, buffer.Count);
               return copy;
            }

            var result = new byte[remaining];
            Buffer.BlockCopy(buffer.Array, buffer.Offset + (int)ms.Position, result,0, remaining);
            return result;
         }

         using var memoryStream = new MemoryStream();
         await streamContent.CopyToAsync(memoryStream, cancellationToken).ConfigureAwait(false);
         return memoryStream.ToArray();
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
            // Use TryAddWithoutValidation to avoid format exceptions and extra remove/add overhead
            if (!request.Headers.TryAddWithoutValidation(kvp.Key, kvp.Value))
            {
               // If it cannot be added to request headers, and content exists, try content headers
               request.Content?.Headers.TryAddWithoutValidation(kvp.Key, kvp.Value);
            }
         }
      }

      /// <summary>
      /// Ensures that the HTTP response has a success status code, otherwise throws a ClientApiException with error details.
      /// </summary>
      /// <param name="response">The HTTP response message to check.</param>
      /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
      /// <returns>A task representing the asynchronous operation.</returns>
      /// <exception cref="ClientApiException">Thrown when the response does not have a success status code.</exception>
      protected static async Task EnsureSuccessStatusCodeAsync(HttpResponseMessage response, CancellationToken cancellationToken = default)
      {
         if (!response.IsSuccessStatusCode)
         {
            string errorContent = await GetErrorContentAsync(response, cancellationToken);
            throw new ClientApiException(!string.IsNullOrEmpty(errorContent) ? errorContent : response.ReasonPhrase, response.StatusCode);
         }
      }

      /// <summary>
      /// Gets the error content from the HTTP response.
      /// </summary>
      /// <param name="response">The HTTP response message.</param>
      /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
      /// <returns>The error content as a string.</returns>
      protected static async Task<string> GetErrorContentAsync(HttpResponseMessage response, CancellationToken cancellationToken = default)
      {
         try { return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false); }
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
