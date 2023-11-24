// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK
{
   #pragma warning disable CS1591
   public abstract class ApiClient : IDisposable
   {
      protected readonly HttpClient HttpClient;

      private static readonly JsonSerializerOptions JsonSerializerOptions = new() { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never };

      protected ApiClient(HttpClient httpClient)
      {
         HttpClient = httpClient;
      }

      protected static void SetRequestHeaderValue(HttpClient httpClient, string headerName, string headerValue)
      {
         if (httpClient.DefaultRequestHeaders.Contains(headerName))
         {
            httpClient.DefaultRequestHeaders.Remove(headerName);
         }
         httpClient.DefaultRequestHeaders.Add(headerName, headerValue);
      }

      protected static ClientApiException CreateApiException(HttpResponseMessage response)
      {
         string errorContent = response.Content.ReadAsStringAsync().Result;         
         return new ClientApiException(!string.IsNullOrEmpty(errorContent) ? errorContent : response.ReasonPhrase, response.StatusCode);
      }

      protected static async Task<T> HttpGet<T>(HttpClient httpClient, string requestUri, CancellationToken cancellationToken = default)
      {        
         var response = await httpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);

         if (!response.IsSuccessStatusCode)
         {
            throw CreateApiException(response);
         }

         var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
         return JsonSerializer.Deserialize<T>(responseContent);
      }    

      protected static async Task HttpPost<TBody>(HttpClient httpClient, string requestUri, TBody body, CancellationToken cancellationToken = default)
      {
         HttpContent content = new StringContent(JsonSerializer.Serialize(body, JsonSerializerOptions), Encoding.UTF8, "application/json");
         var response = await httpClient.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);

         if (!response.IsSuccessStatusCode)
         {
            throw CreateApiException(response);
         }
      }

      protected static async Task<TResult> HttpPost<TBody, TResult>(HttpClient httpClient, string requestUri, TBody body, CancellationToken cancellationToken = default)
      {
         HttpContent content = new StringContent(JsonSerializer.Serialize(body, JsonSerializerOptions), Encoding.UTF8, "application/json");
         var response = await httpClient.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);

         if (!response.IsSuccessStatusCode)
         {
            throw CreateApiException(response);
         }

         var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
         return JsonSerializer.Deserialize<TResult>(responseContent);
      }      

      protected virtual void Dispose(bool disposing)
      {
         if (disposing)
         {
            HttpClient?.Dispose();
         }
      }

      public void Dispose()
      {
         Dispose(true);
         GC.SuppressFinalize(this);
      }
   }
   #pragma warning restore CS1591
}
