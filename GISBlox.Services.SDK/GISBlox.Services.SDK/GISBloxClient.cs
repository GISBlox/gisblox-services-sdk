using GISBlox.Services.SDK.Projection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK
{
   public class GISBloxClient : IDisposable
   {
      /// <summary>
      ///  Initializes a new instance of the <see cref="GISBloxClient"/> class.
      /// </summary>
      /// <param name="baseUrl">The base URL of the services API, i.e. https://services.gisblox.com</param>
      /// <param name="serviceKey">The service key.</param>
      /// <param name="timeoutSeconds">The timeout in seconds for the http requests.</param>
      protected GISBloxClient(string baseUrl, string serviceKey, long timeoutSeconds = 30)
      {
         var apiUrl = new Uri(new Uri(baseUrl), "v1/");

         var handler = new HttpClientHandler
         {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
         };

         var httpClient = new HttpClient(handler, false)
         {
            BaseAddress = apiUrl,
            Timeout = TimeSpan.FromSeconds(timeoutSeconds)            
         };
         
         httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));         
         httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
         httpClient.DefaultRequestHeaders.Add("Service-Key", serviceKey);         

         this.Projection = new ProjectionAPIClient(httpClient);
      }

      /// <summary>
      /// Create client object with specified base URL, service key and timeout.
      /// </summary>
      /// <param name="baseUrl">Base URL for the GISBlox Services resource, i.e. https://services.gisblox.com</param>
      /// <param name="serviceKey">The service key.</param>
      /// <param name="timeoutSeconds">Web request time out in seconds</param>
      /// <returns></returns>
      public static GISBloxClient CreateClient(string baseUrl, string serviceKey, long timeoutSeconds = 30)
      {
         return new GISBloxClient(baseUrl, serviceKey, timeoutSeconds);
      }

      public IProjectionAPI Projection { get;  }

      public void Dispose()
      {
         Projection.Dispose();
      }
   }
}
